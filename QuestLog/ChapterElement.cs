using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using QuestBooks.Quests;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog
{
    [ExtendsFromMod("QuestBooks")]
    public abstract class ChapterElement
    {
        public bool TemplateInstance = false;

        public virtual bool HasInfoPage { get => false; }

        /// <summary>
        /// Determines the layer this element should draw to.<br/>
        /// <c>0f</c> is closer to the background, <c>1f</c> the foreground.
        /// </summary>
        public virtual float DrawPriority { get => 0.5f; }

        public virtual void Update() { }

        public abstract bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition);

        public abstract void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered);

        public virtual void DrawInfoPage(SpriteBatch spriteBatch)
        {
            Rectangle area = new(10, 0, 420, 540);
            spriteBatch.DrawOutlinedStringInRectangle(area, FontAssets.DeathText.Value, Color.White, Color.Black, "Element does not contain an info page!", clipBounds: false);
        }

        public virtual bool IsHovered(Vector2 mousePosition) { return false; }

        public virtual void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition)
        {
            Texture2D texture = QuestAssets.MissingIcon;
            spriteBatch.Draw(texture, mousePosition, null, Color.White with { A = 180 }, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public virtual void DrawDesignerIcon(SpriteBatch spriteBatch, Rectangle iconArea)
        {
            Texture2D texture = QuestAssets.MissingIcon;
            float scale = MathHelper.Min((float)iconArea.Width / texture.Width, (float)iconArea.Height / texture.Height);
            spriteBatch.Draw(texture, iconArea.Center.ToVector2(), null, Color.White, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
        public sealed class HideInDesignerAttribute : Attribute
        {

        }

        public class MemberInfoComparer : IEqualityComparer<MemberInfo>
        {
            public static MemberInfoComparer Instance { get; } = new();

            public bool Equals(MemberInfo x, MemberInfo y)
            {
                return x.Equals(y);
            }

            public int GetHashCode([DisallowNull] MemberInfo obj)
            {
                return obj.GetHashCode();
            }
        }
    }

    public abstract class QuestElement : ChapterElement
    {
        public abstract Quest Quest { get; }
    }
}
