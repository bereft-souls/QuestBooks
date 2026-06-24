using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillMarriage : QBQuest
{
    public bool GroomKilled { get; private set; }

    public bool BrideKilled { get; private set; }

    public override bool CheckCompletion() => GroomKilled && BrideKilled;

    public override void SaveProgress(TagCompound tag)
    {
        tag[nameof(GroomKilled)] = GroomKilled;
        tag[nameof(BrideKilled)] = BrideKilled;
    }

    public override void LoadProgress(TagCompound tag)
    {
        GroomKilled = tag.GetBool(nameof(GroomKilled));
        BrideKilled = tag.GetBool(nameof(BrideKilled));
    }

    public sealed class KillGroomHook() : KillNPCHook(static npc => npc.type == NPCID.TheGroom, static _ => QuestManager.GetQuest<KillMarriage>().GroomKilled = true);

    public sealed class KillBrideHook() : KillNPCHook(static npc => npc.type == NPCID.TheBride, static _ => QuestManager.GetQuest<KillMarriage>().BrideKilled = true);
}