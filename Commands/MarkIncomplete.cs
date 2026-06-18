using QuestBooks.Systems;
using Terraria.ModLoader;

namespace QuestBooks.Commands
{
    internal class MarkIncomplete : QuestBooksCommand
    {
        public override string Command => "markincomplete";

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

            if (!quest.Completed)
            {
                caller.Reply(this.GetLocalization("NotComplete").Format(questKey));
                return;
            }

            QuestBooksMod.MarkIncomplete(quest);
            caller.Reply(this.GetLocalization("Success").Format(questKey));
        }
    }
}
