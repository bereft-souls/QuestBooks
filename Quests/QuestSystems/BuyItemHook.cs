using QuestBooks.Quests.VanillaQuests;
using QuestBooks.Systems;
using Terraria.DataStructures;
using Terraria.ModLoader.Core;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool BuyItemPredicate(Item item);

public delegate void BuyItemCallback(Item item, BuyItemCreationContext context);

public abstract class BuyItemHook : GlobalItem
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should be invoked when an item is bought.
    /// </summary>
    /// <remarks>
    ///     If <see langword="null"/>, evaluates as <see langword="true"/>.
    /// </remarks>
    public BuyItemPredicate Predicate { get; init; }

    /// <summary>
    ///     Gets the callback that is invoked when an item is bought and the predicate matches.
    /// </summary>
    public BuyItemCallback Callback { get; init; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuyItemHook"/> class with the specified predicate and callback.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when an item is bought.
    /// </param>
    /// <param name="callback">
    ///     The callback that is invoked when an item is bought and the predicate matches.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    public BuyItemHook(BuyItemPredicate predicate, BuyItemCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);

        Predicate = predicate;
        Callback = callback;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuyItemHook"/> class with the specified callback.
    /// </summary>
    /// <param name="callback">
    ///     The callback that is invoked when an item is bought and the predicate matches.
    /// </param>
    /// <remarks>
    ///     Checks for any item bought, regardless of type.
    /// </remarks>
    public BuyItemHook(BuyItemCallback callback) : this(null, callback) { }

    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => GlobalTypeLookups<GlobalItem>.Initialized && (Predicate?.Invoke(entity) ?? true);

    public override void OnCreated(Item item, ItemCreationContext context)
    {
        if (context is not BuyItemCreationContext buy)
            return;

        Callback.Invoke(item, buy);
    }
}

public abstract class BuyItemHook<TQuest> : BuyItemHook
    where TQuest : QBQuest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BuyItemHook{TQuest}"/> class with the specified predicate.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when an item is bought.
    /// </param>
    public BuyItemHook(BuyItemPredicate predicate) : base(predicate, Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuyItemHook{TQuest}"/> class.
    /// </summary>
    /// <remarks>
    ///    Checks for any item bought, regardless of type.
    /// </remarks>
    public BuyItemHook() : base(Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuyItemHook{TQuest}"/> class with the specified item type.
    /// </summary>
    /// <param name="type">
    ///     The type of the item that should trigger this hook when bought.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is less than or equal to zero.
    /// </exception>
    public BuyItemHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        Predicate = item => Match(item, type);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuyItemHook{TQuest}"/> class with the specified item bool set.
    /// </summary>
    /// <param name="set">
    ///     A factory bool set of items that should trigger this hook when bought.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="set"/> is null.
    /// </exception>
    public BuyItemHook(bool[] set) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(set);
        Predicate = item => Match(item, set);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuyItemHook{TQuest}"/> class with the specified item types.
    /// </summary>
    /// <param name="matches">
    ///     The types of items that should trigger this hook when bought.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="matches"/> is null.
    /// </exception>
    public BuyItemHook(params int[] matches) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(matches);
        Predicate = item => Match(item, matches);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuyItemHook{TQuest}"/> class with the specified item type.
    /// </summary>
    /// <param name="getItemType">
    ///     A function to retrieve the type of the item that should trigger this hook when bought.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="set"/> is null.
    /// </exception>
    public BuyItemHook(Func<int> getItemType) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(getItemType);
        Predicate = item => Match(item, getItemType);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BuyItemHook{TQuest}"/> class with the specified item type.
    /// </summary>
    /// <param name="getItemTypes">
    ///     A set of functions to retrieve the types of the items that should trigger this hook when bought.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="set"/> is null.
    /// </exception>
    public BuyItemHook(params Func<int>[] getItemTypes) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(getItemTypes);
        Predicate = item => Match(item, getItemTypes);
    }

    protected static void Complete(Item item, BuyItemCreationContext context) => QuestManager.MarkComplete<TQuest>();
}

public abstract class BuyItemHook<TQuest, TModItem> : BuyItemHook<TQuest>
    where TQuest : QBQuest
    where TModItem : ModItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BuyItemHook{TQuest, TModItem}"/> class.
    /// </summary>
    public BuyItemHook() : base(Match<TModItem>) { }
}