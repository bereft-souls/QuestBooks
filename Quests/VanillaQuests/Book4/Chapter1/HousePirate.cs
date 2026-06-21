using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HousePirate : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.Pirate);
}