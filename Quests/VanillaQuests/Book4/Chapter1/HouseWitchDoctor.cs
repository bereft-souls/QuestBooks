namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseWitchDoctor : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.WitchDoctor);
}