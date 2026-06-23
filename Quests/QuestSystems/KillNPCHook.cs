using QuestBooks.Systems;

namespace QuestBooks.Quests.QuestSystems
{
    /// <summary>
    /// Provides a simple hook into when an NPC is killed.<br/>
    /// <br/>
    /// See also:<br/>
    /// <see cref="KillNPCHook{TNPCType}"/><br/>
    /// <see cref="KillNPCCheck{TQuest}"/><br/>
    /// <see cref="KillNPCCheck{TQuest, TNPCType}"/>
    /// </summary>
    /// <param name="match">Checks whether this hook should fire.</param>
    /// <param name="onComplete">The action you want to perform when matched to a crafted item.</param>
    public abstract class KillNPCHook(Func<NPC, bool> match, Action<NPC> onComplete) : GlobalNPC
    {
        public KillNPCHook(int npcType, Action<NPC> onComplete) : this(npc => npc.type == npcType, onComplete) { }

        public Func<NPC, bool> Match { get; init; } = match;

        public Action<NPC> OnComplete { get; init; } = onComplete;

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => Match(entity);

        public override void OnKill(NPC npc) => OnComplete(npc);
    }

    public abstract class KillNPCHook<TNPCType>(Action<NPC> onComplete) : KillNPCHook(npc => npc.type == ModContent.NPCType<TNPCType>(), onComplete)
        where TNPCType : ModNPC;

    public abstract class KillNPCCheck<TQuest>(Func<NPC, bool> match) : KillNPCHook(match, _ => QuestManager.CompleteQuest<TQuest>())
        where TQuest : Quest
    {
        public KillNPCCheck(int npcType) : this(npc => npc.type == npcType) { }
    }

    public abstract class KillNPCCheck<TQuest, TNPCType>() : KillNPCCheck<TQuest>(ModContent.NPCType<TNPCType>())
        where TQuest : Quest
        where TNPCType : ModNPC;
}
