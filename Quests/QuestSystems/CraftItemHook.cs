using System.Linq;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool CraftItemHookPredicate(Item item);

public delegate void CraftItemHookCallback(Item item, RecipeItemCreationContext context);

public abstract class CraftItemHook : GlobalItem
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should be invoked when an item is crafted.
    /// </summary>
    /// <remarks>
    ///     If <see langword="null"/>, evaluates as <see langword="true"/>.
    /// </remarks>
    public CraftItemHookPredicate Predicate { get; init; }
    
    /// <summary>
    ///     Gets the callback that is invoked when an item is crafted and the predicate matches.
    /// </summary>
    public CraftItemHookCallback Callback { get; init; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CraftItemHook"/> class with the specified predicate and callback.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when an item is crafted.
    /// </param>
    /// <param name="callback">
    ///     The callback that is invoked when an item is crafted and the predicate matches.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    public CraftItemHook(CraftItemHookPredicate predicate, CraftItemHookCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="CraftItemHook"/> class with the specified callback.
    /// </summary>
    /// <param name="callback">
    ///     The callback that is invoked when an item is crafted and the predicate matches.
    /// </param>
    /// <remarks>
    ///     Checks for any item crafted, regardless of type.
    /// </remarks>
    public CraftItemHook(CraftItemHookCallback callback) : this(null, callback) { }

    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => Predicate?.Invoke(entity) ?? true;

    public override void OnCreated(Item item, ItemCreationContext context)
    {
        if (context is not RecipeItemCreationContext recipe)
            return;
        
        Callback.Invoke(item, recipe);
    }

    protected static bool Match(Item item, int match) => item.type == match;

    protected static bool Match(Item item, bool[] set) => set[item.type];

    protected static bool Match(Item item, params int[] matches) => matches.Contains(item.type);

    protected static bool Match<T>(Item item) where T : ModItem => item.type == ModContent.ItemType<T>();
}

public abstract class CraftItemHook<TQuest> : CraftItemHook
    where TQuest : Quest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CraftItemHook{TQuest}"/> class with the specified predicate.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when an item is crafted.
    /// </param>
    public CraftItemHook(CraftItemHookPredicate predicate) : base(predicate, Complete) { }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="CraftItemHook{TQuest}"/> class.
    /// </summary>
    /// <remarks>
    ///     Checks for any item crafted, regardless of type.
    /// </remarks>
    public CraftItemHook() : base(Complete) { }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="CraftItemHook{TQuest}"/> class with the specified item type.
    /// </summary>
    /// <param name="type">
    ///     The type of the item that should trigger this hook when crafted.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public CraftItemHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        Predicate = item => Match(item, type);
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="CraftItemHook{TQuest}"/> class with the specified set of item types.
    /// </summary>
    /// <param name="set">
    ///     The set of item types that should trigger this hook when crafted.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="set"/> is <see langword="null"/>.
    /// </exception>
    public CraftItemHook(bool[] set) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(set);

        Predicate = item => Match(item, set);
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="CraftItemHook{TQuest}"/> class with the specified array of item types.
    /// </summary>
    /// <param name="types">
    ///     The array of item types that should trigger this hook when crafted.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="types"/> is <see langword="null"/>.
    /// </exception>
    public CraftItemHook(params int[] types) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(types);

        Predicate = item => Match(item, types);
    }

    protected static void Complete(Item item, RecipeItemCreationContext context) => QuestManager.MarkComplete<TQuest>();
}

public abstract class CraftItemHook<TQuest, TModItem> : CraftItemHook<TQuest> 
    where TQuest : Quest 
    where TModItem : ModItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CraftItemHook{TQuest, TModItem}"/> class.
    /// </summary>
    public CraftItemHook() : base(ModContent.ItemType<TModItem>()) { }
}