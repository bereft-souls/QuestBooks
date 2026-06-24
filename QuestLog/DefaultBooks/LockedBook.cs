using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultQuestBooks;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultBooks
{
    public class LockedBook : TabBook
    {
        public override string DisplayName => Language.GetOrRegister("Mods.QuestBooks.Tooltips.Library.ComingSoon").Value;

        public override bool IsUnlocked() => false;

        public override void Draw(SpriteBatch spriteBatch, Rectangle designatedArea, float scale, bool selected, bool hovered)
        {
            spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, QuestAssets.Grayscale, matrix);
            base.Draw(spriteBatch, designatedArea, scale, selected, hovered);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
        }
    }
}
