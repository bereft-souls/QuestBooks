using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.Quests;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog
{
    [ExtendsFromMod("QuestBooks")]
    public abstract class QuestBook
    {
        /// <summary>
        /// The list of all quest lines contained within this book.
        /// </summary>
        public abstract List<BookChapter> Chapters { get; set; }

        /// <summary>
        /// The string that will be displayed in the quest log. You should use localization here where applicable.
        /// </summary>
        [JsonIgnore]
        public abstract string DisplayName { get; }

        /// <summary>
        /// A collection of all quests contained by all elements in all chapters in this quest book.<br/>
        /// These quests are all the actual implemented instances, and not duplicates or templates - you can access accurate info or methods from them.<br/>
        /// Pulls from <see cref="BasicChapter.Elements"/>.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Quest> QuestList => Chapters.SelectMany(x => x.QuestList).Distinct();

        /// <summary>
        /// The completion progres of this quest book.<br/>
        /// 0f is no quests completed, and 1f is all quests completed.<br/>
        /// <c>float.NaN</c> is returned for quest books that contain no quests in any of their chapters.<br/>
        /// Pulls from <see cref="BasicChapter.QuestList"/>.
        /// </summary>
        [JsonIgnore]
        public virtual float Progress
        {
            get
            {
                int applicableCount = 0;
                float totalProgress = Chapters
                    .Where(x => !float.IsNaN(x.Progress))
                    .Select(x => { applicableCount++; return x; })
                    .Sum(x => x.Progress);

                return applicableCount > 0 ? totalProgress / applicableCount : float.NaN;
            }
        }

        /// <summary>
        /// Whether or not all quests in all chapters of this quest book are complete.<br/>
        /// Pulls from <see cref="BasicChapter.QuestList"/>.
        /// </summary>
        [JsonIgnore]
        public virtual bool Complete => Chapters.All(x => x.Complete);

        public virtual void Update()
        {
            foreach (BookChapter chapter in Chapters)
                chapter.Update();
        }

        /// <summary>
        /// Performs the default drawing behavior of for this <see cref="TabBook"/>. Assigns colors and calls <see cref="DrawBasicBook(SpriteBatch, string, Color, Color, Color, Rectangle, float)"/>.
        /// </summary>
        public abstract void Draw(SpriteBatch spriteBatch, Rectangle designatedArea, float scale, bool selected, bool hovered);

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
        /// Override this to change how the "open quest log" icon is drawn in the inventory.<br/>
        /// <paramref name="drawPriority"/> is the current draw override priority. If you have something of higher priority, modify both <paramref name="drawPriority"/> and <paramref name="iconDraw"/>.
        /// </summary>
        public virtual void OverrideIconDraw(ref float drawPriority, ref QuestLogStyle.IconDrawDelegate iconDraw)
        {
            foreach (var questLine in Chapters)
                questLine.OverrideIconDraw(ref drawPriority, ref iconDraw);
        }

        /// <summary>
        /// Override this to change how the "open quest log" outline is drawn in the inventory.<br/>
        /// <paramref name="drawPriority"/> is the current draw override priority. If you have something of higher priority, modify both <paramref name="drawPriority"/> and <paramref name="outlineDraw"/>.
        /// </summary>
        public virtual void OverrideIconOutlineDraw(ref float drawPriority, ref QuestLogStyle.IconDrawDelegate outlineDraw)
        {
            foreach (var questLine in Chapters)
                questLine.OverrideIconOutlineDraw(ref drawPriority, ref outlineDraw);
        }

        /// <summary>
        /// Clones the members of this quest book into a new quest book.
        /// </summary>
        public virtual void CloneTo(QuestBook newInstance)
        {
            newInstance.Chapters.AddRange(Chapters);
        }

        /// <summary>
        /// Clones the members of an old quest book into this new quest book.
        /// </summary>
        public virtual void CloneFrom(QuestBook oldInstance)
        {

        }
    }
}
