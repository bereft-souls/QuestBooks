using QuestBooks.Systems;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractNurse : QBQuest
{
    private const string Tag = "NurseCoinsSpent";

    /// <summary>
    ///     The amount of coins the player must spend at the nurse in order to complete the quest.
    /// </summary>
    /// <value>
    ///     1 gold coin.
    /// </value>
    public static readonly int CoinsTarget = Item.buyPrice(0, 1);

    /// <summary>
    ///     The amount of coins the player has spent at the nurse so far.
    /// </summary>
    public int CoinsSpent { get; private set; }

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => CoinsTarget >= CoinsSpent;

    public override void SaveProgress(TagCompound tag) => tag[Tag] = CoinsSpent;

    public override void LoadProgress(TagCompound tag) => CoinsSpent = tag.GetInt(Tag);

    public class InteractNurseCheck : ModPlayer
    {
        public override void PostNurseHeal(NPC nurse, int health, bool removeDebuffs, int price)
        {
            var quest = QuestManager.GetQuest<InteractNurse>();

            if (quest.Completed)
            {
                return;
            }

            quest.CoinsSpent += price;

            if (quest.CoinsSpent < InteractNurse.CoinsTarget)
            {
                return;
            }

            QuestManager.MarkComplete<InteractNurse>();
        }
    }
}