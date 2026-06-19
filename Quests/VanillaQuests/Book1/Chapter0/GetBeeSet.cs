using Microsoft.Xna.Framework;
using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class GetBeeSet : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class BeeSetItemCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.BeeGun;

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.HasArmorSet(ItemID.BeeHat, ItemID.BeeBreastplate, ItemID.BeeGreaves))
            {
                QuestManager.MarkComplete<GetBeeSet>();
            }

            return true;
        }
    }
}