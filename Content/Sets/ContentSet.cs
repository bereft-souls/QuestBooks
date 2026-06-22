using System.Collections;
using System.Collections.Generic;

namespace QuestBooks.Content.Sets;

public sealed class ContentSet : IEnumerable<int>
{
    /// <summary>
    ///     The maximum number of types that can be stored in a <see cref="ContentSet"/>.
    /// </summary>
    public const int Length = ushort.MaxValue + 1;
    
    private readonly BitArray flags;
    
    public ContentSet() => flags = new BitArray(Length);

    private ContentSet(BitArray flags)
    {
        ArgumentNullException.ThrowIfNull(flags);

        this.flags = flags;
    }
    
    /// <summary>
    ///     Gets or sets a value indicating whether the set contains the specified type.
    /// </summary>
    /// <param name="type">
    ///     The type to check for.
    /// </param>
    public bool this[int type]
    {
        get => Contains(type);
        set => Set(type, value);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < flags.Length; i++)
            if (flags[i])
                yield return i;
    }

    /// <summary>
    ///     Adds the specified type to the set.
    /// </summary>
    /// <param name="type">
    ///     The type to add to the set.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public void Add(int type)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        
        Set(type, true);
    }
    
    /// <summary>
    ///     Removes the specified type from the set.
    /// </summary>
    /// <param name="type">
    ///     The type to remove from the set.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public void Remove(int type)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        
        Set(type, false);
    }

    /// <summary>
    ///     Sets the specified type in the set to the specified value.
    /// </summary>
    /// <param name="type">
    ///     The type to set.
    /// </param>
    /// <param name="value">
    ///     The value to set the type to.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public void Set(int type, bool value)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        
        flags[type] = value;
    }
    
    /// <summary>
    ///     Determines whether the set contains the specified type.
    /// </summary>
    /// <param name="type">
    ///     The type to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the set contains the specified type; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public bool Contains(int type)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        return flags[type];
    }

    /// <summary>
    ///     Returns a new <see cref="ContentSet"/> that contains all the types that are in either this set or the other set.
    /// </summary>
    /// <param name="other">
    ///     The set to combine with this set.
    /// </param>
    /// <returns>
    ///     A new <see cref="ContentSet"/> that contains all the types that are in either this set or the other set.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="other"/> is <see langword="null"/>.
    /// </exception>
    public ContentSet Union(ContentSet other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return new ContentSet(new BitArray(flags).Or(other.flags));
    }
    
    /// <summary>
    ///     Returns a new <see cref="ContentSet"/> that contains only the types that are in both this set and the other set.
    /// </summary>
    /// <param name="other">
    ///     The set to compare with this set.
    /// </param>
    /// <returns>
    ///     A new <see cref="ContentSet"/> that contains only the types that are in both this set and the other set.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="other"/> is <see langword="null"/>.
    /// </exception>
    public ContentSet Intersect(ContentSet other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return new ContentSet(new BitArray(flags).And(other.flags));
    }
    
    /// <summary>
    ///     Returns a new <see cref="ContentSet"/> that contains the types that are in this set but not in the other set.
    /// </summary>
    /// <param name="other">
    ///     The set to compare with this set.
    /// </param>
    /// <returns>
    ///     A new <see cref="ContentSet"/> that contains the types that are in this set but not in the other set.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="other"/> is <see langword="null"/>.
    /// </exception>

    public ContentSet Except(ContentSet other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return new ContentSet(new BitArray(flags).And(new BitArray(other.flags).Not()));
    }
    
    public static ContentSet operator |(ContentSet left, ContentSet right) => left.Union(right);
    
    public static ContentSet operator &(ContentSet left, ContentSet right) => left.Intersect(right);
    
    public static ContentSet operator -(ContentSet left, ContentSet right) => left.Except(right);
}