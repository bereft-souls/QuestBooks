using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class GolemDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedGolemBoss;
}
