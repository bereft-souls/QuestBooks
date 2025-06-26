using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.QuestLog.DefaultChapters;
using QuestBooks.Quests;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog
{
    [ExtendsFromMod("QuestBooks")]
    public abstract class BookChapter
    {
        /// <summary>
        /// The collection of <see cref="ChapterElement"/>s to be displayed in the quest log.
        /// </summary>
        public abstract List<ChapterElement> Elements { get; set; }

        /// <summary>
        /// The string that will be displayed in the quest log. You should use localization here where applicable.
        /// </summary>
        [JsonIgnore]
        public abstract string DisplayName { get; }

        /// <summary>
        /// A collection of all quests contained by elements in this quest line.<br/>
        /// These quests are all the actual implemented instances, and not duplicates or templates - you can access accurate info or methods from them.<br/>
        /// Pulls from <see cref="Elements"/>.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Quest> QuestList => Elements.Where(e => e is QuestElement).Cast<QuestElement>().Select(e => e.Quest);

        /// <summary>
        /// The completion progress of this quest line.<br/>
        /// 0f is no quests completed, and 1f is all quests completed.<br/>
        /// <c>float.NaN</c> is returned for quest lines with no quests in their elements.<br/>
        /// Pulls from <see cref="QuestList"/>.
        /// </summary>
        [JsonIgnore]
        public virtual float Progress => QuestList.Any() ? QuestList.Count(q => q.Completed) / QuestList.Count() : float.NaN;

        /// <summary>
        /// Whether or not all quests in this questline are complete.<br/>
        /// Pulls from <see cref="QuestList"/>.
        /// </summary>
        [JsonIgnore]
        public virtual bool Complete => QuestList.All(q => q.Completed);

        /// <summary>
        /// Determines whether this quest line should appear in the quest log.<br/>
        /// Useful for hiding certain chapters until progression goals are met.
        /// </summary>
        public virtual bool VisibleInLog() => true;

        /// <summary>
        /// Determines whether this quest line should be able to be selected in the quest log.<br/>
        /// Useful for when you want to display a chapter, but hide its contents until progression goals are met.
        /// </summary>
        public virtual bool IsUnlocked() => true;

        /// <summary>
        /// Determines whether this quest line should be "draggable" in the log. This value can be modified from the designer.
        /// </summary>
        public virtual bool EnableShifting { get; set; } = false;

        public virtual void Update()
        {
            foreach (ChapterElement questElement in Elements)
                questElement.Update();
        }

        /// <summary>
        /// Performs the default drawing behavior of for this <see cref="BasicChapter"/>. Assigns colors and calls <see cref="DrawBasicChapter(SpriteBatch, string, Color, Color, Color, Rectangle, float)(SpriteBatch, string, Color, Color, Color, Rectangle, float)"/>.
        /// </summary>
        public abstract void Draw(SpriteBatch spriteBatch, Rectangle designatedArea, float scale, bool selected, bool hovered);

        /// <summary>
        /// Override this to change how the "open quest log" icon is drawn in the inventory.<br/>
        /// <paramref name="drawPriority"/> is the current draw override priority. If you have something of higher priority, modify both <paramref name="drawPriority"/> and <paramref name="iconDraw"/>.
        /// </summary>
        public virtual void OverrideIconDraw(ref float drawPriority, ref QuestLogStyle.IconDrawDelegate iconDraw)
        {

        }

        /// <summary>
        /// Override this to change how the "open quest log" outline is drawn in the inventory.<br/>
        /// <paramref name="drawPriority"/> is the current draw override priority. If you have something of higher priority, modify both <paramref name="drawPriority"/> and <paramref name="outlineDraw"/>.
        /// </summary>
        public virtual void OverrideIconOutlineDraw(ref float drawPriority, ref QuestLogStyle.IconDrawDelegate outlineDraw)
        {

        }

        /// <summary>
        /// Clones the members of this quest book into a new quest line.
        /// </summary>
        public virtual void CloneTo(BookChapter newInstance)
        {
            newInstance.Elements.AddRange(Elements);
        }
    }
}
