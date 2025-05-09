using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.Utilities
{
    public static partial class Utils
    {
        public static Rectangle CenteredRectangle(Vector2 center, Vector2 size)
        {
            return new((int)center.X - ((int)size.X / 2), (int)center.Y - ((int)size.Y / 2), (int)size.X, (int)size.Y);
        }
    }
}
