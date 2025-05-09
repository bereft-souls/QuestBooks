using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.QuestLog
{
    public abstract class QuestLogStyle
    {
        public abstract string Key { get; }
        public abstract string DisplayName { get; }

        public virtual void OnToggle(bool active) { }

        public virtual void UpdateLog() { }

        public virtual void UpdateDesigner() { }

        public abstract void DrawLog(SpriteBatch spriteBatch);

        public abstract void DrawDesigner(SpriteBatch spriteBatch);
    }
}
