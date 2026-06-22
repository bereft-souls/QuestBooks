using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace QuestBooks.Systems.Common.Tiles;

public sealed class TileBreakPlayer : ModPlayer
{
    private sealed class TileBreakGlobalTile : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail || effectOnly || (Main.tileFrameImportant[type] && !TileObjectData.IsTopLeft(i, j)))
                return;

            Main.LocalPlayer.GetModPlayer<TileBreakPlayer>().Count++;
        }
    }

    private const string Tag = "Count";

    /// <summary>
    ///     Gets the number of tiles the player has broken.
    /// </summary>
    public int Count { get; private set; }

    public override void SaveData(TagCompound tag) => tag[Tag] = Count;

    public override void LoadData(TagCompound tag) => Count = tag.GetInt(Tag);
}