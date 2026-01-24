using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuestBooks.QuestLog.DefaultStyles
{
    public partial class BasicQuestLogStyle
    {
        private void UpdateDesigner(Rectangle books, Rectangle chapters, Rectangle questArea)
        {
            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
            });

            HandleRenaming(books, chapters, questArea);
            HandleAddDeleteButtons(books, chapters, questArea);
            HandleSaveLoadButtons();
            HandleTypeSelection();

            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            });
        }
    }
}
