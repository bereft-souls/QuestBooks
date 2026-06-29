namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class FindAFairy : VanillaQuest
{
    // Same distance as the lifeform analyzer.
    private const float Distance = 1300f;

    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion()
    {
        foreach (var npc in Main.ActiveNPCs)
            if (npc.DistanceSQ(Main.LocalPlayer.Center) < Distance * Distance && (npc.type == NPCID.FairyCritterBlue || npc.type == NPCID.FairyCritterGreen || npc.type == NPCID.FairyCritterPink))
                return true;

        return false;
    }
}