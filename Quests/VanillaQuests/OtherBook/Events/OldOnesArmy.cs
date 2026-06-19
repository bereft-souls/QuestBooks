using Terraria.GameContent.Events;

namespace QuestBooks.Quests.VanillaQuests.OtherBook.Events;

public class OOAT1Defeated : QBQuest
{
    public override bool CheckCompletion() => DD2Event.DownedInvasionT1;
}

public class OOAT2Defeated : QBQuest
{
    public override bool CheckCompletion() => DD2Event.DownedInvasionT2;
}

public class OOAT3Defeated : QBQuest
{
    public override bool CheckCompletion() => DD2Event.DownedInvasionT3;
}