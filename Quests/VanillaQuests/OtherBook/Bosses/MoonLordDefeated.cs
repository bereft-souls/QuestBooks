namespace QuestBooks.Quests.VanillaQuests.OtherBook.Bosses;

public class MoonLordDefeated : QBQuest
{
    public override bool CheckCompletion() => NPC.downedMoonlord;
}
