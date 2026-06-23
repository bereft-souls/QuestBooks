using System.Collections.Generic;
using QuestBooks.Systems;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class InteractGuide : QBQuest
{
    /// <summary>
    ///     The amount of unique items the player must interact with in the guide's inventory in order to
    ///     complete the quest.
    /// </summary>
    public const int MaterialsTarget = 50;
    
    /// <summary>
    ///     Gets a list of item types that the player has interacted with in the guide's inventory.
    /// </summary>
    public List<int> MaterialsCache { get; private set; } = [];
    
    /// <summary>
    ///     Gets the amount of unique items the player has interacted with in the guide's inventory.
    /// </summary>
    public int MaterialsAmount => MaterialsCache.Count;
    
    private const string Tag = "GuideMaterialsChecked";
    
    public override QuestType QuestType => QuestType.Player;

    public override void Load() => On_ItemSlot.LeftClick_ItemArray_int_int += Check;
    
    public override bool CheckCompletion() => MaterialsAmount >= MaterialsTarget;

    public override void SaveProgress(TagCompound tag) => tag[Tag] = MaterialsCache;
    
    public override void LoadProgress(TagCompound tag) => MaterialsCache = new List<int>(tag.GetList<int>(Tag));
    
    private static void Check(On_ItemSlot.orig_LeftClick_ItemArray_int_int orig, Item[] inv, int context, int slot)
    {
        orig(inv, context, slot);
        
        var quest = QuestManager.GetQuest<InteractGuide>();

        if (quest.Completed)
            return;

        var guide = context == ItemSlot.Context.GuideItem;
        
        if (!guide)
            return;

        var item = Main.guideItem;
        var cache = quest.MaterialsCache;

        if (item.IsAir || cache.Contains(item.type))
            return;
        
        cache.Add(item.type);
    }
}