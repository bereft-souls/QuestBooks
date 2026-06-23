using QuestBooks.Systems;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool PlaceTilePredicate(int i, int j, int type, Item item);

public delegate void PlaceTileCallback(int i, int j, int type, Item item);

public abstract class PlaceTileHook : GlobalTile
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should be invoked when a tile is placed in the world.
    /// </summary>
    public PlaceTilePredicate Predicate { get; init; }
    
    /// <summary>
    ///     Gets the callback that is invoked when a tile is placed in the world and the predicate matches.
    /// </summary>
    public PlaceTileCallback Callback { get; init; }
    
    public PlaceTileHook(PlaceTilePredicate predicate, PlaceTileCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }
    
    public PlaceTileHook(PlaceTileCallback callback) : this(null, callback) { }

    public override void PlaceInWorld(int i, int j, int type, Item item)
    {
        var matches = Predicate?.Invoke(i, j, type, item) ?? true;
        
        if (!matches)
        {
            return;
        }
        
        Callback.Invoke(i, j, type, item);
    }
}

public abstract class PlaceTileHook<TQuest> : PlaceTileHook where TQuest : Quest
{
    public PlaceTileHook(PlaceTilePredicate predicate) : base(predicate, Complete) { }

    public PlaceTileHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        Predicate = (_, _, match, _) => Match(type, match);
    }
    
    protected static bool Match(int type, int match) => type == match;

    protected static void Complete(int i, int j, int type, Item item) => QuestManager.CompleteQuest<TQuest>();
}