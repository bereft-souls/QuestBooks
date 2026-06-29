namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseCyborg : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Cyborg);
}