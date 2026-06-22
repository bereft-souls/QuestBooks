using QuestBooks.Systems.Common.NPCs;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class DefeatFlyingDutchman : QBQuest
{
    public override bool CheckCompletion() => NPCDownedFlagsSystem.Downed(NPCID.PirateShip);
}