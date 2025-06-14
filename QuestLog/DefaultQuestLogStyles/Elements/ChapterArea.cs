using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultQuestLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.ID;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    public partial class BasicQuestLogStyle
    {
        // These keep track of the selected QuestLine, as well as
        // some parameters to handle "sliding" between lines
        private static bool previousLineSwipeDirection = false;
        private static float previousLineSwipeOffset = 0f;
        private static QuestLine previousLine = null;
        public static QuestLine SelectedChapter = null;

        private static int BooksScrollOffset = 0;
        private static int ChaptersScrollOffset = 0;
        private static int previousChapterScrollOffset = 0;

        private static readonly List<(Rectangle area, BasicQuestLine questBook)> chapterLibrary = [];
        private void UpdateChapters(Rectangle chapters, Vector2 scaledMouse)
        {
            SwitchTargets(chaptersTarget);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Black * 0.1f));

            // If we aren't in the middle of swiping and there are no chapters to draw,
            // return early
            if (previousBookSwipeOffset == 0f && !AvailableChapters.Any())
            {
                SwitchTargets(null);
                return;
            }

            // Cache mouse position within chapters and whether the chapter area contians the mouse
            var mouseChapters = scaledMouse.ToPoint();
            bool hoveringChapters = false;

            // Handle the scrolling of the chapter area
            if (chapters.Contains(mouseChapters))
            {
                hoveringChapters = true;
                int data = PlayerInput.ScrollWheelDeltaForUI;

                if (data != 0)
                {
                    int scrollAmount = (int)(data / 2.5f);
                    int initialScrollOffset = ChaptersScrollOffset;
                    ChaptersScrollOffset += scrollAmount;

                    Rectangle lastBook = chapterLibrary[^1].area;
                    int minScrollValue = -(lastBook.Bottom - (chapters.Height + chapters.Y));

                    ChaptersScrollOffset = minScrollValue < 0 ? int.Clamp(ChaptersScrollOffset, minScrollValue, 0) : 0;

                    //if (ChaptersScrollOffset != initialScrollOffset)
                    //    SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.3f });
                }
            }

            // Lerp to the real scroll value to create smooth transitions
            realChaptersScrollOffset = MathHelper.Lerp(realChaptersScrollOffset, ChaptersScrollOffset, scrollAcceleration);

            // Re-set the chapter library rectangles
            chapterLibrary.Clear();
            Rectangle chapter = chapters.CookieCutter(new(0f, -0.9f), new(1f, 0.1f));
            float xOffset = 0f;

            // Add any available chapters
            if (AvailableChapters.Any())
            {
                chapterLibrary.Add((chapter, AvailableChapters.ElementAt(0)));

                for (int i = 1; i < AvailableChapters.Count(); i++)
                {
                    chapter = chapter.CookieCutter(new(0f, 2.25f), Vector2.One);
                    chapterLibrary.Add((chapter, AvailableChapters.ElementAt(i)));
                }
            }

            // Add in the swiping for the previous chapter if there is any
            if (previousBookSwipeOffset > 0f)
                previousBookSwipeOffset = MathHelper.Lerp(previousBookSwipeOffset, 0f, 0.25f);

            // Check if we've completed the swiping
            if (previousBookSwipeOffset <= 0.005f)
            {
                previousBook = null;
                previousBookSwipeOffset = 0f;
            }

            // Otherwise add in the previously visible chapters
            else
            {
                // We swipe from right to left if going "forward" chapters,
                // and left to right if going "back" chapters
                int sign = previousBookSwipeDirection ? -1 : 1;
                var firstChapter = chapters.CookieCutter(new(0f, -0.9f), new(1f, 0.1f));

                Rectangle nextChapter = firstChapter.CookieCutter(new(2.2f * sign, 0f), Vector2.One);
                xOffset = nextChapter.Width * 1.1f * previousBookSwipeOffset * -sign;

                var previousChapters = previousBook?.Chapters.Cast<BasicQuestLine>() ?? [];

                // Draw the previous chapters, if there are any
                if (previousChapters.Any())
                {
                    nextChapter.Offset(0, previousChapterScrollOffset - (int)realChaptersScrollOffset);
                    chapterLibrary.Add((nextChapter, previousChapters.ElementAt(0)));

                    for (int i = 1; i < previousChapters.Count(); i++)
                    {
                        nextChapter = nextChapter.CookieCutter(new(0f, 2.25f), Vector2.One);
                        chapterLibrary.Add((nextChapter, previousChapters.ElementAt(i)));
                    }
                }
            }

            // Check for selecting a new questline and draw each questline to the area
            foreach (var (rectangle, questLine) in chapterLibrary)
            {
                rectangle.Offset((int)xOffset, (int)realChaptersScrollOffset);
                bool hovered = hoveringChapters && rectangle.Contains(mouseChapters);

                if (hovered && LeftMouseJustReleased && previousLineSwipeOffset == 0f)
                {
                    SelectedChapter = SelectedChapter != questLine ? questLine : null;
                    //QuestAreaPositionOffset = Vector2.Zero;
                }

                DrawTasks.Add(sb =>
                {
                    Color color = Color.MediumSeaGreen;

                    if (SelectedChapter != questLine)
                        color = Color.Lerp(color, Color.Black, 0.35f);

                    if (hovered)
                        color = Color.Lerp(color, Color.White, 0.1f);

                    Color outlineColor = Color.Lerp(color, Color.Black, 0.2f);
                    sb.Draw(QuestAssets.LogEntryBackground, rectangle.Center(), null, color, 0f, QuestAssets.LogEntryBackground.Asset.Size() * 0.5f, targetScale, SpriteEffects.None, 0f);
                    sb.Draw(QuestAssets.LogEntryBorder, rectangle.Center(), null, outlineColor, 0f, QuestAssets.LogEntryBorder.Asset.Size() * 0.5f, targetScale, SpriteEffects.None, 0f);
                    outlineColor = Color.Lerp(outlineColor, Color.Black, 0.4f);

                    string displayName = questLine.DisplayName;
                    Rectangle nameRectangle = rectangle.CreateScaledMargins(left: 0.065f, right: 0.165f, top: 0.1f, bottom: 0.1f);

                    float scaleShift = InverseLerp(0.4f, 2f, LogScale) * 0.8f;
                    float stroke = MathHelper.Lerp(1f, 4f, scaleShift);
                    Vector2 offset = new(0f, MathHelper.Lerp(2f, 10f, scaleShift));

                    var font = FontAssets.DeathText.Value;
                    var (line, drawPos, origin, scale) = GetRectangleStringParameters(nameRectangle, font, displayName, offset: offset, alignment: Utilities.TextAlignment.Left)[0];

                    sb.End();
                    sb.Begin(SpriteSortMode.Deferred, CustomBlendState, SamplerState.LinearClamp, CustomDepthStencilState, CustomRasterizerState);

                    sb.DrawString(font, line, drawPos + new Vector2(-stroke, 0f), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    sb.DrawString(font, line, drawPos + new Vector2(stroke, 0f), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    sb.DrawString(font, line, drawPos + new Vector2(0f, stroke), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    sb.DrawString(font, line, drawPos + new Vector2(0f, -stroke), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    sb.DrawString(font, line, drawPos, Color.White, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);

                    sb.End();
                    sb.Begin(SpriteSortMode.Deferred, CustomBlendState, CustomSamplerState, CustomDepthStencilState, CustomRasterizerState);
                });
            }

            SwitchTargets(null);
        }
    }
}
