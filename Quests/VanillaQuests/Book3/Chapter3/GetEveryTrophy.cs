using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class GetEveryTrophy : QBQuest
{
    // Despite the fact that mods may add extra trophies, modded trophies should NOT count
    // towards this quest, and as such do not need to be registered with a set
    public static readonly int[] AllTrophies = [
        ItemID.KingSlimeTrophy,
        ItemID.EyeofCthulhuTrophy,
        ItemID.EaterofWorldsTrophy,
        ItemID.BrainofCthulhuTrophy,
        ItemID.QueenBeeTrophy,
        ItemID.SkeletronTrophy,
        ItemID.DeerclopsTrophy,
        ItemID.WallofFleshTrophy,
        ItemID.QueenSlimeTrophy,
        ItemID.RetinazerTrophy,
        ItemID.SpazmatismTrophy,
        ItemID.DestroyerTrophy,
        ItemID.SkeletronPrimeTrophy,
        ItemID.PlanteraTrophy,
        ItemID.GolemTrophy,
        ItemID.DukeFishronTrophy,
        ItemID.AncientCultistTrophy,
        ItemID.BetsyMasterTrophy,
        ItemID.MoonLordTrophy,
        ItemID.OgreMasterTrophy,
        ItemID.FlyingDutchmanTrophy,
        ItemID.MourningWoodTrophy,
        ItemID.PumpkingTrophy,
        ItemID.EverscreamTrophy,
        ItemID.SantaNK1Trophy,
        ItemID.IceQueenTrophy,
        ItemID.MartianSaucerTrophy
    ];

    public const string TagKey = "VanillaTrophiesCollected";

    public readonly HashSet<int> CollectedTrophies = [];

    public override QuestType QuestType => QuestType.Player;

    public override void Update()
    {
        if (Main.dedServ)
            return;

        foreach (int trophyType in AllTrophies)
            if (Main.LocalPlayer.inventory.Any(item => item.type == trophyType && item.stack > 0))
                CollectedTrophies.Add(trophyType);
    }

    public override void SaveProgress(TagCompound tag) => tag[TagKey] = CollectedTrophies.ToArray();
    public override void LoadProgress(TagCompound tag) { foreach (int trophyType in tag.GetIntArray(TagKey)) CollectedTrophies.Add(trophyType); }

    public override bool CheckCompletion() => AllTrophies.All(CollectedTrophies.Contains);
}