using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SmashGeodes : QBQuest
{
    public override bool CheckCompletion() => SmashGeodesCheck.Completed;

    public class SmashGeodesCheck : GlobalItem
    {
        private const string Tag = "Count";

        /// <summary>
        ///     The number of geodes the player must smash in order to complete the quest.
        /// </summary>
        public const int Target = 10;

        /// <summary>
        ///     Gets the number of geodes the player has smashed.
        /// </summary>
        public static int Count { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether the player has smashed enough geodes to complete the quest.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if <see cref="Count"/> is greater than or equal to <see cref="Target"/>;
        ///     otherwise, <see langword="false"/>.
        /// </value>
        public static bool Completed => Count >= Target;

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Geode;

        public override void RightClick(Item item, Player player) => Count++;

        public override void SaveData(Item item, TagCompound tag) => tag[Tag] = Count;

        public override void LoadData(Item item, TagCompound tag) => Count = tag.GetInt(Tag);
    }
}