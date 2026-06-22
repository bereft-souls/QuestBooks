namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class EncounterCultists : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyoneNearCultists();
}