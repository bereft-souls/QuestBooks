using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book2.Chapter0;

public class MineHardmodeOre : QBQuest
{
    public static readonly bool[] HardmodeOres = TileID.Sets.Factory.CreateNamedSet("HardmodeOres")
        .Description("Hardmode ore tiles")
        .RegisterBoolSet(
            TileID.Cobalt,
            TileID.Palladium,
            TileID.Mythril,
            TileID.Orichalcum,
            TileID.Adamantite,
            TileID.Titanium
        );

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class MineHardmodeOreCheck() : KillTileHook<MineHardmodeOre>(HardmodeOres);
}