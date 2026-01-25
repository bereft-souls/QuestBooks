using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.QuestLog;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuestBooks.Systems
{
    public class QuestLogDrawer : ModSystem
    {
        public static RenderTarget2D ScreenRenderTarget { get; private set; }
        public static BlendState BlendState { get; } = new()
        {
            ColorSourceBlend = Blend.SourceAlpha,
            AlphaSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            AlphaDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.Add,
            AlphaBlendFunction = BlendFunction.Add
        };

        public static bool DisplayLog { get; private set; } = false;
        public static Vector2 RealScreenSize => ScreenRenderTarget.Size();

        public static Dictionary<string, QuestLogStyle> QuestLogStyles { get; internal set; } = null;
        public static QuestLogStyle ActiveStyle { get; internal set; } = null;

        public static Dictionary<string, QuestBooksMod.CoverDrawDelegate> CoverDrawCalls { get; } = [];
        public static Dictionary<string, QuestBooksMod.LogTitleRetrievalDelegate> LogTitleRetrievalCalls { get; } = [];
        public static Dictionary<string, QuestBooksMod.LogTitleDrawDelegate> LogTitleDrawCalls { get; } = [];

        public static Vector2 QuestLogDrawOffset { get; set; } = Vector2.Zero;
        public static float QuestLogDrawOpacity { get; set; } = 1f;

        public static void Toggle(bool? active = null)
        {
            bool display = active ?? !DisplayLog;
            DisplayLog = display;

            // If the inventory is currently open, close it.
            // We can't guarantee the log won't overlap with the inventory,
            // so the easiest way to reduce mouse overlap conflict is just close it.
            if (DisplayLog && Main.playerInventory)
                Main.playerInventory = false;

            ActiveStyle.OnToggle(display);
        }

        public static void SelectLogStyle(string style) => SelectLogStyle(QuestLogStyles[style]);

        public static void SelectLogStyle(QuestLogStyle style)
        {
            ActiveStyle?.OnDeselect();
            ActiveStyle = style;
            style.OnSelect();
        }

        // This draws the actual quest log to the RenderTarget2D
        private static bool targetCleared = false;
        public static void DrawQuestLog()
        {
            if (!DisplayLog)
            {
                if (targetCleared)
                    return;

                var graphicsDevice = Main.spriteBatch.GraphicsDevice;
                var oldTargets = graphicsDevice.GetRenderTargets();

                graphicsDevice.SetRenderTarget(ScreenRenderTarget);
                graphicsDevice.Clear(Color.Transparent);
                graphicsDevice.SetRenderTargets(oldTargets);

                targetCleared = true;
                return;
            }

            var graphics = Main.spriteBatch.GraphicsDevice;
            var targets = graphics.GetRenderTargets();

            graphics.SetRenderTarget(ScreenRenderTarget);
            graphics.Clear(Color.Transparent);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState);
            ActiveStyle.DrawLog(Main.spriteBatch);
            Main.spriteBatch.End();

            graphics.SetRenderTargets(targets);
            targetCleared = false;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextLayer == -1)
                return;

            if (Main.playerInventory)
            {
                Point achievementPositionInInventory = new Vector2(516f, 30f).ToPoint();
                Rectangle achievement = new(achievementPositionInInventory.X, achievementPositionInInventory.Y, 48, 48);

                Rectangle iconArea = achievement.CookieCutter(new(2f, 0f), new(0.7f, 0.7f));
                bool hovered = false;

                if (iconArea.Contains(Main.MouseScreen.ToPoint()))
                {
                    hovered = true;
                    Main.LocalPlayer.mouseInterface = true;
                    Main.instance.MouseTextNoOverride(Language.GetTextValue("Mods.QuestBooks.Tooltips.OpenQuestLog"));
                }

                if (hovered && Main.mouseLeftRelease && Main.mouseLeft)
                    Toggle(true);

                else
                    layers.Insert(mouseTextLayer, new LegacyGameInterfaceLayer(
                        "QuestBooks: Book Prompt", () =>
                        {
                            ActiveStyle.DrawQuestLogIcon(Main.spriteBatch, iconArea, hovered);
                            return true;
                        },
                        InterfaceScaleType.UI
                    ));
            }

            if (!DisplayLog)
                return;

            layers.Insert(mouseTextLayer, new LegacyGameInterfaceLayer(
                "QuestBooks: Quest Log", () =>
                {
                    Main.spriteBatch.Draw(ScreenRenderTarget,
                        Main.ScreenSize.ToVector2() * 0.5f + QuestLogDrawOffset,
                        null,
                        Color.White * QuestLogDrawOpacity,
                        0f,
                        ScreenRenderTarget.Size() * 0.5f,
                        1f,
                        SpriteEffects.None,
                        0f);

                    return true;
                },
                InterfaceScaleType.None
            ));

            ActiveStyle.ModifyInterfaceLayers(layers);
        }

        public override void Load()
        {
            SetupRenderTarget();

            // Prepare render targets.
            Main.OnPreDraw += (_) => DrawQuestLog();
            Main.OnResolutionChanged += _ => SetupRenderTarget();
        }

        public static void SetupRenderTarget()
        {
            static void ResetAction()
            {
                ScreenRenderTarget?.Dispose();
                ScreenRenderTarget = new(
                    Main.graphics.GraphicsDevice,
                    Main.graphics.GraphicsDevice.Viewport.Width,
                    Main.graphics.GraphicsDevice.Viewport.Height,
                    false,
                    SurfaceFormat.Color,
                    DepthFormat.None,
                    0,
                    RenderTargetUsage.PreserveContents);
            }

            if (ThreadCheck.IsMainThread)
                ResetAction();

            else
                Main.RunOnMainThread(ResetAction).GetAwaiter().GetResult();
        }
    }
}
