using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultQuestLines;
using QuestBooks.QuestLog.DefaultQuestLogStyles;
using QuestBooks.Quests;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using static System.Net.Mime.MediaTypeNames;

namespace QuestBooks.QuestLog.DefaultQuestBooks
{
    /// <summary>
    /// Represents a basic <see cref="QuestBook"/> implementation, containing a set of <see cref="QuestLine"/>s.
    /// </summary>
    public class BasicQuestBook() : QuestBook
    {
        /// <summary>
        /// The list of all quest lines contained within this book.
        /// </summary>
        [JsonIgnore]
        public override IEnumerable<QuestLine> Chapters => QuestLines;

        public List<BasicQuestLine> QuestLines = [];

        [JsonIgnore]
        public virtual string DisplayName { get => Language.GetOrRegister(NameKey).Value; }

        public string NameKey;

        /// <summary>
        /// A collection of all quests contained by all elements in all chapters in this quest book.<br/>
        /// These quests are all the actual implemented instances, and not duplicates or templates - you can access accurate info or methods from them.<br/>
        /// Pulls from <see cref="BasicQuestLine.Elements"/>.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Quest> QuestList => QuestLines.SelectMany(x => x.QuestList).Distinct();

        /// <summary>
        /// The completion progres of this quest book.<br/>
        /// 0f is no quests completed, and 1f is all quests completed.<br/>
        /// <c>float.NaN</c> is returned for quest books that contain no quests in any of their chapters.<br/>
        /// Pulls from <see cref="BasicQuestLine.QuestList"/>.
        /// </summary>
        [JsonIgnore]
        public virtual float Progress
        {
            get
            {
                float applicableCount = QuestLines.Count(x => !float.IsNaN(x.Progress));
                return applicableCount > 0 ? QuestLines.Where(x => !float.IsNaN(x.Progress)).Sum(x => x.Progress) / applicableCount : float.NaN;
            }
        }

        /// <summary>
        /// Whether or not all quests in all chapters of this quest book are complete.<br/>
        /// Pulls from <see cref="BasicQuestLine.QuestList"/>.
        /// </summary>
        [JsonIgnore]
        public virtual bool Complete => QuestLines.All(x => x.Complete);

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

        public override void Update()
        {
            foreach (BasicQuestLine questLine in QuestLines)
                questLine.Update();
        }

        /// <summary>
        /// Performs the default drawing behavior of for this <see cref="BasicQuestBook"/>. Assigns colors and calls <see cref="DrawBasicBook(SpriteBatch, string, Color, Color, Color, Rectangle, float)"/>.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch, Rectangle designatedArea, float scale, bool selected, bool hovered)
        {
            Color color = Color.SlateGray;

            if (!selected)
                color = Color.Lerp(color, Color.Black, 0.25f);

            if (hovered)
                color = Color.Lerp(color, Color.White, 0.1f);

            Color outlineColor = BasicQuestLogStyle.UseDesigner && selected ? Color.Red : Color.Lerp(color, Color.Black, 0.2f);
            Color textOutlineColor = Color.Lerp(color, Color.Black, 0.4f);
            DrawBasicBook(spriteBatch, DisplayName, color, Color.White, outlineColor, textOutlineColor, designatedArea, scale);
        }

        /// <summary>
        /// Performs the default book drawing code to the spritebatch. Draws a simple container with the specified colors, and text inside that container.
        /// </summary>
        public static void DrawBasicBook(SpriteBatch spriteBatch, string text, Color bookColor, Color textColor, Color outlineColor, Color textOutlineColor, Rectangle area, float scale)
        {
            spriteBatch.Draw(QuestAssets.LogEntryBackground, area.Center(), null, bookColor, 0f, QuestAssets.LogEntryBackground.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(QuestAssets.LogEntryBorder, area.Center(), null, outlineColor, 0f, QuestAssets.LogEntryBorder.Asset.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            if (string.IsNullOrWhiteSpace(text))
                return;

            spriteBatch.End();
            spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, SamplerState.LinearClamp, depth, raster, effect, matrix);

            DrawBookText(spriteBatch, text, textColor, textOutlineColor, area, scale);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
        } 

        /// <summary>
        /// Performs the default book text drawing code to the spritebatch. Draws the text as it should sit within the given rectangle with the specified colors.
        /// </summary>
        public static void DrawBookText(SpriteBatch spriteBatch, string text, Color textColor, Color outlineColor, Rectangle area, float scale)
        {
            Rectangle nameRectangle = area.CreateScaledMargins(left: 0.065f, right: 0.065f, top: 0.1f, bottom: 0.1f);
            float scaleShift = InverseLerp(0.4f, 2f, scale) * 0.8f;
            float stroke = MathHelper.Lerp(1f, 4f, scaleShift);
            Vector2 offset = new(0f, MathHelper.Lerp(2f, 10f, scaleShift) / MathHelper.Clamp(text.Length / 15f, 1f, 2f));

            var font = FontAssets.DeathText.Value;
            var (line, drawPos, origin, textScale) = GetRectangleStringParameters(nameRectangle, font, text, offset: offset, alignment: Utilities.TextAlignment.Left)[0];
            textScale *= 0.8f;

            spriteBatch.DrawOutlinedString(font, line, drawPos, origin, textScale, stroke, outlineColor, textColor);
        }

        /// <summary>
        /// Clones the members of this quest book into a new quest book.
        /// </summary>
        public virtual void CloneTo(BasicQuestBook newInstance)
        {
            newInstance.NameKey = NameKey;
            newInstance.QuestLines = QuestLines;
        }
    }
}
