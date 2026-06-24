using QuestBooks.Systems;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool PlaceTilePredicate(int i, int j, int type, Item item);

public delegate void PlaceTileCallback(int i, int j, int type, Item item);

public abstract class PlaceTileHook : GlobalTile
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should be invoked when a tile is placed in the world.
    /// </summary>
    /// <remarks>
    ///     If <see langword="null"/>, evaluates as <see langword="true"/>.
    /// </remarks>
    public PlaceTilePredicate Predicate { get; init; }

    /// <summary>
    ///     Gets the callback that is invoked when a tile is placed in the world and the predicate matches.
    /// </summary>
    public PlaceTileCallback Callback { get; init; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PlaceTileHook"/> class with the specified predicate and callback.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when a tile is placed in the world.
    /// </param>
    /// <param name="callback">
    ///     The callback that is invoked when a tile is placed in the world and the predicate matches.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    public PlaceTileHook(PlaceTilePredicate predicate, PlaceTileCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);

        Predicate = predicate;
        Callback = callback;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PlaceTileHook"/> class with the specified callback.
    /// </summary>
    /// <param name="callback">
    ///     The callback that is invoked when a tile is placed in the world and the predicate matches.
    /// </param>
    /// <remarks>
    ///     Checks for any tile placed in the world, regardless of type.
    /// </remarks>
    public PlaceTileHook(PlaceTileCallback callback) : this(null, callback) { }

    public sealed override void PlaceInWorld(int i, int j, int type, Item item)
    {
        var matches = Predicate?.Invoke(i, j, type, item) ?? true;

        if (!matches)
        {
            return;
        }

        Callback.Invoke(i, j, type, item);
    }
}

public abstract class PlaceTileHook<TQuest> : PlaceTileHook
    where TQuest : Quest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PlaceTileHook{TQuest}"/> class with the specified predicate.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when a tile is placed in the world.
    /// </param>
    public PlaceTileHook(PlaceTilePredicate predicate) : base(predicate, Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PlaceTileHook{TQuest}"/> class.
    /// </summary>
    /// <remarks>
    ///     Checks for any tile placed in the world, regardless of type.
    /// </remarks>
    public PlaceTileHook() : base(Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PlaceTileHook{TQuest}"/> class.
    /// </summary>
    /// <param name="type">
    ///     The type of the tile that should trigger this hook when placed in the world.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is less than or equal to zero.
    /// </exception>
    public PlaceTileHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        Predicate = (_, _, tile, _) => Match(tile, type);
    }

    public PlaceTileHook(bool[] set) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(set);
        Predicate = (_, _, tile, _) => Match(tile, set);
    }

    public PlaceTileHook(params int[] types) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(types);
        Predicate = (_, _, tile, _) => Match(tile, types);
    }

    public PlaceTileHook(Func<int> getType) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(getType);
        Predicate = (_, _, tile, _) => Match(tile, getType);
    }

    public PlaceTileHook(params Func<int>[] getTypes) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(getTypes);
        Predicate = (_, _, tile, _) => Match(tile, getTypes);
    }

    protected static void Complete(int i, int j, int type, Item item) => QuestManager.CompleteQuest<TQuest>();
}

public abstract class PlaceTileHook<TQuest, TModTile> : PlaceTileHook<TQuest>
    where TQuest : Quest
    where TModTile : ModTile
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PlaceTileHook{TQuest, TItem}"/> class.
    /// </summary>
    public PlaceTileHook() : base(Match<TModTile>) { }
}