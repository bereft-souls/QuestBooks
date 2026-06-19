using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class LunaticCultistDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedAncientCultist;
}
