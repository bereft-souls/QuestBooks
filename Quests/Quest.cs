using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Systems;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests
{
    /// <summary>
    /// Represents a quest that can be loaded into a quest book.<br/>
    /// By default, quests are tagged with the <see cref="ExtendsFromModAttribute"/> to only load when QuestBooks is enabled.
    /// </summary>
    [ExtendsFromMod("QuestBooks")]
    public abstract class Quest : ModType, ILocalizedModType
    {
        /// <summary>
        /// Whether this quest has been completed.<br/>
        /// This will only be accurate on the implementation instance from <see cref="QuestManager.GetQuest{TQuest}()"/> (or one of its overloads)
        /// </summary>
        public bool Completed { get; internal set; }

        /// <summary>
        /// The unique identifier of this quest. Can be anything, but cannot be used by other quests.<br/>
        /// This is used when retrieving <see cref="QuestManager.GetQuest(string)"/>.
        /// </summary>
        public virtual string Key { get => GetType().Name; }

        /// <summary>
        /// Allows you to modify the collection of objects passed to all of this quest's localization retrievals.<br/>
        /// The default contains only 1 entry, which is a color string (green if the quest is completed, yellow if not).
        /// </summary>
        public virtual object[] LocalizationArgs { get => [(Completed ? new Color(20, 210, 70) : new Color(255, 240, 0))]; }

        /// <summary>
        /// The text that should display in the mouse tooltip whenever this quest is hovered over in the quest log.<br/>
        /// This value is not required.
        /// </summary>
        public virtual string HoverTooltip => Language.Exists(this.GetLocalizationKey("Tooltip")) ? this.GetLocalization("Tooltip").Format(LocalizationArgs) : null;

        /// <summary>
        /// The text that should display in the mouse tooltip whenever this quest is both locked and being hovered over in the quest log.<br/>
        /// This value is not required.
        /// </summary>
        public virtual string LockedTooltip => Language.Exists(this.GetLocalizationKey("LockedTooltip")) ? this.GetLocalization("LockedTooltip").Format(LocalizationArgs) : Language.GetTextValue("Mods.QuestBooks.Tooltips.Library.LockedTooltip");

        /// <summary>
        /// Override this method to implement your own custom drawing for info pages in the quest log.<br/>
        /// Note that your logic should NOT scale with any UI parameters, as scaling is handled via matrices and render targets here.<br/>
        /// The entire drawing area is roughly 450x550.<br/>
        /// Return <see langword="true"/> if you did custom drawing, otherwise <see langword="false"/>.
        /// </summary>
        public virtual bool DrawCustomInfoPage(SpriteBatch spriteBatch, Vector2 mousePosition, ref Action updateAction) => false;

        /// <summary>
        /// Override this method to modify the parameters for a "simple info page" drawing. This will use the default info page draw logic.<br/>
        /// You can return <see langword="null"/> for any of the parameters if you do not want to assign them.<br/>
        /// <br/>
        /// If you do not override this method, default values will be fetched based on localized <c>Mods.ModName.QuestBooks.QuestName.{Title / Contents}</c>.<br/>
        /// You can alternatively override <see cref="LocalizationCategory"/> to change only the localization category ("QuestBooks") and keep other details default.
        /// </summary>
        /// 
        /// <param name="title">The "title" of this quest. Displays in an info page when this quest is clicked in the quest log.<br/>
        /// Quests that do not implement this nor <paramref name="contents"/> will not be clickable in the quest log.</param>
        /// 
        /// <param name="contents">A "description" for this quest. Displays in an info page when this quest is clicked in the quest log.<br/>
        /// Quests that do not implement this nor <paramref name="title"/> will not be clickable in the quest log.</param>
        /// 
        /// <param name="texture">An image to display in an info page when this quest is clicked in the quest log.<br/>
        /// Quests that do not implement this will not draw a texture to their page.<br/>
        /// The texture draws in the upper-righthand corner of the page.</param>
        /// 
        public virtual void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
        {
            title = this.GetLocalization("Title").Format(LocalizationArgs);
            contents = this.GetLocalization("Contents").Format(LocalizationArgs);
            texture = ModContent.RequestIfExists<Texture2D>($"{TextureCategory}/{Name}", out var asset, ReLogic.Content.AssetRequestMode.ImmediateLoad) ? asset.Value : null;
        }

        /// <summary>
        /// Use <see cref="QuestType.World"/> for quests that are saved and managed in the world, and <see cref="QuestType.Player"/> for individual player quests.<br/>
        /// <br/>
        /// Default is <see cref="QuestType.World"/>.
        /// </summary>
        public virtual QuestType QuestType { get => QuestType.World; }

        public virtual string LocalizationCategory { get => "QuestBooks"; }

        /// <summary>
        /// The folder path that will be used to fetch the default texture for use in <see cref="MakeSimpleInfoPage(out string, out string, out Texture2D)"/>.<br/>
        /// If you override <see cref="MakeSimpleInfoPage(out string, out string, out Texture2D)"/>, you can supply any arbitrary texture instead.<br/>
        /// <br/>
        /// Default is <c>{YourModName}/Assets/Texture/QuestBooks</c>
        /// </summary>
        public virtual string TextureCategory { get => $"{Mod.Name}/Assets/Textures/QuestBooks"; }

        /// <summary>
        /// This is called every frame, regardless of whether the quest is completed. You can do any logic updating, dynamic quest updating, etc. here.<br/>
        /// This will be called on both client and server, despite <see cref="CheckCompletion"/> being netmode-dependent; implement your updating accordingly.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// This is called the first time this quest is completed. You can do things like play sounds, spawn items, etc here.<br/>
        /// This can be called on both client and server, so implement your completion accordingly.
        /// </summary>
        public virtual void OnCompletion() { }

        /// <summary>
        /// This is called to actually mark your quest as complete.<br/>
        /// This may be when the world initially loads and quests that were previously completed are being marked, or it may be the first time the quest is marked as completed.<br/>
        /// <br/>
        /// You should *only* modify metadata for your quest or external systems here, not implement completion events.
        /// </summary>
        public virtual void MarkAsComplete() { }

        /// <summary>
        /// Return <see langword="true"/> if this quest should be counted as complete.<br/>
        /// Otherwise return <see langword="false"/>.
        /// </summary>
        public abstract bool CheckCompletion();

        /// <summary>
        /// Called before the QuestDisplay icon is drawn to the canvas.<br/>
        /// Return <see langword="false"/> to prevent the icon from drawing.<br/>
        /// <br/>
        /// Returns <see langword="true"/> by default.
        /// </summary>
        public virtual bool PreTextureDraw(SpriteBatch spriteBatch, Vector2 canvasViewOffset, float zoom, bool unlocked, bool selected, bool hovered) => true;

        /// <summary>
        /// Called after the QuestDisplay icon would be drawn to the canvas, even if <see cref="PreTextureDraw(SpriteBatch, Vector2, float, bool, bool)"/> returned <see langword="false"/>.
        /// </summary>
        public virtual void PostTextureDraw(SpriteBatch spriteBatch, Vector2 canvasViewOffset, float zoom, bool unlocked, bool selected, bool hovered) { }

        /// <summary>
        /// Called if/when a "notification" indicator is requesting to be drawn for this quest.<br/>
        /// Override this to change how the notification should display.
        /// </summary>
        public virtual void DrawNotification(SpriteBatch spriteBatch, Vector2 bottomRight, float zoom, bool unlocked, bool hovered)
        {
            Texture2D texture = QuestAssets.NotificationMark;
            spriteBatch.Draw(texture, bottomRight, null, Color.White, 0f, texture.Size() * 0.5f, zoom, SpriteEffects.None, 0f);
        }

        protected sealed override void Register() => QuestLoader.loadingQuests.Add(GetType(), Key);

        public sealed override void SetupContent() => SetStaticDefaults();

        protected sealed override void InitTemplateInstance() => QuestLoader.QuestMods[GetType()] = Mod;

        public virtual void SaveProgress(TagCompound tag) { }

        public virtual void LoadProgress(TagCompound tag) { }
    }

    public enum QuestType
    {
        World,
        Player
    }
}
