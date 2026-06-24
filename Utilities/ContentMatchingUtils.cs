using System.Linq;
using Terraria.DataStructures;

namespace QuestBooks.Utilities;

public static partial class Utils
{
    #region Item

    public static bool Match(Item item, int type) => item.type == type;

    public static bool Match(Item item, bool[] set) => set[item.type];

    public static bool Match(Item item, params int[] types) => types.Contains(item.type);

    public static bool Match<T>(Item item) where T : ModItem => item.type == ModContent.ItemType<T>();

    public static bool Match(Item item, Func<int> getItemType) => item.type == getItemType();

    public static bool Match(Item item, params Func<int>[] getItemTypes) => getItemTypes.Any(f => f() == item.type);

    #endregion

    #region NPC

    public static bool Match(NPC npc, int match) => npc.type == match;

    public static bool Match(NPC npc, bool[] set) => set[npc.type];

    public static bool Match(NPC npc, params int[] matches) => matches.Contains(npc.type);

    public static bool Match<T>(NPC npc) where T : ModNPC => npc.type == ModContent.NPCType<T>();

    public static bool Match(NPC npc, Func<int> getNpcType) => npc.type == getNpcType();

    public static bool Match(NPC npc, params Func<int>[] getNpcTypes) => getNpcTypes.Any(f => f() == npc.type);

    #endregion

    #region Tiles/Fishing/Misc

    public static bool Match(int type, int match) => type == match;

    public static bool Match(int type, bool[] set) => set[type];

    public static bool Match(int type, params int[] matches) => matches.Contains(type);

    public static bool Match<T>(int type) where T : ModItem => type == ModContent.ItemType<T>();

    /// <summary>
    /// Allows shorthand delegate matching access to <see cref="Match{T}(int)"/>
    /// </summary>
    public static bool Match<T>(FishingAttempt attempt, int drop, int npc, AdvancedPopupRequest sonar, Vector2 position) where T : ModItem => Match<T>(drop);

    public static bool Match(int type, Func<int> getContentType) => type == getContentType();

    public static bool Match(int type, params Func<int>[] getContentTypes) => getContentTypes.Any(f => f() == type);

    public static bool MatchTile<T>(int type) where T : ModTile => type == ModContent.TileType<T>();

    /// <summary>
    /// Allows shorthand delegate matching access to <see cref="MatchTile{T}(int)"/>
    /// </summary>
    public static bool Match<T>(int x, int y, int tileType) where T : ModTile => MatchTile<T>(tileType);

    /// <summary>
    /// Allows shorthand delegate matching access to <see cref="MatchTile{T}(int)"/>
    /// </summary>
    public static bool Match<T>(int i, int j, int type, Item item) where T : ModTile => MatchTile<T>(type);

    #endregion
}
