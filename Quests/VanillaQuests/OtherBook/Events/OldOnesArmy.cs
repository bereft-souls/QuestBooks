using Terraria.GameContent.Events;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class OOAT1Defeated : VanillaQuest
{
    public override bool CheckCompletion() => DD2Event.DownedInvasionT1;
}

public class OOAT2Defeated : VanillaQuest
{
    public override bool CheckCompletion() => DD2Event.DownedInvasionT2;
}

public class OOAT3Defeated : VanillaQuest
{
    public override bool CheckCompletion() => DD2Event.DownedInvasionT3;
}