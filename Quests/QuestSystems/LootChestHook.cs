using QuestBooks.Systems;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool LootChestPredicate(int x, int y, int type);

public delegate void LootChestCallback(int x, int y, int type);

public abstract class LootChestHook : GlobalTile
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should be invoked when a tile is right-clicked.
    /// </summary>
    /// <remarks>
    ///     If <see langword="null"/>, evaluates as <see langword="true"/>.
    /// </remarks>
    public LootChestPredicate Predicate { get; init; }

    /// <summary>
    ///     Gets the callback that is invoked when a tile is right-clicked and the predicate matches.
    /// </summary>
    public LootChestCallback Callback { get; init; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LootChestHook"/> class with the specified predicate and callback.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when a tile is right-clicked.
    /// </param>
    /// <param name="callback">
    ///     The callback that is invoked when a tile is right-clicked and the predicate matches.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    public LootChestHook(LootChestPredicate predicate, LootChestCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);

        Predicate = predicate;
        Callback = callback;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LootChestHook"/> class with the specified callback.
    /// </summary>
    /// <param name="callback">
    ///     The callback that is invoked when a tile is right-clicked and the predicate matches.
    /// </param>
    public LootChestHook(LootChestCallback callback) : this(null, callback) { }

    public override void RightClick(int i, int j, int type)
    {
        var matches = Predicate?.Invoke(i, j, type) ?? true;

        if (!matches)
            return;

        Callback.Invoke(i, j, type);
    }
}

public abstract class LootChestHook<TQuest> : LootChestHook
    where TQuest : Quest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LootChestHook{TQuest}"/> class with the specified predicate.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when a tile is right-clicked.
    /// </param>
    public LootChestHook(LootChestPredicate predicate) : base(predicate, Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LootChestHook{TQuest}"/> class with the specified type and frames.
    /// </summary>
    /// <param name="type">
    ///     The type of the chest tile to match.
    /// </param>
    /// <param name="frames">
    ///     The frames of the chest tile to match.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="frames"/> is <see langword="null"/>.
    /// </exception>
    public LootChestHook(int type, params int[] frames) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        ArgumentNullException.ThrowIfNull(frames);

        Predicate = (x, y, _) => Match(x, y, type, frames);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LootChestHook{TQuest}"/> class with the specified type and frames.
    /// </summary>
    /// <param name="getType">
    ///     The function to retrieve the type of the chest tile to match.
    /// </param>
    /// <param name="frames">
    ///     The frames of the chest tile to match.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="getType"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="frames"/> is <see langword="null"/>.
    /// </exception>
    public LootChestHook(Func<int> getType, params int[] frames) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(getType);
        ArgumentNullException.ThrowIfNull(frames);

        Predicate = (x, y, _) => Match(x, y, getType, frames);
    }

    protected static bool Match(int x, int y, Func<int> getType, params int[] frames) => Match(x, y, getType(), frames);

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
