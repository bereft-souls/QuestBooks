using QuestBooks.Systems;
using Terraria.ModLoader;

namespace QuestBooks.Commands
{
    internal abstract class QuestBooksCommand : ModCommand, ILocalizedModType
    {
        public override CommandType Type => CommandType.World;

        public virtual string LocalizationCategory => "ChatMessages.Commands";

        public sealed override void Action(CommandCaller caller, string input, string[] args) => Run(caller, args);

        public abstract void Run(CommandCaller caller, string[] args);

        protected static string TryFormatQuestKey(string questKey)
        {
            foreach (string key in QuestManager.ActiveQuests.Keys)
                if (questKey.Equals(key, System.StringComparison.OrdinalIgnoreCase))
                    return key;

            return questKey;
        }
    }
}
