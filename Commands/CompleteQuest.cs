using QuestBooks.Systems;

namespace QuestBooks.Commands
{
    internal class CompleteQuest : QuestBooksCommand
    {
        public override string Command => "completequest";

        public override void Run(CommandCaller caller, string[] args)
        {
            if (args.Length != 1)
            {
                caller.Reply(this.GetLocalizedValue("Usage"));
                return;
            }

            string questKey = TryFormatQuestKey(args[0]);
            if (!QuestManager.TryGetQuest(questKey, out var quest))
            {
                caller.Reply(this.GetLocalization("NoMatch").Format(questKey));
                return;
            }

            if (quest.Completed)
            {
                caller.Reply(this.GetLocalization("AlreadyComplete").Format(questKey));
                return;
            }

            QuestManager.CompleteQuest(quest);
            caller.Reply(this.GetLocalization("Success").Format(questKey));
        }
    }
}
