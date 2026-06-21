using QuestBooks.Systems;
using QuestBooks.Utilities;
using Terraria.GameContent.Events;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class EventParty : QBQuest
{
    public override bool CheckCompletion() => BirthdayParty.PartyIsUp;
}