using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class GoblinArmy : QBQuest
{
    public override bool CheckCompletion() => NPC.downedGoblins;
}
