using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SmashGeodes : QBQuest
{
    public override bool CheckCompletion() => GeodeItemCheck.Completed;

    public class GeodeItemCheck : GlobalItem
    {
        public const int Threshold = 10;

        public static int Count { get; private set; }

        public static bool Completed => Count >= Threshold;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Geode;

        public override void RightClick(Item item, Player player) => Count++;

        public override void SaveData(Item item, TagCompound tag) => tag[nameof(Count)] = Count;

        public override void LoadData(Item item, TagCompound tag) => Count = tag.GetInt(nameof(Count));
    }
}