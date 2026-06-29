using QuestBooks.Quests.QuestSystems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillDoctorBones : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class KillDoctorBonesCheck() : KillNPCHook<KillDoctorBones>(NPCID.DoctorBones);
}