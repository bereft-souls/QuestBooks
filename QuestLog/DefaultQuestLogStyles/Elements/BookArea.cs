using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultQuestBooks;
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
        // These keep track of the selected QuestBook, as well as
        // some parameters to handle "sliding" between books
        private static bool previousBookSwipeDirection = false;
        private static float previousBookSwipeOffset = 0f;
        private static QuestBook previousBook = null;
        public static QuestBook SelectedBook = null;

        private static float realBooksScrollOffset = 0;
        private static float realChaptersScrollOffset = 0;

        private static readonly List<(Rectangle area, BasicQuestBook questBook)> bookLibrary = [];
        private void UpdateBooks(Rectangle books, Vector2 scaledMouse)
        {
            SwitchTargets(booksTarget);
            DrawTasks.Add(_ => Main.graphics.GraphicsDevice.Clear(Color.Black * 0.1f));

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
                    int initialOffset = BooksScrollOffset;
                    BooksScrollOffset += scrollAmount;

                    Rectangle lastBook = bookLibrary[^1].area;
                    int minScrollValue = -(lastBook.Bottom - (books.Height + books.Y));

                    BooksScrollOffset = minScrollValue < 0 ? int.Clamp(BooksScrollOffset, minScrollValue, 0) : 0;

                    //if (BooksScrollOffset != initialOffset)
                    //    SoundEngine.PlaySound(SoundID.MenuTick);
                }
            }

            // Smoothly interpolate between the current and next scroll destinations
            realBooksScrollOffset = MathHelper.Lerp(realBooksScrollOffset, BooksScrollOffset, scrollAcceleration);

            // Re-set the books area
            bookLibrary.Clear();
            Rectangle book = books.CookieCutter(new(0f, -0.9f), new(1f, 0.1f));
            bookLibrary.Add((book, AvailableBooks.ElementAt(0)));

            // Add each book basing location off of the first one
            for (int i = 1; i < AvailableBooks.Count(); i++)
            {
                book = book.CookieCutter(new(0f, 2.25f), Vector2.One);
                bookLibrary.Add((book, AvailableBooks.ElementAt(i)));
            }

            // Check for selection of a new book and draw each one
            foreach (var (rectangle, questBook) in bookLibrary)
            {
                rectangle.Offset(0, (int)realBooksScrollOffset);
                bool hovered = hoveringBooks && rectangle.Contains(mouseBooks);

                if (hovered && LeftMouseJustReleased && previousBookSwipeOffset == 0f)
                {
                    previousBook = SelectedBook;
                    SelectedBook = SelectedBook != questBook ? questBook : null;

                    previousChapterScrollOffset = (int)realChaptersScrollOffset;
                    ChaptersScrollOffset = 0;
                    realChaptersScrollOffset = 0f;

                    previousBookSwipeDirection = SelectedBook is null || bookLibrary.FindIndex(kvp => kvp.questBook == SelectedBook) > bookLibrary.FindIndex(kvp => kvp.questBook == previousBook);
                    previousBookSwipeOffset = 1f;
                }

                DrawTasks.Add(sb =>
                {
                    Color color = Color.SlateGray;

                    if (SelectedBook != questBook)
                        color = Color.Lerp(color, Color.Black, 0.25f);

                    if (hovered)
                        color = Color.Lerp(color, Color.White, 0.1f);

                    Color outlineColor = Color.Lerp(color, Color.Black, 0.2f);
                    sb.Draw(QuestAssets.LogEntryBackground, rectangle.Center(), null, color, 0f, QuestAssets.LogEntryBackground.Asset.Size() * 0.5f, targetScale, SpriteEffects.None, 0f);
                    sb.Draw(QuestAssets.LogEntryBorder, rectangle.Center(), null, outlineColor, 0f, QuestAssets.LogEntryBorder.Asset.Size() * 0.5f, targetScale, SpriteEffects.None, 0f);
                    outlineColor = Color.Lerp(outlineColor, Color.Black, 0.4f);

                    string displayName = questBook.DisplayName;
                    Rectangle nameRectangle = rectangle.CreateScaledMargins(left: 0.065f, right: 0.165f, top: 0.1f, bottom: 0.1f);

                    float scaleShift = InverseLerp(0.4f, 2f, LogScale) * 0.8f;
                    float stroke = MathHelper.Lerp(1f, 4f, scaleShift);
                    Vector2 offset = new(0f, MathHelper.Lerp(2f, 10f, scaleShift));

                    var font = FontAssets.DeathText.Value;
                    var (line, drawPos, origin, scale) = GetRectangleStringParameters(nameRectangle, font, displayName, offset: offset, alignment: Utilities.TextAlignment.Left)[0];

                    sb.DrawString(font, line, drawPos + new Vector2(-stroke, 0f), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    sb.DrawString(font, line, drawPos + new Vector2(stroke, 0f), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    sb.DrawString(font, line, drawPos + new Vector2(0f, stroke), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    sb.DrawString(font, line, drawPos + new Vector2(0f, -stroke), outlineColor, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                    sb.DrawString(font, line, drawPos, Color.White, 0f, origin, scale * 0.8f, SpriteEffects.None, 0f);
                });
            }

            SwitchTargets(null);
        }
    }
}
