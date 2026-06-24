namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class UseBinoculars : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.HeldItem.type == ItemID.Binoculars;
}