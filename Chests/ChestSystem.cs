using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace QuestBooks.Chests;

public sealed class ChestSystem : ModSystem
{
    private sealed class ChestSystemGlobalTile : GlobalTile
    {
        public override void RightClick(int i, int j, int type)
        {
            var index = Chest.FindChest(i, j);

            if (index == -1)
                return;

            var chest = Main.chest[index];

            flags[(chest.x, chest.y)] = true;
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail)
                return;

            var index = Chest.FindChest(i, j);

            if (index == -1)
                return;

            var chest = Main.chest[index];

            flags.Remove((chest.x, chest.y));
        }
    }

    private static readonly Dictionary<(int X, int Y), bool> flags = [];

    public override void PostWorldGen() => Populate();

    public override void ClearWorld() => flags.Clear();

    public override void SaveWorldData(TagCompound tag) => Save(tag);

    public override void LoadWorldData(TagCompound tag) => Load(tag);

    /// <summary>
    ///     Determines whether the chest at the given coordinates has been naturally generated.
    /// </summary>
    /// <param name="x">
    ///     The horizontal coordinate of the chest, in tiles.
    /// </param>
    /// <param name="y">
    ///     The vertical coordinate of the chest, in tiles.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the chest at the given coordinates has been naturally generated;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="x"/> or <paramref name="y"/> is negative.
    /// </exception>
    public static bool IsNatural(int x, int y)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);

        return flags.ContainsKey((x, y));
    }

    /// <summary>
    ///     Determines whether the chest at the given coordinates has been explored by a player.
    /// </summary>
    /// <param name="x">
    ///     The horizontal coordinate of the chest, in tiles.
    /// </param>
    /// <param name="y">
    ///     The vertical coordinate of the chest, in tiles.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the chest at the given coordinates has been explored by a player;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="x"/> or <paramref name="y"/> is negative.
    /// </exception>
    public static bool IsExplored(int x, int y)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);

        return flags.TryGetValue((x, y), out var data) && data;
    }

    private static void Populate()
    {
        for (var i = 0; i < Main.maxChests; i++)
        {
            var chest = Main.chest[i];

            if (chest == null)
                continue;

            flags.Add((chest.x, chest.y), false);
        }
    }

    private static void Save(TagCompound tag)
    {   
        // TODO: Save flags.
    }

    private static void Load(TagCompound tag)
    {
        // TODO: Load flags.
    }
}