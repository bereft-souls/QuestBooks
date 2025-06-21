using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent;
using Terraria.Localization;
using ReLogic.Graphics;
using Terraria;
using QuestBooks.QuestLog.DefaultQuestLogStyles;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.QuestLog.DefaultQuestLineElements;

namespace QuestBooks.QuestLog.DefaultQuestLines
{
    /// <summary>
    /// Represents a basic <see cref="QuestLine"/> implementation, with a chapter name and contained elements.
    /// </summary>
    public class BasicQuestLine() : QuestLine
    {
        /// <summary>
        /// The list of all elements contained within this quest line.
        /// </summary>
        [JsonIgnore]
        public override IEnumerable<QuestLineElement> Elements { get => ChapterElements; }

        public List<BasicQuestLineElement> ChapterElements = [];

        [JsonIgnore]
        public virtual string DisplayName { get => Language.GetOrRegister(NameKey).Value; }

        public string NameKey;

        /// <summary>
        /// A collection of all quests contained by elements in this quest line.<br/>
        /// These quests are all the actual implemented instances, and not duplicates or templates - you can access accurate info or methods from them.<br/>
        /// Pulls from <see cref="Elements"/>.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Quest> QuestList => ChapterElements.Where(e => e is BasicQuestElement).Cast<BasicQuestElement>().Select(e => e.Quest);

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

        /// <summary>
        /// Forwards <see cref="QuestLineElement.Update"/> calls to each element in the quest line.
        /// </summary>
        public override void Update()
        {
            foreach (QuestLineElement questElement in Elements)
                questElement.Update();
        }

        /// <summary>
        /// Performs the default drawing behavior of for this <see cref="BasicQuestLine"/>. Assigns colors and calls <see cref="DrawBasicChapter(SpriteBatch, string, Color, Color, Color, Rectangle, float)(SpriteBatch, string, Color, Color, Color, Rectangle, float)"/>.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch, Rectangle designatedArea, float scale, bool selected, bool hovered)
        {
            Color color = Color.MediumSeaGreen;

            if (!selected)
                color = Color.Lerp(color, Color.Black, 0.35f);

            if (hovered)
                color = Color.Lerp(color, Color.White, 0.1f);

            Color outlineColor = BasicQuestLogStyle.UseDesigner && selected ? Color.Red : Color.Lerp(color, Color.Black, 0.2f);
            Color textOutlineColor = Color.Lerp(color, Color.Black, 0.4f);
            DrawBasicChapter(spriteBatch, DisplayName, color, Color.White, outlineColor, textOutlineColor, designatedArea, scale);
        }

        /// <summary>
        /// Performs the default chapter drawing code to the spritebatch. Draws a simple container with the specified colors, and text inside the contianer.
        /// </summary>
        public static void DrawBasicChapter(SpriteBatch spriteBatch, string text, Color chapterColor, Color textColor, Color outlineColor, Color textOutlineColor, Rectangle area, float scale)
        {
            spriteBatch.Draw(QuestAssets.LogEntryBackground, area.Center(), null, chapterColor, 0f, QuestAssets.LogEntryBackground.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(QuestAssets.LogEntryBorder, area.Center(), null, outlineColor, 0f, QuestAssets.LogEntryBorder.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, SamplerState.LinearClamp, depth, raster, effect, matrix);

            DrawChapterText(spriteBatch, text, textColor, textOutlineColor, area, scale);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
        }

        /// <summary>
        /// Performs the default chapter text drawing code to the spritebatch. Draws the text as it should sit within the given rectangle with the specified colors.
        /// </summary>
        public static void DrawChapterText(SpriteBatch spriteBatch, string text, Color textColor, Color outlineColor, Rectangle area, float scale)
        {
            Rectangle nameRectangle = area.CreateScaledMargins(left: 0.065f, right: 0.165f, top: 0.1f, bottom: 0.1f);
            float scaleShift = InverseLerp(0.4f, 2f, scale) * 0.8f;
            float stroke = MathHelper.Lerp(1f, 4f, scaleShift);
            Vector2 offset = new(0f, MathHelper.Lerp(2f, 10f, scaleShift) / MathHelper.Clamp(text.Length / 15f, 1f, 2f));

            var font = FontAssets.DeathText.Value;
            spriteBatch.DrawOutlinedStringInRectangle(nameRectangle, font, textColor, outlineColor, text, stroke, offset: offset, extraScale: 0.8f, alignment: Utilities.TextAlignment.Left);
        }

        /// <summary>
        /// Clones the members of this quest book into a new quest line.
        /// </summary>
        public virtual void CloneTo(BasicQuestLine newInstance)
        {
            newInstance.NameKey = NameKey;
            newInstance.ChapterElements = ChapterElements;
        }
    }
}
