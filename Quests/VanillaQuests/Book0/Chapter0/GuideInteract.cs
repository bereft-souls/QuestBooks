using QuestBooks.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class GuideInteract : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class GuideInteractCheck : GlobalNPC
    {
        // TODO: Test this

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (npc.type != NPCID.Guide)
                return;

            QuestManager.CompleteQuest<GuideInteract>();
        }
    }
}
