using QuestBooks.Systems;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool LootChestPredicate(int x, int y, int type);

public delegate void LootChestCallback(int x, int y, int type);

public abstract class LootChestHook : GlobalTile
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should be invoked when a tile is right-clicked.
    /// </summary>
    public LootChestPredicate Predicate { get; init; }
    
    /// <summary>
    ///     Gets the callback that is invoked when a tile is right-clicked and the predicate matches.
    /// </summary>
    public LootChestCallback Callback { get; init; }

    public LootChestHook(LootChestPredicate predicate, LootChestCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }

    public LootChestHook(LootChestCallback callback) : this(null, callback) { }
    
    public override void RightClick(int i, int j, int type)
    {
        var matches = Predicate?.Invoke(i, j, type) ?? true;

        if (!matches)
        {
            return;
        }
        
        Callback.Invoke(i, j, type);
    }
}

public abstract class LootChestHook<TQuest> : LootChestHook where TQuest : Quest
{
    public LootChestHook(LootChestPredicate predicate) : base(predicate, Complete) { }

    public LootChestHook(int type, params int[] frames) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        
        ArgumentNullException.ThrowIfNull(frames);

        Predicate = (x, y, _) => Match(x, y, type, frames);
    }

    protected static bool Match(int x, int y, int type, params int[] frames)
    {
        if (!ChestSystem.IsNatural(x, y) || ChestSystem.IsExplored(x, y))
        {
            return false;
        }

        var tile = Framing.GetTileSafely(x, y);

        var match = false;

        foreach (var frame in frames)
        {
            match |= tile.TileFrameX == frame * 36;
        }

        return match;
    }

    protected static void Complete(int x, int y, int type) => QuestManager.CompleteQuest<TQuest>();
}
