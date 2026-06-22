using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractGuide : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => Main.LocalPlayer.GetModPlayer<InteractGuideCheck>().Completed;

    public class InteractGuideCheck : ModPlayer
    {
        private const string Tag = "Cache";

        /// <summary>
        ///     The amount of unique items the player must interact with in the guide's inventory in order to
        ///     complete the quest.
        /// </summary>
        public const int Target = 50;

        /// <summary>
        ///     Gets a list of item types that the player has interacted with in the guide's inventory.
        /// </summary>
        public List<int> Cache { get; private set; } = [];

        /// <summary>
        ///     Gets the amount of unique items the player has interacted with in the guide's inventory.
        /// </summary>
        public int Amount => Cache.Count;

        /// <summary>
        ///     Gets a value indicating whether the player has interacted with the guide enough times to
        ///     complete the quest.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if <see cref="Amount"/> is greater than or equal to <see cref="Target"/>
        ///     ; otherwise, <see langword="false"/>.
        /// </value>
        public bool Completed => Amount >= Target;

        public override void Load() => On_ItemSlot.LeftClick_ItemArray_int_int += Check;

        public override void SaveData(TagCompound tag) => tag[Tag] = Cache;

        public override void LoadData(TagCompound tag) => Cache = new List<int>(tag.GetList<int>(Tag));

        // TODO: We may want to stop caching new entries after the quest is completed.
        private static void Check(On_ItemSlot.orig_LeftClick_ItemArray_int_int orig, Item[] inv, int context, int slot)
        {
            orig(inv, context, slot);

            if (context != ItemSlot.Context.GuideItem)
                return;

            var item = Main.guideItem;

            if (item.IsAir)
                return;

            var player = Main.LocalPlayer.GetModPlayer<InteractGuideCheck>();

            if (player.Cache.Contains(item.type))
                return;

            player.Cache.Add(item.type);
        }
    }
}