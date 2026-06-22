using Terraria.ModLoader.IO;

namespace QuestBooks.Utilities
{
    public static partial class Utils
    {
        public static bool[] GetBoolArray(this TagCompound tag, string key) => tag.Get<bool[]>(key);
    }
}
