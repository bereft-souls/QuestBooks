namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class MoonLordDefeated : VanillaQuest
{
    public override bool CheckCompletion() => NPC.downedMoonlord;
}
