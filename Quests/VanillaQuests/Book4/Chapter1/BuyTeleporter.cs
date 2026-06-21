using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class BuyTeleporter : QBQuest
{
    public override bool CheckCompletion() => false;

    public class BuyTeleporterCheck : GlobalItem
    {
        /// <summary>
        ///     The amount of teleporters the player needs to buy to complete the quest.
        /// </summary>
        public const int Target = 2;
        
        /// <summary>
        ///     Gets the amount of teleporters the player has bought.
        /// </summary>
        public int Amount { get; private set; }
        
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Teleporter;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not BuyItemCreationContext)
                return;

            Amount++;
            
            if (Amount < Target)
                return;
            
            QuestManager.MarkComplete<BuyTeleporter>();
        }

        public override void SaveData(Item item, TagCompound tag) => tag[nameof(Amount)] = Amount;
        
        public override void LoadData(Item item, TagCompound tag) => Amount = tag.GetInt(nameof(Amount));
    }
}