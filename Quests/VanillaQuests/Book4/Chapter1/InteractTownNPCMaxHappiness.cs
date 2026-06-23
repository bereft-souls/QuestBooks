using QuestBooks.Systems;
using Terraria.GameContent;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractTownNPCMaxHappiness : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class InteractTownNPCMaxHappinessCheck : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.townNPC;

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            var settings = Main.ShopHelper.GetShoppingSettings(Main.LocalPlayer, npc);

            if (settings.PriceAdjustment >= ShopHelper.MaxHappinessAchievementPriceMultiplier)
                return;

            QuestManager.MarkComplete<InteractTownNPCMaxHappiness>();
        }
    }
}