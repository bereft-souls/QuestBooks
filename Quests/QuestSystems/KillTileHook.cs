using System.Linq;
using QuestBooks.Systems;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool KillTilePredicate(int x, int y, int type);

public delegate void KillTileCallback(int x, int y, int type);

public abstract class KillTileHook : GlobalTile
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should be invoked when a tile is killed.
    /// </summary>
    public KillTilePredicate Predicate { get; init; }
    
    /// <summary>
    ///     Gets the callback that is invoked when a tile is killed and the predicate matches.
    /// </summary>
    public KillTileCallback Callback { get; init; }
    
    public KillTileHook(KillTilePredicate predicate, KillTileCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }
    
    public KillTileHook(KillTileCallback callback) : this(null, callback) { }
    
    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail)
        {
            return;
        }
        
        var matches = Predicate?.Invoke(i, j, type) ?? true;
        
        if (!matches)
        {
            return;
        }
        
        Callback.Invoke(i, j, type);
    }
}

public abstract class KillTileHook<TQuest> : KillTileHook where TQuest : Quest
{
    public KillTileHook(KillTilePredicate predicate) : base(predicate, Complete) { }

    public KillTileHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        
        Predicate = (_, _, match) => Match(type, match);
    }

    public KillTileHook(bool[] set) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(set);
        
        Predicate = (_, _, type) => Match(type, set);
    }
    
    public KillTileHook(params int[] types) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(types);
        
        Predicate = (_, _, type) => Match(type, types);
    }

    protected static bool Match(int type, int match) => type == match;

    protected static bool Match(int type, bool[] set) => set[type];

    protected static bool Match(int type, params int[] types) => types.Contains(type);

    protected static void Complete(int x, int y, int type) => QuestManager.MarkComplete<TQuest>();
}