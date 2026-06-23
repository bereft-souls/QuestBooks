using System.Linq;
using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool CraftItemHookPredicate(Item item);

public delegate void CraftItemHookCallback(Item item, RecipeItemCreationContext context);

public abstract class CraftItemHook : GlobalItem
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should fire when an item is crafted.
    /// </summary>
    public CraftItemHookPredicate Predicate { get; init; }
    
    /// <summary>
    ///     Gets the callback that is invoked when an item is crafted and the predicate matches.
    /// </summary>
    public CraftItemHookCallback Callback { get; init; }

    public CraftItemHook(CraftItemHookPredicate predicate, CraftItemHookCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }
    
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

public abstract class CraftItemHook<TQuest> : CraftItemHook where TQuest : Quest
{
    public CraftItemHook() : base(Complete) { }

    public CraftItemHook(CraftItemHookPredicate predicate) : base(predicate, Complete) { }
    
    public CraftItemHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        Predicate = item => Match(item, type);
    }
    
    public CraftItemHook(bool[] set) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(set);

        Predicate = item => Match(item, set);
    }
    
    public CraftItemHook(params int[] types) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(types);

        Predicate = item => Match(item, types);
    }

    protected static void Complete(Item item, RecipeItemCreationContext context) => QuestManager.MarkComplete<TQuest>();
}

public abstract class CraftItemHook<TQuest, TModItem> : CraftItemHook<TQuest> where TQuest : Quest where TModItem : ModItem
{
    public CraftItemHook() => Predicate = static item => Match<TModItem>(item);
}