using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuestBooks.Systems
{
    internal class QuestLogDrawer : ModSystem
    {
        public static RenderTarget2D ScreenRenderTarget { get; private set; }
        public static Vector2 RealScreenSize => ScreenRenderTarget.Size();
        public static bool DisplayLog { get; private set; } = false;

        public static void Toggle(bool? active = null)
        {
            bool display = active ?? !DisplayLog;
            DisplayLog = display;

            // If the inventory is currently open, close it.
            // We can't guarantee the log won't overlap with the inventory,
            // so the easiest way to reduce mouse overlap conflict is just close it.
            if (DisplayLog && Main.playerInventory)
                Main.playerInventory = false;

            QuestManager.ActiveStyle.OnToggle(display);
        }

        // This draws the actual quest log to the RenderTarget2D
        public static void DrawQuestLog()
        {
            if (!DisplayLog)
                return;

            var graphics = Main.spriteBatch.GraphicsDevice;
            graphics.SetRenderTarget(ScreenRenderTarget);
            graphics.Clear(Color.Transparent);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, new()
            {
                ColorSourceBlend = Blend.SourceAlpha,
                AlphaSourceBlend = Blend.One,
                ColorDestinationBlend = Blend.InverseSourceAlpha,
                AlphaDestinationBlend = Blend.One,
                ColorBlendFunction = BlendFunction.Add,
                AlphaBlendFunction = BlendFunction.Add
            });

            QuestManager.ActiveStyle.DrawLog(Main.spriteBatch);
            Main.spriteBatch.End();

            graphics.SetRenderTargets(null);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextLayer != -1 && Main.playerInventory)
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
                            QuestManager.ActiveStyle.DrawQuestLogIcon(Main.spriteBatch, iconArea, hovered);
                            return true;
                        },
                        InterfaceScaleType.UI
                    ));
            }

            if (!DisplayLog)
                return;

            if (mouseTextLayer != -1)
            {
                layers.Insert(mouseTextLayer, new LegacyGameInterfaceLayer(
                    "QuestBooks: Quest Log", () =>
                    {
                        Main.spriteBatch.Draw(ScreenRenderTarget, Main.ScreenSize.ToVector2() * 0.5f, null, Color.White, 0f, ScreenRenderTarget.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                        return true;
                    },
                    InterfaceScaleType.None
                ));
            }

            QuestManager.ActiveStyle.ModifyInterfaceLayers(layers);
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
