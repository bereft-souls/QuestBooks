using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.QuestLog;
using QuestBooks.QuestLog.DefaultQuestLogStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
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

            Main.spriteBatch.Begin(SpriteSortMode.Deferred,
                QuestManager.ActiveStyle.CustomBlendState,
                QuestManager.ActiveStyle.CustomSamplerState,
                QuestManager.ActiveStyle.CustomDepthStencilState,
                QuestManager.ActiveStyle.CustomRasterizerState);

            QuestManager.ActiveStyle.DrawLog(Main.spriteBatch);
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
                    Main.spriteBatch.Draw(ScreenRenderTarget, Main.ScreenSize.ToVector2() * 0.5f, null, Color.White, 0f, ScreenRenderTarget.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
                    return true;
                },
                InterfaceScaleType.None
            ));
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
