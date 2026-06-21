using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class HouseArmsDealer : QBQuest
{
    public override bool CheckCompletion() => NPC.AnyNPCs(NPCID.ArmsDealer);
}