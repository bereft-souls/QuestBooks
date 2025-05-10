using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.QuestLog;
using QuestBooks.QuestLog.DefaultQuestLogStyles;
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
        public static bool UseDesigner { get; set; } = false;
        private static bool useDesignerCache = false;

        public static float LogScale { get; set; } = 1f;
        public static Vector2 LogPositionOffset { get; set; } = Vector2.Zero;

        public static void Toggle(bool? active = null)
        {
            bool display = active ?? !DisplayLog;
            DisplayLog = display;
            ActiveStyle.OnToggle(display);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            useDesignerCache = UseDesigner;

            if (!UseDesigner)
                ActiveStyle.UpdateLog();

            else
                ActiveStyle.UpdateDesigner();
        }

        // This draws the actual quest log to the RenderTarget2D
        public static void DrawQuestLog()
        {
            if (!DisplayLog)
                return;

            var graphics = Main.spriteBatch.GraphicsDevice;

            graphics.SetRenderTarget(ScreenRenderTarget);
            graphics.Clear(Color.Transparent);

            Main.spriteBatch.Begin();

            if (QuestBooks.DesignerEnabled && useDesignerCache)
                ActiveStyle.DrawDesigner(Main.spriteBatch);

            else
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
                    Main.spriteBatch.Draw(ScreenRenderTarget, Main.ScreenSize.ToVector2() / 2f, null, Color.White, 0f, ScreenRenderTarget.Size() / 2f, Main.UIScale, SpriteEffects.None, 0f);
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
            Main.OnResolutionChanged += (_) => SetupRenderTarget();

            // TODO: Change this for config loading
            ActiveStyle = new BasicQuestLogStyle();
        }

        public static void SetupRenderTarget()
        {
            Main.RunOnMainThread(() =>
            {
                ScreenRenderTarget?.Dispose();
                ScreenRenderTarget = new(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            }).GetAwaiter().GetResult();
        }
    }
}
