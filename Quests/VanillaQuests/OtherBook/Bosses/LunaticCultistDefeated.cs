namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class LunaticCultistDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedAncientCultist;
}
