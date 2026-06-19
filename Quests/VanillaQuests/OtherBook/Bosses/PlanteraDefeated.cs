using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class PlanteraDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedPlantBoss;
}
