using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        /// The text that should display in the mouse tooltip whenever this quest is hovered over in the quest log.<br/>
        /// This value is not required.
        /// </summary>
        public virtual string HoverTooltip => null;

        /// <summary>
        /// Override this to indicate whether the quest should display an "info page" in the quest log when clicked.
        /// </summary>
        public virtual bool HasInfoPage => false;

        /// <summary>
        /// Override this method to implement your own custom drawing for info pages in the quest log.<br/>
        /// Note that your logic NOT scale with any UI parameters, as scaling is handled via matrices and render targets here.
        /// </summary>
        public virtual void DrawCustomInfoPage(SpriteBatch spriteBatch, Vector2 mousePosition) { }

        /// <summary>
        /// Override this method to modify the parameters for a "simple info page" drawing.<br/>
        /// This will use the default info page draw logic.<br/>
        /// You can return <see langword="null"/> for any of the parameters if you do not want to assign them.
        /// </summary>
        /// 
        /// <param name="title">The "title" of this quest. Displays in an info page when this quest is clicked in the quest log.<br/>
        /// Quests that do not implement this nor <paramref name="contents"/> will not be clickable in the quest log.</param>
        /// 
        /// <param name="contents">A "description" for this quest. Displays in an info page when this quest is clicked in the quest log.<br/>
        /// Quests that do not implement this nor <paramref name="title"/> will not be clickable in the quest log.</param>
        /// 
        /// <param name="texture">An image to display in an info page when this quest is clicked in the quest log.<br/>
        /// Quests that do not implement this will not draw a texture to their page.<br/>
        /// The texture draws in the upper-righthand corner of the page.</param>
        /// 
        public virtual void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = null;
            contents = null;
            texture = null;
        }

        /// <summary>
        /// Use <see cref="QuestType.World"/> for quests that are saved and managed in the world, and <see cref="QuestType.Player"/> for individual player quests.
        /// </summary>
        public virtual QuestType QuestType { get => QuestType.World; }

        /// <summary>
        /// This is called every frame, regardless of whether the quest is completed. You can do any logic updating, dynamic quest updating, etc. here.<br/>
        /// This can be called on both client and server, so implement your updating accordingly.
        /// </summary>
        public virtual void Update() { }

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

    public enum QuestType
    {
        World,
        Player
    }
}
