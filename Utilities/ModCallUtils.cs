using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuestBooks.Utilities
{
    public class DynamicDict<T> : DynamicDict
    {
        public DynamicDict(object target) : this(target, null) { }

        public DynamicDict(object target, Action<object> parentWriteBack) : base(parentWriteBack)
        {
            ArgumentNullException.ThrowIfNull(target);
            Target = target;
        }

        public static explicit operator T(DynamicDict<T> self) => (T)self.Target;

        public override string ToString() => $"DynamicDict<{typeof(T).Name}>({Target.GetType().Name})";
    }

    /// <summary>
    /// A reflection-based dictionary adapter over an arbitrary object's public fields, properties, and methods.<br/>
    /// <br/>
    /// Usage:<br/>
    /// <br/><c>
    ///     // Fields / Properties<br/>
    ///     var dict = DynamicDict.Wrap(myObject);      // or: new DynamicDict(myObject)<br/>
    ///     int x = (int)dict["SomeIntField"];<br/>
    ///     dict["SomeIntField"] = 42;<br/>
    ///     var nested = dict["SomeComplexField"];      // itself a DynamicDict<br/>
    ///     dict["SomeComplexField"]["InnerField"] = 5; // chained indexing<br/>
    ///     <br/>
    ///     // Methods - two equivalent ways to call them:<br/>
    ///     var result = dict.Invoke("Add", 2, 3);<br/>
    ///     var addFn = (Func`object[], object`)dict["Add"];<br/>
    ///     var result2 = addFn(new object[] { 2, 3 });
    /// </c>
    /// </summary>
    public class DynamicDict : IDictionary<string, object>
    {
        // Reflected member info is cached per-Type so repeated wrapping is cheap.
        private static readonly ConcurrentDictionary<Type, Dictionary<string, MemberEntry>> _cache = new();

        // Called (if non-null) after any set on this instance, to push our (possibly mutated,
        // possibly boxed-struct) Target back into whatever member of the parent it came from.
        protected readonly Action<object> _parentWriteBack;

        protected DynamicDict(Action<object> parentWriteBack) =>_parentWriteBack = parentWriteBack;

        /// <summary>
        /// The live object (or boxed struct copy) this instance reflects over.
        /// </summary>
        public object Target { get; init; }

        public static DynamicDict Wrap(object target)
        {
            MethodInfo wrapMethod = typeof(DynamicDict).GetMethod(nameof(WrapGeneric), BindingFlags.Public | BindingFlags.Static);
            MethodInfo wrapGeneric = wrapMethod.MakeGenericMethod(target.GetType());
            return (DynamicDict)wrapGeneric.Invoke(null, [target]);
        }

        public static DynamicDict<T> WrapGeneric<T>(T target) => new(target);

        private static DynamicDict<T> WrapMember<T>(T target, Action<object> parentWriteBack) => new(target, parentWriteBack);

        private Dictionary<string, MemberEntry> Members => _cache.GetOrAdd(Target.GetType(), BuildMembers);

        private static Dictionary<string, MemberEntry> BuildMembers(Type type)
        {
            var result = new Dictionary<string, MemberEntry>();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            foreach (var field in type.GetFields(flags))
            {
                result[field.Name] = MemberEntry.ForData(
                    field.FieldType,
                    t => field.GetValue(t),
                    (t, v) => field.SetValue(t, v));
            }

            foreach (var prop in type.GetProperties(flags))
            {
                if (prop.GetIndexParameters().Length > 0)
                    continue; // skip indexer properties (this[int] etc.)

                if (!prop.CanRead)
                    continue;

                Action<object, object> setter = prop.CanWrite
                    ? (t, v) => prop.SetValue(t, v)
                    : null;

                result[prop.Name] = MemberEntry.ForData(prop.PropertyType, t => prop.GetValue(t), setter);
            }

            // Methods: group overloads by name. Skip property/event accessors and operators
            // (IsSpecialName), open generic method definitions (kept simple), and methods declared
            // directly on System.Object (ToString/Equals/GetHashCode/GetType) to reduce noise.
            var methodGroups = type.GetMethods(flags)
                .Where(m => !m.IsSpecialName)
                .Where(m => m.DeclaringType != typeof(object))
                .Where(m => !m.IsGenericMethodDefinition)
                .GroupBy(m => m.Name);

            foreach (var group in methodGroups)
            {
                if (result.ContainsKey(group.Key))
                    continue; // a field/property with the same name wins

                result[group.Key] = MemberEntry.ForMethod([..group]);
            }

            return result;
        }

        private static bool IsSimple(Type type)
        {
            var t = Nullable.GetUnderlyingType(type) ?? type;
            return t.IsPrimitive
                || t.IsEnum
                || t == typeof(string)
                || t == typeof(decimal)
                || t == typeof(DateTime)
                || t == typeof(DateTimeOffset)
                || t == typeof(TimeSpan)
                || t == typeof(Guid);
        }

        private object WrapValue(object value, string memberName)
        {
            if (value == null)
                return null;

            var valueType = value.GetType();
            if (IsSimple(valueType))
                return value;

            // Setting through the nested dict just needs to set OUR member; SetMember below
            // already takes care of bubbling further up if our own Target is a value type
            void WriteBack(object newValue) => SetMember(memberName, newValue);

            MethodInfo wrapMethod = typeof(DynamicDict).GetMethod(nameof(WrapMember), BindingFlags.Public | BindingFlags.Static);
            MethodInfo wrapGeneric = wrapMethod.MakeGenericMethod(valueType);
            return wrapGeneric.Invoke(null, [value, (Action<object>)WriteBack]);
        }

        private void SetMember(string key, object value)
        {
            if (!Members.TryGetValue(key, out var entry))
                throw new KeyNotFoundException($"'{key}' is not a public field, property, or method of {Target.GetType().Name}.");

            if (entry.Kind == MemberKind.Method)
                throw new NotSupportedException($"'{key}' on {Target.GetType().Name} is a method, not a settable member. Use Invoke(\"{key}\", ...) or dict[\"{key}\"] as a delegate to call it.");

            if (entry.Setter == null)
                throw new NotSupportedException($"'{key}' on {Target.GetType().Name} has no accessible setter.");

            // Allow assigning a DynamicDict directly (unwraps to its underlying object)
            if (value is DynamicDict nested)
                value = nested.Target;

            // Best-effort coercion for convertible mismatches (e.g. an int field set from a long)
            if (value != null && !entry.MemberType.IsInstanceOfType(value))
            {
                var underlying = Nullable.GetUnderlyingType(entry.MemberType) ?? entry.MemberType;

                if (typeof(IConvertible).IsAssignableFrom(underlying) && value is IConvertible)
                    value = Convert.ChangeType(value, underlying);
            }

            entry.Setter(Target, value);

            // If Target is a boxed struct, that set only mutated OUR local box - propagate the
            // updated copy up to whatever member of the parent it lives in.
            if (Target.GetType().IsValueType)
                _parentWriteBack?.Invoke(Target);
        }

        public delegate object MethodDelegate(object[] args);

        public object this[string key]
        {
            get
            {
                if (!Members.TryGetValue(key, out var entry))
                    throw new KeyNotFoundException($"'{key}' is not a public field, property, or method of {Target.GetType().Name}.");

                if (entry.Kind == MemberKind.Method)
                {
                    // Return a callable delegate: ((Func<object[],object>)dict["Foo"])(args)
                    return (MethodDelegate)(args => Invoke(key, args ?? []));
                }

                return WrapValue(entry.Getter(Target), key);
            }
            set => SetMember(key, value);
        }

        /// <summary>
        /// Invokes the named method on the wrapped object, auto-selecting the best-matching overload (if any) for the given arguments.<br/>
        /// DynamicDict-wrapped arguments are automatically unwrapped to their underlying object.
        /// </summary>
        public object Invoke(string methodName, params object[] args)
        {
            args ??= [];

            if (!Members.TryGetValue(methodName, out var entry) || entry.Kind != MemberKind.Method)
                throw new NotSupportedException($"'{methodName}' is not a method on {Target.GetType().Name}.");

            var unwrapped = args.Select(a => a is DynamicDict d ? d.Target : a).ToArray();

            var method = SelectBestOverload(entry.Overloads, unwrapped)
                ?? throw new MissingMethodException($"No overload of '{methodName}' on {Target.GetType().Name} matches the given {unwrapped.Length} argument(s).");

            var finalArgs = BindArguments(method, unwrapped);
            return method.Invoke(Target, finalArgs);
        }

        private static MethodInfo SelectBestOverload(List<MethodInfo> overloads, object[] args)
        {
            MethodInfo best = null;
            int bestScore = int.MinValue;

            foreach (var method in overloads)
            {
                var parameters = method.GetParameters();
                if (args.Length > parameters.Length)
                    continue;

                if (args.Length < parameters.Length && !parameters.Skip(args.Length).All(p => p.IsOptional))
                    continue;

                int score = 0;
                bool valid = true;

                for (int i = 0; i < args.Length; i++)
                {
                    var arg = args[i];
                    var paramType = parameters[i].ParameterType;

                    if (arg == null)
                    {
                        if (paramType.IsValueType && Nullable.GetUnderlyingType(paramType) == null)
                        {
                            valid = false;
                            break;
                        }

                        score += 1;
                    }

                    else if (paramType.IsInstanceOfType(arg))
                        score += 3;

                    else
                    {
                        var underlying = Nullable.GetUnderlyingType(paramType) ?? paramType;
                        if (typeof(IConvertible).IsAssignableFrom(underlying) && arg is IConvertible)
                            score += 1;

                        else
                        {
                            valid = false;
                            break;
                        }
                    }
                }

                if (!valid)
                    continue;

                // Prefer overloads that don't rely on defaulted trailing parameters
                score -= (parameters.Length - args.Length);

                if (score > bestScore)
                {
                    bestScore = score;
                    best = method;
                }
            }

            return best;
        }

        private static object[] BindArguments(MethodInfo method, object[] args)
        {
            var parameters = method.GetParameters();
            var result = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                if (i >= args.Length)
                {
                    result[i] = parameters[i].HasDefaultValue ? parameters[i].DefaultValue : Type.Missing;
                    continue;
                }

                var arg = args[i];
                var paramType = parameters[i].ParameterType;

                if (arg != null && !paramType.IsInstanceOfType(arg))
                {
                    var underlying = Nullable.GetUnderlyingType(paramType) ?? paramType;
                    if (typeof(IConvertible).IsAssignableFrom(underlying) && arg is IConvertible)
                        arg = Convert.ChangeType(arg, underlying);
                }

                result[i] = arg;
            }

            return result;
        }

        public ICollection<string> Keys => Members.Keys;

        public ICollection<object> Values => [..Members.Keys.Select(k => this[k])];

        public int Count => Members.Count;

        public bool IsReadOnly => false;

        public bool ContainsKey(string key) => Members.ContainsKey(key);

        public bool TryGetValue(string key, out object value)
        {
            if (!Members.ContainsKey(key))
            {
                value = null;
                return false;
            }

            value = this[key];
            return true;
        }

        public void Add(string key, object value)
        {
            if (!ContainsKey(key))
                throw new NotSupportedException("Cannot add new members; only existing fields/properties can be set.");

            this[key] = value;
        }

        public void Add(KeyValuePair<string, object> item) => Add(item.Key, item.Value);

        public bool Remove(string key) =>
            throw new NotSupportedException("Cannot remove members from a reflected object.");

        public bool Remove(KeyValuePair<string, object> item) =>
            throw new NotSupportedException("Cannot remove members from a reflected object.");

        public void Clear() =>
            throw new NotSupportedException("Cannot clear members from a reflected object.");

        public bool Contains(KeyValuePair<string, object> item) =>
            ContainsKey(item.Key) && Equals(this[item.Key], item.Value);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            foreach (var kvp in this)
                array[arrayIndex++] = kvp;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var key in Members.Keys)
                yield return new KeyValuePair<string, object>(key, this[key]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString() => $"DynamicDict({Target.GetType().Name})";

        private enum MemberKind { Data, Method }

        private sealed class MemberEntry
        {
            public MemberKind Kind { get; private set; }

            // Data (field/property) members
            public Type MemberType { get; private set; }
            public Func<object, object> Getter { get; private set; }
            public Action<object, object> Setter { get; private set; }

            // Method members
            public List<MethodInfo> Overloads { get; private set; }

            public static MemberEntry ForData(Type memberType, Func<object, object> getter, Action<object, object> setter) =>
                new() { Kind = MemberKind.Data, MemberType = memberType, Getter = getter, Setter = setter };

            public static MemberEntry ForMethod(List<MethodInfo> overloads) =>
                new() { Kind = MemberKind.Method, Overloads = overloads };
        }
    }

}
