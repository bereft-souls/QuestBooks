using QuestBooks.Systems;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool BuyItemAmountPredicate(Item item);

public delegate void BuyItemAmountCallback(Item item, BuyItemCreationContext context);

public abstract class BuyItemAmountHook : GlobalItem
{
    private const string Tag = "ItemPurchaseAmount";
    
    public BuyItemAmountPredicate Predicate { get; init; }
    
    public BuyItemAmountCallback Callback { get; init; }
    
    public static int Amount { get; private set; }

    public BuyItemAmountHook(BuyItemAmountPredicate predicate, BuyItemAmountCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }
    
    public BuyItemAmountHook(BuyItemAmountCallback callback) : this(null, callback) { }

    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => Predicate?.Invoke(entity) ?? true;

    public override void OnCreated(Item item, ItemCreationContext context)
    {
        if (context is not BuyItemCreationContext buyItemContext)
        {
            return;
        }
        
        Amount++;
        
        Callback.Invoke(item, buyItemContext);
    }
    
    public override void SaveData(Item item, TagCompound tag) => tag[Tag] = Amount;
    
    public override void LoadData(Item item, TagCompound tag) => Amount = tag.GetInt(Tag);
}

public abstract class BuyItemAmountHook<TQuest> : BuyItemAmountHook 
    where TQuest : Quest
{
    public BuyItemAmountHook(int type, int amount) : base(Complete)
    {
       ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
       ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
       
       Predicate = item => item.type == type && Amount >= amount;
    }

    protected static void Complete(Item item, BuyItemCreationContext context) => QuestManager.MarkComplete<TQuest>();
}

public abstract class BuyItemAmountHook<TQuest, TModItem> : BuyItemAmountHook 
    where TQuest : Quest
    where TModItem : ModItem
{
    public BuyItemAmountHook(int amount) : base(Complete)
    {
       ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
       
       Predicate = item => item.type == ModContent.ItemType<TModItem>() && Amount >= amount;
    }

    protected static void Complete(Item item, BuyItemCreationContext context) => QuestManager.MarkComplete<TQuest>();
}