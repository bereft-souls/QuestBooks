using QuestBooks.Systems;
using System.Linq;

namespace QuestBooks.Quests.QuestSystems;

public delegate bool ChatNPCHookPredicate(NPC npc);

public delegate void ChatNPCHookCallback(NPC npc, bool firstButton);

public abstract class ChatNPCHook : GlobalNPC
{
    public ChatNPCHookPredicate Predicate { get; init; }
    
    public ChatNPCHookCallback Callback { get; init; }

    public ChatNPCHook(ChatNPCHookPredicate predicate, ChatNPCHookCallback callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        
        Predicate = predicate;
        Callback = callback;
    }
    
    public ChatNPCHook(ChatNPCHookCallback callback) : this(null, callback) { }
    
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => Predicate?.Invoke(entity) ?? true;
    
    public override void OnChatButtonClicked(NPC npc, bool firstButton) => Callback.Invoke(npc, firstButton);
}

public abstract class ChatNPCHook<TQuest> : ChatNPCHook
    where TQuest : Quest
{
    public ChatNPCHook(ChatNPCHookCallback callback) : base(callback) { }

    public ChatNPCHook() : base(Complete) { }

    public ChatNPCHook(int type) : base(Complete)
    {
        type = NPCID.FromNetId(type);
        
        Predicate = npc => npc.type == type;
    }

    public ChatNPCHook(bool[] set) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(set);
        
        Predicate = npc => set[npc.type];
    }

    public ChatNPCHook(int[] types) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(types);
        
        Predicate = npc => types.Contains(npc.type);
    }
    
    protected static void Complete(NPC npc, bool firstButton) => QuestManager.CompleteQuest<TQuest>();
}

public abstract class ChatNPCHook<TQuest, TModNPC> : ChatNPCHook<TQuest>
    where TQuest : Quest
    where TModNPC : ModNPC
{
    public ChatNPCHook() : base(ModContent.NPCType<TModNPC>()) { }
}