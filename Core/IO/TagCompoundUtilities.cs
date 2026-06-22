using System.Linq;
using Terraria.ModLoader.IO;

namespace QuestBooks.Core.IO;

public static class TagCompoundExtensions
{
    public static bool[] GetBoolArray(this TagCompound tag, string key) => tag.Get<bool[]>(key);
}