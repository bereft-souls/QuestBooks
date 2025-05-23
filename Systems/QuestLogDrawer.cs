using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.QuestLog;
using QuestBooks.QuestLog.DefaultQuestLogStyles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace QuestBooks.Systems
{
    internal class QuestLogDrawer : ModSystem
    {
        public static RenderTarget2D ScreenRenderTarget { get; private set; }
        public static Vector2 RealScreenSize => ScreenRenderTarget.Size();

        public static Dictionary<Mod, List<QuestLogStyle>> LogStyleRegistry = [];
        public static Dictionary<string, QuestLogStyle> QuestLogStyles = null;

        public static QuestLogStyle ActiveStyle = null;
        public static bool DisplayLog { get; private set; } = false;
        
        public static void Toggle(bool? active = null)
        {
            bool display = active ?? !DisplayLog;
            DisplayLog = display;
            ActiveStyle.OnToggle(display);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            ActiveStyle.UpdateLog();
        }

        // This draws the actual quest log to the RenderTarget2D
        public static void DrawQuestLog()
        {
            if (!DisplayLog)
                return;

            var graphics = Main.spriteBatch.GraphicsDevice;
            graphics.SetRenderTarget(ScreenRenderTarget);
            graphics.Clear(Color.Transparent);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, ActiveStyle.CustomBlendState, ActiveStyle.CustomSamplerState, ActiveStyle.CustomDepthStencilState, ActiveStyle.CustomRasterizerState);
            ActiveStyle.DrawLog(Main.spriteBatch);
            Main.spriteBatch.End();

            graphics.SetRenderTargets(null);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (!DisplayLog)
                return;

            int mouseTextLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextLayer == -1)
                return;

            layers.Insert(mouseTextLayer, new LegacyGameInterfaceLayer(
                "QuestBooks: Quest Log", () =>
                {
                    Main.spriteBatch.Draw(ScreenRenderTarget, Main.ScreenSize.ToVector2() / 2f, null, Color.White, 0f, ScreenRenderTarget.Size() / 2f, 1f, SpriteEffects.None, 0f);
                    return true;
                },
                InterfaceScaleType.None
            ));
        }

        public override void Load()
        {
            SetupRenderTarget(new(Main.screenWidth, Main.screenHeight));

            // Prepare render targets.
            Main.OnPreDraw += (_) => DrawQuestLog();
            Main.OnResolutionChanged += (newSize) => SetupRenderTarget(newSize.ToPoint());
        }

        public override void PostSetupContent()
        {
            // TODO: Change this for config loading
            ActiveStyle = new BasicQuestLogStyle();
            ActiveStyle.OnSelect();
        }

        public static void SetupRenderTarget(Point screenSize)
        {
            void ResetAction()
            {
                ScreenRenderTarget?.Dispose();
                ScreenRenderTarget = new(
                    Main.graphics.GraphicsDevice,
                    screenSize.X,
                    screenSize.Y,
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
