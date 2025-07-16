using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace QuestBooks.Quests
{
    public abstract class DynamicQuest : Quest
    {
        public abstract Asset<Texture2D> Texture { get; }

        public virtual Asset<Texture2D> OutlineTexture => null;

        public virtual Asset<Texture2D> IncompleteTexture => null;

        public virtual Asset<Texture2D> LockedTexture => null;
    }
}
