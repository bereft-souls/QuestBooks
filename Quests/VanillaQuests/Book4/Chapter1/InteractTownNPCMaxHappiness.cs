using QuestBooks.Systems;
using QuestBooks.Utilities;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractTownNPCMaxHappiness : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;
    
    public class InteractTownNPCMaxHappinessCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.townNPC;

        // TODO: Implementation.
        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            QuestManager.MarkComplete<InteractTownNPCMaxHappiness>();
        }
    }
}