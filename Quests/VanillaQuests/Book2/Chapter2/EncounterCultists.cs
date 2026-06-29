namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter2;

public class EncounterCultists : VanillaQuest
{
    public override bool CheckCompletion() => NPC.AnyoneNearCultists();
}