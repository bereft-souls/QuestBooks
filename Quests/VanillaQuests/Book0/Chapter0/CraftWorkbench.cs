using QuestBooks.Systems;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class CraftWorkbench : QBQuest
{
    // Despite being a "craft" quest, this is more related to world state,
    // and thus remains a world quest instead of a player quest.

    public override bool CheckCompletion() => false;

    public class WorkbenchItemCheck : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is not RecipeItemCreationContext || item.createTile == -1)
                return;

            if (item.createTile != TileID.WorkBenches && !(ModContent.GetModTile(item.createTile)?.AdjTiles?.Contains(TileID.WorkBenches) ?? false))
                return;

            QuestManager.CompleteQuest<CraftWorkbench>();
        }
    }
}