using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class EmpressOfLightDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedEmpressOfLight;
}
