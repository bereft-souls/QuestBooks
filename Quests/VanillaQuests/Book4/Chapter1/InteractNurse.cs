using QuestBooks.Systems;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractNurse : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class InteractNurseCheck : ModPlayer
    {
        /// <summary>
        ///     The amount of gold coins the player must spend at the nurse in order to complete the quest.
        /// </summary>
        public static readonly int Coins = Item.buyPrice(0, 1);
        
        /// <summary>
        ///     Gets the total amount of gold coins the player has spent at the nurse.
        /// </summary>
        public static int Value { get; private set; }

        public override void PostNurseHeal(NPC nurse, int health, bool removeDebuffs, int price)
        {
            Value += price;
            
            if (Value < Coins)
                return;
            
            QuestManager.MarkComplete<InteractNurse>();
        }
        
        public override void SaveData(TagCompound tag) => tag[nameof(Value)] = Value;

        public override void LoadData(TagCompound tag) => Value = tag.GetInt(nameof(Value));
    }
}