using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class DefeatEverySlime : QBQuest
{
    // Despite the fact that mods may add extra slimes, modded slimes should NOT count
    // towards this quest, and as such do not need to be registered with a set
    public static readonly int[] AllSlimes = [
        NPCID.GreenSlime,
        NPCID.BlueSlime,
        NPCID.RedSlime,
        NPCID.PurpleSlime,
        NPCID.YellowSlime,
        NPCID.BlackSlime,
        NPCID.IceSlime,
        NPCID.SandSlime,
        NPCID.JungleSlime,
        NPCID.SpikedIceSlime,
        NPCID.CorruptSlime,
        NPCID.Slimeling,
        NPCID.Slimer,
        NPCID.Crimslime,
        NPCID.IlluminantSlime,
        NPCID.ToxicSludge,
        NPCID.SpikedJungleSlime,
        NPCID.LavaSlime,
        NPCID.DungeonSlime,
        NPCID.UmbrellaSlime,
        NPCID.Pinky,
        NPCID.RainbowSlime,
        NPCID.GoldenSlime,
        NPCID.BabySlime,
        NPCID.MotherSlime,
        NPCID.BunnySlimed,
        NPCID.SlimeRibbonGreen,
        NPCID.SlimeRibbonWhite,
        NPCID.SlimeRibbonRed,
        NPCID.SlimeRibbonYellow,
        NPCID.ShimmerSlime,
        NPCID.WindyBalloon,
        NPCID.Gastropod,
        NPCID.QueenSlimeMinionPink,
        NPCID.QueenSlimeMinionBlue,
        NPCID.QueenSlimeMinionPurple
    ];

    public const string TagKey = "SlimeTypesKilled";

    public readonly HashSet<int> KilledSlimes = [];

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => AllSlimes.All(KilledSlimes.Contains);

    public override void SaveProgress(TagCompound tag) => tag[TagKey] = KilledSlimes.ToArray();
    public override void LoadProgress(TagCompound tag) { foreach (int slimeType in tag.GetIntArray(TagKey)) KilledSlimes.Add(slimeType); }

    public sealed class KillSlimeHook() : KillNPCHook(
        npc => AllSlimes.Contains(npc.netID),
        npc => QuestManager.GetQuest<DefeatEverySlime>().KilledSlimes.Add(npc.netID));
}