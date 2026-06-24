using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class BuyTeleporter : QBQuest
{
    public int TeleportersBought { get; set; }

    public override bool CheckCompletion() => false;

    public override void SaveProgress(TagCompound tag) => tag[nameof(TeleportersBought)] = TeleportersBought >= 2 ? null : TeleportersBought;
    public override void LoadProgress(TagCompound tag) => TeleportersBought = tag.GetInt(nameof(TeleportersBought));

    public class BuyTeleporterCheck() : BuyItemHook(item => Match(item, ItemID.Teleporter), static (_, _) => QuestManager.GetQuest<BuyTeleporter>().TeleportersBought++);
}