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

    public class BeeSetPlayerCheck : ModPlayer
    {
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ItemID.BeeGun && Player.HasArmorSet(ItemID.BeeHat, ItemID.BeeBreastplate, ItemID.BeeGreaves))
            {
                QuestManager.MarkComplete<GetBeeSet>();
            }
            
            return true;
        }
    }
}