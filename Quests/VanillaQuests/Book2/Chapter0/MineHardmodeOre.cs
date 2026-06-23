using QuestBooks.Systems;

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

    public class MineHMOreCheck : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (fail || noItem || !HardmodeOres[type])
                return;

            QuestManager.MarkComplete<MineHardmodeOre>();
        }
    }
}