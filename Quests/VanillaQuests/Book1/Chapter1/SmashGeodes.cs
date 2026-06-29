using QuestBooks.Systems;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class SmashGeodes : VanillaQuest
{
    private const string Tag = "SmashedGeodesCount";

    /// <summary>
    ///     The number of geodes the player must smash in order to complete the quest.
    /// </summary>
    public const int GeodesTarget = 10;

    /// <summary>
    ///     Gets the number of geodes the player has smashed.
    /// </summary>
    public int GeodesSmashed { get; private set; }

    public override bool CheckCompletion() => GeodesSmashed >= GeodesTarget;

    public override void SaveProgress(TagCompound tag) => tag[Tag] = GeodesSmashed;

    public override void LoadProgress(TagCompound tag) => GeodesSmashed = tag.GetInt(Tag);

    public class SmashGeodesCheck : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Geode;

        public override void RightClick(Item item, Player player)
        {
            var quest = QuestManager.GetQuest<SmashGeodes>();

            if (quest.Completed)
            {
                return;
            }

            quest.GeodesSmashed++;
        }
    }
}