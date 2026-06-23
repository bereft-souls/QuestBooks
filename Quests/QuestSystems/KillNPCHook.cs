using QuestBooks.Systems;
using System.Linq;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool KillNPCPredicate(NPC npc);

public delegate void KillNPCCallback(NPC npc);

public abstract class KillNPCHook : GlobalNPC
{
    public KillNPCPredicate Predicate { get; init; }
    
    public KillNPCCallback Callback { get; init; }
    
    public KillNPCHook(KillNPCPredicate predicate, KillNPCCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }
    
    public KillNPCHook(KillNPCCallback callback) : this(null, callback) { }
    
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => Predicate?.Invoke(entity) ?? true;

    public override void OnKill(NPC npc) => Callback.Invoke(npc);
}

public abstract class KillNPCHook<TQuest> : KillNPCHook where TQuest : Quest
{
    public KillNPCHook(KillNPCPredicate predicate) : base(predicate, Complete) { }
    
    public KillNPCHook() : base(Complete) { }
    
    public KillNPCHook(int type) : base(Complete)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        Predicate = npc => Match(npc, type);
    }

    public KillNPCHook(bool[] set) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(set);

        Predicate = npc => Match(npc, set);
    }

    public KillNPCHook(int[] matches) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(matches);

        Predicate = npc => Match(npc, matches);
    }
    
    protected static bool Match(NPC npc, int match) => npc.type == match;

    protected static bool Match(NPC npc, bool[] set) => set[npc.type];

    protected static bool Match(NPC npc, params int[] matches) => matches.Contains(npc.type);

    protected static bool Match<T>(NPC npc) where T : ModNPC => npc.type == ModContent.NPCType<T>();

    protected static void Complete(NPC npc) => QuestManager.CompleteQuest<TQuest>();
}
