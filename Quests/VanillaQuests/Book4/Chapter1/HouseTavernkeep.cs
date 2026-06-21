using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseTavernkeep : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.DD2Bartender);
}