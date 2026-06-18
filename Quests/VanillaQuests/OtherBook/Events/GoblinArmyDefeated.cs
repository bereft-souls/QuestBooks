using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class GoblinArmyDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedGoblins;
}
