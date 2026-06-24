using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatFlyingDutchman : QBQuest
{
    public override bool CheckCompletion() => false;

    public sealed class KillFlyingDutchmanCheck() : KillNPCHook<DefeatFlyingDutchman>(NPCID.PirateShip);
}