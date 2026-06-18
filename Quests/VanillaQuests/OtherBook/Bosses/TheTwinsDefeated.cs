using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class TheTwinsDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedMechBoss2;
}
