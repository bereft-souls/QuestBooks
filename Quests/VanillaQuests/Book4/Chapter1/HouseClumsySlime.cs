namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseClumsySlime : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.TownSlimePurple);
}