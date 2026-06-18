using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests;

/// <summary>
/// Intended for internal QuestBooks use.<br/>
/// Shorthands the localization category based on namespace instead of <c>QuestBooks</c>.
/// </summary>
public abstract class QBQuest : Quest
{
    private string _localizationCategory = null;

    public override string TextureCategory => $"{Mod.Name}/Assets/Textures/Quests/InfoPages";

    public override string LocalizationCategory
    {
        get
        {
            _localizationCategory ??= GetType().Namespace[GetType().Namespace.LastIndexOf("Book")..];
            return _localizationCategory;
        }
    }
}

/// <summary>
/// Intended for internal QuestBooks use.<br/>
/// Acts as a quest that is marked as completed once it has been opened.
/// </summary>
public abstract class InfoQuest : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public bool Read { get; set; } = false;

    public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
    {
        base.MakeSimpleInfoPage(out title, out contents, out texture);
        Read = true;
    }

    public override bool CheckCompletion() => Read;
}

/// <summary>
/// Intended for internal QuestBooks use.<br/>
/// Shorthands the localization category based on namespace instead of <c>QuestBooks</c>.
/// </summary>
public abstract class QBDynamicQuest : DynamicQuest
{
    private string _localizationCategory = null;

    public override string TextureCategory => $"{Mod.Name}/Assets/Textures/Quests/InfoPages";

    public override string LocalizationCategory
    {
        get
        {
            _localizationCategory ??= GetType().Namespace[GetType().Namespace.LastIndexOf("Book")..];
            return _localizationCategory;
        }
    }
}

internal class Placeholder : Quest
{
    public override bool CheckCompletion() => true;

    public override string LocalizationCategory => "Tooltips";
}

internal class PlaceholderDynamic : DynamicQuest
{
    public override bool CheckCompletion() => true;

    public override string LocalizationCategory => "Tooltips";

    public readonly Texture2D IconTexture = Main.dedServ ? null : ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/Quests/Medium").Value;
    public readonly Texture2D OutlineTexture = Main.dedServ ? null : ModContent.Request<Texture2D>("QuestBooks/Assets/Textures/Quests/MediumOutline").Value;

    public override void DrawLocked(SpriteBatch spriteBatch, Vector2 canvasPosition, Vector2 canvasOffset, float zoom, bool hovered, bool selected)
    {
        DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, Color.Black);
        DrawTexture(spriteBatch, IconTexture, canvasPosition, canvasOffset, zoom, Color.Black);
    }

    public override void DrawIncomplete(SpriteBatch spriteBatch, Vector2 canvasPosition, Vector2 canvasOffset, float zoom, bool hovered, bool selected)
    {
        Effect grayscale = QuestAssets.Grayscale;

        spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, grayscale, matrix);
        DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, Color.Gray);
        DrawTexture(spriteBatch, IconTexture, canvasPosition, canvasOffset, zoom, Color.White);
        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
    }

    public override void DrawCompleted(SpriteBatch spriteBatch, Vector2 canvasPosition, Vector2 canvasOffset, float zoom, bool hovered, bool selected)
    {
        if (selected)
            DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, Color.Yellow);

        else if (hovered)
            DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, Color.LightGray);

        else
            DrawOutline(spriteBatch, canvasPosition, canvasOffset, zoom, new(108, 118, 199, 255));

        DrawTexture(spriteBatch, IconTexture, canvasPosition, canvasOffset, zoom, Color.White);
    }

    protected void DrawOutline(SpriteBatch spriteBatch, Vector2 canvasPosition, Vector2 canvasOffset, float zoom, Color color) =>
        DrawTexture(spriteBatch, OutlineTexture, canvasPosition, canvasOffset, zoom, color);

    protected static void DrawTexture(SpriteBatch spriteBatch, Texture2D texture, Vector2 canvasPosition, Vector2 canvasOffset, float zoom, Color color)
    {
        Vector2 drawPos = (canvasPosition - canvasOffset) * zoom;
        spriteBatch.Draw(texture, drawPos, null, color, 0f, texture.Size() * 0.5f, zoom, SpriteEffects.None, 0f);
    }

    public override Vector2 HoverAreaSize(bool unlocked) => IconTexture.Size();
}

internal class IncompletePlaceholder : Placeholder
{
    public override bool CheckCompletion() => false;
}