using QuestBooks.Systems;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool KillTilePredicate(int x, int y, int type);

public delegate void KillTileCallback(int x, int y, int type);

public abstract class KillTileHook : GlobalTile
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should be invoked when a tile is killed.
    /// </summary>
    /// <remarks>
    ///     If <see langword="null"/>, evaluates as <see langword="true"/>.
    /// </remarks>
    public KillTilePredicate Predicate { get; init; }

    /// <summary>
    ///     Gets the callback that is invoked when a tile is killed and the predicate matches.
    /// </summary>
    public KillTileCallback Callback { get; init; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillTileHook"/> class with the specified predicate and callback.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when a tile is killed.
    /// </param>
    /// <param name="callback">
    ///     The callback that is invoked when a tile is killed and the predicate matches.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    public KillTileHook(KillTilePredicate predicate, KillTileCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);

        Predicate = predicate;
        Callback = callback;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillTileHook"/> class with the specified callback.
    /// </summary>
    /// <param name="callback">
    ///     The callback that is invoked when a tile is killed and the predicate matches.
    /// </param>
    /// <remarks>
    ///     Checks for any tile killed, regardless of type.
    /// </remarks>
    public KillTileHook(KillTileCallback callback) : this(null, callback) { }

    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (fail || effectOnly)
            return;

        var matches = Predicate?.Invoke(i, j, type) ?? true;

        if (!matches)
            return;

        Callback.Invoke(i, j, type);
    }
}

public abstract class KillTileHook<TQuest> : KillTileHook
    where TQuest : Quest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="KillTileHook{TQuest}"/> class with the specified predicate.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when a tile is killed.
    /// </param>
    public KillTileHook(KillTilePredicate predicate) : base(predicate, Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillTileHook{TQuest}"/> class.
    /// </summary>
    /// <remarks>
    ///     Checks for any tile killed, regardless of type.
    /// </remarks>
    public KillTileHook() : base(Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillTileHook{TQuest}"/> class.
    /// </summary>
    /// <param name="type">
    ///     The type of the tile that should trigger this hook when killed.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public KillTileHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        Predicate = (_, _, match) => Match(type, match);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillTileHook{TQuest}"/> class.
    /// </summary>
    /// <param name="set">
    ///     The set of tile types that should trigger this hook when killed.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="set"/> is <see langword="null"/>.
    /// </exception>
    public KillTileHook(bool[] set) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(set);
        Predicate = (_, _, type) => Match(type, set);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillTileHook{TQuest}"/> class.
    /// </summary>
    /// <param name="types">
    ///     The array of tile types that should trigger this hook when killed.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="types"/> is <see langword="null"/>.
    /// </exception>
    public KillTileHook(params int[] types) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(types);
        Predicate = (_, _, type) => Match(type, types);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillTileHook{TQuest}"/> class.
    /// </summary>
    /// <param name="getTileType">
    ///     The function that returns the tile type that should trigger this hook when killed.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="getTileType"/> is <see langword="null"/>.
    /// </exception>
    public KillTileHook(Func<int> getTileType) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(getTileType);
        Predicate = (_, _, type) => Match(type, getTileType);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillTileHook{TQuest}"/> class.
    /// </summary>
    /// <param name="getTileTypes">
    ///     The functions that return the tile types that should trigger this hook when killed.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="getTileTypes"/> is <see langword="null"/>.
    /// </exception>
    public KillTileHook(params Func<int>[] getTileTypes) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(getTileTypes);
        Predicate = (_, _, type) => Match(type, getTileTypes);
    }

    protected static void Complete(int x, int y, int type) => QuestManager.MarkComplete<TQuest>();
}

public abstract class KillTileHook<TQuest, TModTile> : KillTileHook<TQuest>
    where TQuest : Quest
    where TModTile : ModTile
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="KillTileHook{TQuest, TModTile}"/> class.
    /// </summary>
    public KillTileHook() : base(Match<TModTile>) { }
}
