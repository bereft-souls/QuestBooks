using QuestBooks.Systems;
using Terraria.GameContent.Events;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractPartyGirl : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class InteractPartyGirlCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.PartyGirl;

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (!BirthdayParty.PartyIsUp || !BirthdayParty.GenuineParty)
                return;

            QuestBooksMod.MarkComplete<InteractPartyGirl>();
        }
    }
}