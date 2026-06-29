namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class EquipEyeBone : VanillaQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.petFlagChesterPet;
}