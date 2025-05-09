using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    internal class BasicQuestLogStyle : QuestLogStyle
    {
        public override string Key => "DefaultQuestLog";

        public override string DisplayName => "Book";

        public static QuestBook SelectedBook = null;
        public static QuestLine SelectedLine = null;
        public static QuestLineElement SelectedElement = null;



        #region Normal Log

        public override void UpdateLog()
        {
            if (!QuestLogDrawer.DisplayLog)
                return;

            Vector2 logSize = QuestAssets.BasicQuestCanvas.Texture.Size();
            if (Utils.CenteredRectangle(Main.ScreenSize.ToVector2() / 2f, logSize).Contains(Main.MouseScreen.ToPoint()))
                Main.LocalPlayer.mouseInterface = true;
        }

        public override void DrawLog(SpriteBatch spriteBatch)
        {
            Texture2D logTexture = QuestAssets.BasicQuestCanvas;
            spriteBatch.Draw(logTexture, Main.ScreenSize.ToVector2() / 2f, null, Color.White, 0f, logTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
        }

        #endregion

        #region Designer

        public override void UpdateDesigner()
        {

        }

        public override void DrawDesigner(SpriteBatch spriteBatch)
        {
            
        }

        #endregion
    }
}
