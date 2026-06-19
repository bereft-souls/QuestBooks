using Terraria;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class EquipEyeBone : QBQuest
{
    public override bool CheckCompletion() => Main.LocalPlayer.petFlagChesterPet;
}