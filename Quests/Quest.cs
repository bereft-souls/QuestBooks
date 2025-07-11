using QuestBooks.Systems;
using Terraria.ModLoader;

namespace QuestBooks.Quests
{
    /// <summary>
    /// Represents a quest that can be loaded into a quest book.<br/>
    /// By default, quests are tagged with the <see cref="ExtendsFromModAttribute"/> to only load when QuestBooks is enabled.
    /// </summary>
    [ExtendsFromMod("QuestBooks")]
    public abstract class Quest
    {
        /// <summary>
        /// Whether this quest has been completed.<br/>
        /// This will only be accurate on the implementation instance from <see cref="QuestManager.GetQuest{TQuest}()"/> (or one of its overloads)
        /// </summary>
        public bool Completed { get; internal set; }

        /// <summary>
        /// The unique identifier of this quest. Can be anything, but cannot be used by other quests.<br/>
        /// This is used when retrieving <see cref="QuestManager.GetQuest(string)"/>.
        /// </summary>
        public virtual string Key { get => this.GetType().Name; }

        /// <summary>
        /// Use <see cref="QuestType.World"/> for quests that are saved and managed in the world, and <see cref="QuestType.Player"/> for individual player quests.
        /// </summary>
        public virtual QuestType QuestType { get => QuestType.World; }

        /// <summary>
        /// This is called the first time this quest is completed. You can do things like play sounds, spawn items, etc here.<br/>
        /// This can be called on both client and server, so implement your completion accordingly.
        /// </summary>
        public virtual void OnCompletion() { }

        /// <summary>
        /// This is called to actually mark your quest as complete.<br/>
        /// This may be when the world initially loads and quests that were previously completed are being marked, or it may be the first time the quest is marked as completed.<br/>
        /// <br/>
        /// You should *only* modify metadata for your quest or external systems here, not implement completion events.
        /// </summary>
        public virtual void MarkAsComplete() { }

        /// <summary>
        /// Return <see langword="true"/> if this quest should be counted as complete.<br/>
        /// Otherwise return <see langword="false"/>.
        /// </summary>
        public abstract bool CheckCompletion();
    }

    /// <summary>
    /// Represets a quest with some additional required members that allows it to be clicked inside the quest log.
    /// </summary>
    public abstract class ProgressionQuest : Quest
    {

    }

    public enum QuestType
    {
        World,
        Player
    }
}
