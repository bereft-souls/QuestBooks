using Terraria.GameContent.Events;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class EventParty : VanillaQuest
{
    public override bool CheckCompletion() => BirthdayParty.PartyIsUp;
}