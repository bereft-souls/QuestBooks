using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        // These keep track of the selected QuestLine, as well as
        // some parameters to handle "sliding" between lines
        public static BookChapter SelectedChapter { get; set; } = null;

        private static int BooksScrollOffset = 0;
        private static int ChaptersScrollOffset = 0;
        private static int previousChapterScrollOffset = 0;

        private static readonly List<(Rectangle area, BookChapter questBook)> chapterLibrary = [];
        private void UpdateChapters(Rectangle chapters, Vector2 scaledMouse)
        {
            SwitchTargets(chaptersTarget, LibraryBlending);
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
            foreach (var questLine in AvailableChapters)
            {
                if (!questLine.VisibleInLog() && !UseDesigner)
                    continue;

                chapterLibrary.Add((chapter, questLine));
                chapter = chapter.CookieCutter(new(0f, 2.25f), Vector2.One);
            }

            // Add in the swiping for the previous chapters if there is any
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
                nextChapter.Offset(0, previousChapterScrollOffset - (int)realChaptersScrollOffset);
                xOffset = nextChapter.Width * 1.1f * previousBookSwipeOffset * -sign;

                var previousChapters = previousBook?.Chapters ?? [];
                foreach (var questLine in previousChapters)
                {
                    if (!questLine.VisibleInLog() && !UseDesigner)
                        continue;

                    chapterLibrary.Add((nextChapter, questLine));
                    nextChapter = nextChapter.CookieCutter(new(0f, 2.25f), Vector2.One);
                }
            }

            // Check for selecting a new questline and draw each questline to the area
            foreach (var (rectangle, questLine) in chapterLibrary)
            {
                rectangle.Offset((int)xOffset, (int)realChaptersScrollOffset);
                bool hovered = hoveringChapters && rectangle.Contains(mouseChapters);

                if (hovered && LeftMouseJustReleased && (questLine.IsUnlocked() || UseDesigner) && questElementSwipeOffset == 0f)
                {
                    QuestAreaOffset = Vector2.Zero;
                    int sign = SelectedBook.Chapters.IndexOf(questLine) >= SelectedBook.Chapters.IndexOf(SelectedChapter) ? 1 : -1;
                    questElementSwipeOffset = questAreaTarget.Width * sign;
                    SortedElements = null;

                    SelectedChapter = SelectedChapter != questLine ? questLine : null;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                }

                bool selected = SelectedChapter == questLine;
                DrawTasks.Add(sb => questLine.Draw(sb, rectangle, targetScale, selected, hovered));
            }

            SwitchTargets(null);
        }
    }
}
