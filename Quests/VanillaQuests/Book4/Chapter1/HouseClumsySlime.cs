namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseClumsySlime : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimePurple);
}