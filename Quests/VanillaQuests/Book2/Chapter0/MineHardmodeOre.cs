using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

[ReinitializeDuringResizeArrays]
public class MineHardmodeOre : VanillaQuest
{
    static MineHardmodeOre()
    {
        HardmodeOres = TileID.Sets.Factory.CreateNamedSet("HardmodeOres")
            .Description("Hardmode ore tiles")
            .RegisterBoolSet(
                TileID.Cobalt,
                TileID.Palladium,
                TileID.Mythril,
                TileID.Orichalcum,
                TileID.Adamantite,
                TileID.Titanium
            );
    }

    public static readonly bool[] HardmodeOres;

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class MineHardmodeOreCheck() : KillTileHook<MineHardmodeOre>(HardmodeOres);
}