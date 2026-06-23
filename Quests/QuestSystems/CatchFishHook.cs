using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool CatchFishPredicate(FishingAttempt attempt, int drop, int npc, AdvancedPopupRequest sonar, Vector2 position);

public delegate void CatchFishCallback(FishingAttempt attempt, int drop, int npc, AdvancedPopupRequest sonar, Vector2 position);

public abstract class CatchFishHook : ModPlayer
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should be invoked when a fish is caught.
    /// </summary>
    /// <remarks>
    ///     If <see langword="null"/>, evaluates as <see langword="true"/>.
    /// </remarks>
    public CatchFishPredicate Predicate { get; init; }
    
    /// <summary>
    ///     Gets the callback that is invoked when a fish is caught and the predicate matches.
    /// </summary>
    public CatchFishCallback Callback { get; init; }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="CatchFishHook"/> class with the specified predicate and callback.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when a fish is caught.
    /// </param>
    /// <param name="callback">
    ///     The callback that is invoked when a fish is caught and the predicate matches.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    public CatchFishHook(CatchFishPredicate predicate, CatchFishCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="CatchFishHook"/> class with the specified callback.
    /// </summary>
    /// <param name="callback">
    ///     The callback that is invoked when a fish is caught and the predicate matches.
    /// </param>
    /// <remarks>
    ///     Checks for any fish caught, regardless of type.
    /// </remarks>
    public CatchFishHook(CatchFishCallback callback) : this(null, callback) { }
    
    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        if (itemDrop <= ItemID.None)
        {
            return;
        }
        
        var matches = Predicate?.Invoke(attempt, itemDrop, npcSpawn, sonar, sonarPosition) ?? true;
        
        if (!matches)
        {
            return;
        }
        
        Callback.Invoke(attempt, itemDrop, npcSpawn, sonar, sonarPosition);
    }
}

public abstract class CatchFishHook<TQuest> : CatchFishHook 
    where TQuest : Quest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CatchFishHook{TQuest}"/> class with the specified predicate.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when a fish is caught.
    /// </param>
    public CatchFishHook(CatchFishPredicate predicate) : base(predicate, Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CatchFishHook{TQuest}"/> class.
    /// </summary>
    /// <remarks>
    ///     Checks for any fish caught, regardless of type.
    /// </remarks>
    public CatchFishHook() : base(Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CatchFishHook{TQuest}"/> class with the specified fish type.
    /// </summary>
    /// <param name="type">
    ///     The type of the fish that should trigger this hook when caught.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public CatchFishHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        Predicate = (_, drop, _, _, _) => Match(type, drop);
    }
    
    private static bool Match(int type, int drop) => drop == type;

    private static void Complete(FishingAttempt attempt, int drop, int npc, AdvancedPopupRequest sonar, Vector2 position) => QuestManager.MarkComplete<TQuest>();
}

public abstract class CatchFishHook<TQuest, TModItem> : CatchFishHook<TQuest>
    where TQuest : Quest
    where TModItem : ModItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CatchFishHook{TQuest, TItem}"/> class.
    /// </summary>
    public CatchFishHook() : base(ModContent.ItemType<TModItem>()) { }
}