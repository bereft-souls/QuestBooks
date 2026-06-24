using QuestBooks.Systems;
using Terraria.ModLoader.Core;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool KillNPCPredicate(NPC npc);

public delegate void KillNPCCallback(NPC npc);

public abstract class KillNPCHook : GlobalNPC
{
    /// <summary>
    ///     Gets the predicate that determines whether this hook should be invoked when an NPC is killed.
    /// </summary>
    /// <remarks>
    ///     If <see langword="null"/>, evaluates as <see langword="true"/>.
    /// </remarks>
    public KillNPCPredicate Predicate { get; init; }

    /// <summary>
    ///     Gets the callback that is invoked when an NPC is killed and the predicate matches.
    /// </summary>
    public KillNPCCallback Callback { get; init; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillNPCHook"/> class with the specified predicate and callback.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when an NPC is killed.
    /// </param>
    /// <param name="callback">
    ///     The callback that is invoked when an NPC is killed and the predicate matches.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="callback"/> is <see langword="null"/>.
    /// </exception>
    public KillNPCHook(KillNPCPredicate predicate, KillNPCCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);

        Predicate = predicate;
        Callback = callback;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillNPCHook"/> class with the specified callback.
    /// </summary>
    /// <param name="callback">
    ///     The callback that is invoked when an NPC is killed and the predicate matches.
    /// </param>
    /// <remarks>
    ///     Checks for any NPC killed, regardless of type.
    /// </remarks>
    public KillNPCHook(KillNPCCallback callback) : this(null, callback) { }

    public sealed override bool InstancePerEntity => true;

    public sealed override bool AppliesToEntity(NPC entity, bool lateInstantiation) => true;

    public sealed override void OnKill(NPC npc)
    {
        var matches = Predicate?.Invoke(npc) ?? true;

        if (!matches)
            return;
        
        Callback.Invoke(npc);
    }
}

public abstract class KillNPCHook<TQuest> : KillNPCHook
    where TQuest : Quest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="KillNPCHook{TQuest}"/> class with the specified predicate.
    /// </summary>
    /// <param name="predicate">
    ///     The predicate that determines whether this hook should be invoked when an NPC is killed.
    /// </param>
    public KillNPCHook(KillNPCPredicate predicate) : base(predicate, Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillNPCHook{TQuest}"/> class.
    /// </summary>
    /// <remarks>
    ///     Checks for any NPC killed, regardless of type.
    /// </remarks>
    public KillNPCHook() : base(Complete) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillNPCHook{TQuest}"/> class with the specified NPC type.
    /// </summary>
    /// <param name="type">
    ///     The type of the NPC that should trigger this hook when killed.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public KillNPCHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        Predicate = npc => Match(npc, type);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillNPCHook{TQuest}"/> class with the specified set of NPC types.
    /// </summary>
    /// <param name="set">
    ///     The set of NPC types that should trigger this hook when killed.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="set"/> is <see langword="null"/>.
    /// </exception>
    public KillNPCHook(bool[] set) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(set);
        Predicate = npc => Match(npc, set);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="KillNPCHook{TQuest}"/> class with the specified array of NPC types.
    /// </summary>
    /// <param name="types">
    ///     The array of NPC types that should trigger this hook when killed.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="types"/> is <see langword="null"/>.
    /// </exception>
    public KillNPCHook(int[] types) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(types);
        Predicate = npc => Match(npc, types);
    }

    protected static void Complete(NPC npc) => QuestManager.CompleteQuest<TQuest>();
}

public abstract class KillNPCHook<TQuest, TModNPC> : KillNPCHook<TQuest>
    where TQuest : Quest
    where TModNPC : ModNPC
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="KillNPCHook{TQuest, TModNPC}"/> class.
    /// </summary>
    public KillNPCHook() : base(Match<TModNPC>) { }
}