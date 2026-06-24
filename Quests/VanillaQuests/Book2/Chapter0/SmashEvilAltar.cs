using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class SmashEvilAltar : QBQuest
{
    public override bool CheckCompletion() => false;

    public class SmashEvilAltarCheck() : KillTileHook<SmashEvilAltar>(TileID.DemonAltar);
}