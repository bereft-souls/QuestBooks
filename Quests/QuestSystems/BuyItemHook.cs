using QuestBooks.Quests.VanillaQuests;
using QuestBooks.Systems;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool BuyItemPredicate(Item item, BuyItemCreationContext context);

public delegate void BuyItemCallback(Item item, BuyItemCreationContext context);

public abstract class BuyItemHook : GlobalItem
{
    private const string Tag = "ItemPurchaseAmount";
    
    /// <summary>
    ///     Gets the predicate that determines whether this hook should fire when an item is bought.
    /// </summary>
    public BuyItemPredicate Predicate { get; init; }

    /// <summary>
    ///     Gets the callback that is invoked when an item is bought and the predicate matches.
    /// </summary>
    public BuyItemCallback Callback { get; init; }
    
    /// <summary>
    ///     Gets the amount of times the item has been bought that matches the predicate.
    /// </summary>
    public static int Amount { get; private set; }
    
    public BuyItemHook(BuyItemPredicate predicate, BuyItemCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }
    
    public BuyItemHook(BuyItemCallback callback) : this(null, callback) { }
    
    public override void OnCreated(Item item, ItemCreationContext context)
    {
        if (context is not BuyItemCreationContext buy)
        {
            return;
        }
        
        var matches = Predicate?.Invoke(item, buy) ?? true;

        if (!matches)
        {
            return;
        }

        Amount++;
        
        Callback.Invoke(item, buy);
    }

    public override void SaveData(Item item, TagCompound tag) => tag[Tag] = Amount;
    
    public override void LoadData(Item item, TagCompound tag) => Amount = tag.GetInt(Tag);
}

public abstract class BuyItemHook<TQuest> : BuyItemHook where TQuest : QBQuest
{
    public BuyItemHook(BuyItemPredicate predicate) : base(predicate, Complete) { }

    public BuyItemHook() : base(Complete) { }

    public BuyItemHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        Predicate = (item, _) => Match(item, type);
    }

    public BuyItemHook(int type, int amount) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        
        Predicate = (item, _) => item.type == type && Amount >= amount;
    }
    
    protected static bool Match(Item item, int type) => item.type == type;

    protected static bool Match(Item item, int type, int amount) => item.type == type && Amount >= amount;

    protected static void Complete(Item item, BuyItemCreationContext context) => QuestManager.MarkComplete<TQuest>();
}

public abstract class BuyItemHook<TQuest, TModItem> : BuyItemHook<TQuest> where TQuest : QBQuest where TModItem : ModItem
{
    public BuyItemHook() => Predicate = (item, _) => Match(item);

    public BuyItemHook(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        
        Predicate = (item, _) => Match(item, amount);
    }
    
    protected static bool Match(Item item) => item.type == ModContent.ItemType<TModItem>();
    
    protected static new bool Match(Item item, int amount) => item.type == ModContent.ItemType<TModItem>() && Amount >= amount;
}