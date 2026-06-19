using Terraria;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class PiratesDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedPirates;
}
