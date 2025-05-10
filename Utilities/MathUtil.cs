using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace QuestBooks.Utilities
{
    public static partial class Utils
    {
        /// <summary>
        /// Centers a rectangle on a give point.
        /// </summary>
        public static Rectangle CenteredRectangle(Vector2 center, Vector2 size)
        {
            size = new(Math.Abs(size.X), Math.Abs(size.Y));
            return new((int)center.X - ((int)size.X / 2), (int)center.Y - ((int)size.Y / 2), (int)size.X, (int)size.Y);
        }

        /// <summary>
        /// Scales a rectangle around its center.
        /// </summary>
        public static Rectangle Scale(this Rectangle rectangle, float scale)
        {
            return CenteredRectangle(rectangle.Center(), rectangle.Size() * scale);
        }

        /// <summary>
        /// Creates non-scaled margins at the edges of a rectangle.<br/>
        /// Positive margins make the rectangle smaller, negative margins make it bigger.
        /// </summary>
        public static Rectangle CreateMargin(this Rectangle rectangle, float margin)
        {
            return CenteredRectangle(rectangle.Center(), new(rectangle.Width - margin, rectangle.Height - margin));
        }

        /// <summary>
        /// Creates non-scaled margins at the edges of a rectangle.<br/>
        /// Positive margins make the rectangle smaller, negative margins make it bigger.
        /// </summary>
        public static Rectangle CreateMargins(this Rectangle rectangle, int left = 0, int right = 0, int top = 0, int bottom = 0)
        {
            return new Rectangle(rectangle.Left + left, rectangle.Top + top, rectangle.Width - left - right, rectangle.Height - top - bottom);
        }

        /// <summary>
        /// Creates a new rectangle from a "cookie cutter" slice of another rectangle.<br/>
        /// <br/>
        /// <paramref name="center"/> is the position within the given rectangle that you want to center your new rectangle. <c>-1f</c> for the top/left, <c>1f</c> for the bottom/right.<br/>
        /// <paramref name="size"/> is the scale of your new rectangle, based on the size of the given rectangle. <c>1f</c> is the same size as the given rectangle, <c>0.5f</c> is half the size.<br/>
        /// <br/>
        /// Both <paramref name="center"/> and <paramref name="size"/> can be beyond the bounds of the given rectangle (ie. <paramref name="center"/> can be greater/less than <c>1f</c>/<c>-1f</c>, <paramref name="size"/> can be greater/less than <c>1f</c>/<c>0f</c>.
        /// </summary>
        public static Rectangle CookieCutter(this Rectangle rectangle, Vector2 center, Vector2 size)
        {
            Vector2 cookieCenter = rectangle.Center.ToVector2() + (center * rectangle.Size() * 0.5f);
            return CenteredRectangle(cookieCenter, size * rectangle.Size());
        }

        public static Vector2 GetPointOfIntersection(Vector2 linePoint1, Vector2 lineAngle1, Vector2 linePoint2, Vector2 lineAngle2)
        {
            Vector2 p1End = linePoint1 + lineAngle1; // another point in line p1->a1
            Vector2 p2End = linePoint2 + lineAngle2; // another point in line p2->a2

            float m1 = (p1End.Y - linePoint1.Y) / (p1End.X - linePoint1.X); // slope of line p1->a1
            float m2 = (p2End.Y - linePoint2.Y) / (p2End.X - linePoint2.X); // slope of line p2->a2

            float b1 = linePoint1.Y - m1 * linePoint1.X; // y-intercept of line p1->a1
            float b2 = linePoint2.Y - m2 * linePoint2.X; // y-intercept of line p2->a2

            float px = (b2 - b1) / (m1 - m2); // collision x
            float py = m1 * px + b1; // collision y

            return new Vector2(px, py);
        }
    }
}
