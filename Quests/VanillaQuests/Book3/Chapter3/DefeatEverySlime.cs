using QuestBooks.Quests.QuestSystems;
using QuestBooks.Systems;
using System.Collections.Generic;
using System.Linq;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class DefeatEverySlime : QBQuest
{
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

    public readonly HashSet<int> KilledSlimes = [];

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => AllSlimes.All(KilledSlimes.Contains);

    public sealed class KillSlimeHook() : KillNPCHook(
        npc => AllSlimes.Contains(npc.netID),
        npc => QuestManager.GetQuest<DefeatEverySlime>().KilledSlimes.Add(npc.netID));
}