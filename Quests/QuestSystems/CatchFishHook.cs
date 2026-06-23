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
    public CatchFishPredicate Predicate { get; init; }
    
    /// <summary>
    ///     Gets the callback that is invoked when a fish is caught and the predicate matches.
    /// </summary>
    public CatchFishCallback Callback { get; init; }
    
    public CatchFishHook(CatchFishPredicate predicate, CatchFishCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }
    
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

public abstract class CatchFishHook<TQuest> : CatchFishHook where TQuest : Quest
{
    public CatchFishHook(CatchFishPredicate predicate) : base(predicate, Complete) { }

    public CatchFishHook() : base(Complete) { }

    public CatchFishHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        Predicate = (_, drop, _, _, _) => Match(type, drop);
    }
    
    private static bool Match(int type, int drop) => drop == type;

    private static void Complete(FishingAttempt attempt, int drop, int npc, AdvancedPopupRequest sonar, Vector2 position) => QuestManager.MarkComplete<TQuest>();
}