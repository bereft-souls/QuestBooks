using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace QuestBooks.QuestLog
{
    public abstract class QuestLogStyle
    {
        public virtual SamplerState CustomSamplerState { get; } = SamplerState.PointClamp;
        public virtual DepthStencilState CustomDepthStencilState { get; } = DepthStencilState.Default;
        public virtual RasterizerState CustomRasterizerState { get; } = RasterizerState.CullNone;
        public virtual BlendState CustomBlendState { get; } = new ()
        {
            ColorSourceBlend = Blend.SourceAlpha,
            AlphaSourceBlend = Blend.SourceAlpha,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            AlphaDestinationBlend = Blend.DestinationColor,
            AlphaBlendFunction = BlendFunction.Max
        };

        public abstract string Key { get; }
        public abstract string DisplayName { get; }

        public virtual void OnSelect() { }
        public virtual void OnDeselect() { }

        public virtual void OnToggle(bool active) { }

        public virtual void UpdateLog() { }
        public abstract void DrawLog(SpriteBatch spriteBatch);
        public virtual void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) { }

        public virtual void SavePlayerData(TagCompound tag) { }
        public virtual void SaveWorldData(TagCompound tag) { }
        public virtual void LoadPlayerData(TagCompound tag) { }
        public virtual void LoadWorldData(TagCompound tag) { }
    }
}
