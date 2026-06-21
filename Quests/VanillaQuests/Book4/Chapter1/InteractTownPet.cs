using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractTownPet : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class InteractTownNPCCheck : ModSystem
    {
        public override void Load() => On_Player.PetAnimal += Check;

        private static void Check(On_Player.orig_PetAnimal orig, Player self, int index)
        {
            orig(self, index);

            QuestManager.MarkComplete<InteractTownPet>();
        }
    }
}