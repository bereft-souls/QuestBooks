using QuestBooks.Systems;
using Terraria.ModLoader.Core;

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

    public override bool InstancePerEntity => true;

    public override void OnChatButtonClicked(NPC npc, bool firstButton)
    {
        var matches = Predicate?.Invoke(npc) ?? true;

        if (!matches)
            return;

        Callback.Invoke(npc, firstButton);
    }
}

public abstract class ChatNPCHook<TQuest> : ChatNPCHook
    where TQuest : Quest
{
    public ChatNPCHook(ChatNPCHookPredicate predicate) : base(predicate, Complete) { }

    public ChatNPCHook() : base(Complete) { }

    public ChatNPCHook(int type) : base(Complete)
    {
        type = NPCID.FromNetId(type);
        Predicate = npc => Match(npc, type);
    }

    public ChatNPCHook(bool[] set) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(set);
        Predicate = npc => Match(npc, set);
    }

    public ChatNPCHook(params int[] types) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(types);
        Predicate = npc => Match(npc, types);
    }

    public ChatNPCHook(Func<int> getNpcType) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(getNpcType);
        Predicate = npc => Match(npc, getNpcType);
    }

    public ChatNPCHook(params Func<int>[] getNpcTypes) : base(Complete)
    {
        ArgumentNullException.ThrowIfNull(getNpcTypes);
        Predicate = npc => Match(npc, getNpcTypes);
    }

    protected static void Complete(NPC npc, bool firstButton) => QuestManager.CompleteQuest<TQuest>();
}

public abstract class ChatNPCHook<TQuest, TModNPC> : ChatNPCHook<TQuest>
    where TQuest : Quest
    where TModNPC : ModNPC
{
    public ChatNPCHook() : base(Match<TModNPC>) { }
}