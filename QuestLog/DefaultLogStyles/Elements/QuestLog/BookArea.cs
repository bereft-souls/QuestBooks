using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        // These keep track of the selected QuestBook, as well as
        // some parameters to handle "sliding" between books
        private static bool previousBookSwipeDirection = false;
        private static float previousBookSwipeOffset = 0f;
        private static QuestBook previousBook = null;
        public static QuestBook SelectedBook { get; set; } = null;

        private static float realBooksScrollOffset = 0;
        private static float realChaptersScrollOffset = 0;

        private static readonly List<(Rectangle area, QuestBook questBook)> bookLibrary = [];
        private void UpdateBooks(Rectangle books, Vector2 scaledMouse)
        {
            SwitchTargets(booksTarget, LibraryBlending, SamplerState.PointClamp);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Black * 0.08f));

            // Skip drawing books if none are available
            if (!AvailableBooks.Any())
            {
                SwitchTargets(null);
                return;
            }

            // Cache the mouse position within the books and whether
            // the book area is being hovered
            var mouseBooks = scaledMouse.ToPoint();
            bool hoveringBooks = false;

            // Handle the scrolling of the books area
            if (books.Contains(mouseBooks))
            {
                int data = PlayerInput.ScrollWheelDeltaForUI;
                hoveringBooks = true;

                if (data != 0)
                {
                    int scrollAmount = data / 6;
                    int initialOffset = booksScrollOffset;
                    booksScrollOffset += scrollAmount;

                    Rectangle lastBook = bookLibrary[^1].area;
                    int minScrollValue = -(lastBook.Bottom - (books.Height + books.Y));

                    booksScrollOffset = minScrollValue < 0 ? int.Clamp(booksScrollOffset, minScrollValue, 0) : 0;

                    //if (BooksScrollOffset != initialOffset)
                    //    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }

            // Smoothly interpolate between the current and next scroll destinations
            realBooksScrollOffset = MathHelper.Lerp(realBooksScrollOffset, booksScrollOffset, scrollAcceleration);

            // Re-set the books area
            bookLibrary.Clear();

            // Assign the position of the first book in the library
            Rectangle book = books.CookieCutter(new(0f, -0.9f), new(1f, 0.1065f));

            // Add each book basing location off of the first one
            foreach (var questBook in AvailableBooks)
            {
                if (!questBook.VisibleInLog() && !UseDesigner)
                    continue;

                bookLibrary.Add((book, questBook));
                book = book.CookieCutter(new(0f, 2.25f), Vector2.One);
            }

            // Check for selection of a new book and draw each one
            foreach (var (rectangle, questBook) in bookLibrary)
            {
                rectangle.Offset(0, (int)realBooksScrollOffset);
                bool hovered = hoveringBooks && rectangle.Contains(mouseBooks) && SelectedElement is null;

                if (hovered && LeftMouseJustReleased && (questBook.IsUnlocked() || UseDesigner) && previousBookSwipeOffset == 0f)
                {
                    previousBook = SelectedBook;
                    SelectedBook = SelectedBook != questBook ? questBook : null;
                    SoundEngine.PlaySound(SoundID.MenuTick);

                    previousChapterScrollOffset = (int)realChaptersScrollOffset;
                    chaptersScrollOffset = 0;
                    realChaptersScrollOffset = 0f;

                    previousBookSwipeDirection = SelectedBook is null || bookLibrary.FindIndex(kvp => kvp.questBook == SelectedBook) > bookLibrary.FindIndex(kvp => kvp.questBook == previousBook);
                    previousBookSwipeOffset = 1f;
                }

                bool selected = SelectedBook == questBook;
                DrawTasks.Add(sb => questBook.Draw(sb, rectangle, TargetScale, selected, hovered));
            }

            SwitchTargets(null);

            if (!UseDesigner)
                return;
        }
    }
}
