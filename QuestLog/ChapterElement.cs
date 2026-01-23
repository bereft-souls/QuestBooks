using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.Quests;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog
{
    [ExtendsFromMod("QuestBooks")]
    public abstract class ChapterElement
    {
        [JsonIgnore]
        [HideInDesigner]
        public bool TemplateInstance { get; internal set; } = false;

        [JsonIgnore]
        public virtual bool HasInfoPage { get => false; }

        /// <summary>
        /// Determines the layer this element should draw to.<br/>
        /// <c>0f</c> is closer to the background, <c>1f</c> the foreground.
        /// </summary>
        [JsonIgnore]
        public virtual float DrawPriority { get => 0.5f; }

        #region Common Methods

        public virtual void Update() { }

        public virtual bool IsHovered(Vector2 mousePosition, Vector2 canvasViewOffset, float zoom, ref string mouseTooltip) { return false; }

        public virtual void OnSelect() { }

        public virtual bool VisibleOnCanvas() { return true; }

        public abstract void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, float zoom, bool selected, bool hovered);

        public virtual void DrawInfoPage(SpriteBatch spriteBatch, Vector2 mousePosition, ref Action layerAction)
        {
            Rectangle area = new(10, 0, 420, 540);
            spriteBatch.DrawOutlinedStringInRectangle(area, FontAssets.DeathText.Value, Color.White, Color.Black, "Element does not contain an info page!", clipBounds: false);
        }

        #endregion

        #region Designer Methods

        public abstract bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition, Vector2 canvasViewOffset);

        public virtual void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition, Vector2 canvasViewOffset, float zoom)
        {
            Texture2D texture = QuestAssets.MissingIcon;
            Vector2 drawPos = (mousePosition - canvasViewOffset) * zoom;
            spriteBatch.Draw(texture, drawPos, null, Color.White with { A = 180 }, 0f, texture.Size() * 0.5f, zoom, SpriteEffects.None, 0f);
        }

        public virtual void DrawDesignerIcon(SpriteBatch spriteBatch, Rectangle iconArea) => DrawSimpleIcon(spriteBatch, QuestAssets.MissingIcon, iconArea);

        protected static void DrawSimpleIcon(SpriteBatch spriteBatch, Texture2D texture, Rectangle iconArea)
        {
            float scale = MathHelper.Min((float)iconArea.Width / texture.Width, (float)iconArea.Height / texture.Height);
            spriteBatch.Draw(texture, iconArea.Center.ToVector2(), null, Color.White, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

        public virtual void OnDelete() { }

        #endregion

        #region Designer Attributes

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
        public sealed class HideInDesignerAttribute : Attribute;

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
        public sealed class UseConverterAttribute(Type propertyConverterType) : Attribute
        {
            public Type PropertyConverterType { get; init; } = propertyConverterType;
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
        private sealed class NonDefaultAttribute : Attribute;

        /// <summary>
        /// Intended only for QuestBooks internal use. Simplifies the tooltip attribute to only need the last part of the key.
        /// </summary>
        internal sealed class ElementTooltipAttribute(string localizationKey) : TooltipAttribute($"Mods.QuestBooks.Tooltips.Elements.{localizationKey}");

        #endregion

        #region Converter Implementation

        public static readonly FrozenDictionary<Type, Type> DefaultConverters =
            typeof(ChapterElement)
            .GetNestedTypes()
            .Where(t => t.GetInterfaces().Length != 0)
            .Where(t => Attribute.GetCustomAttribute(t, typeof(NonDefaultAttribute)) is null)
            .Select(t =>
            {
                Type conversionType = t.GetInterfaces()[0].GetGenericArguments()[0];
                return new KeyValuePair<Type, Type>(conversionType, t);
            })
            .ToFrozenDictionary();

        public interface IMemberConverter<T>
        {
            public bool TryParse(string input, out T result);
            public string Convert(T input);
        }

        #endregion

        #region Default Converters

        public class StringConverter : IMemberConverter<string>
        {
            public string Convert(string input) => input;
            public bool TryParse(string input, out string result)
            {
                result = input;
                return true;
            }
        }

        public class IntConverter : IMemberConverter<int>
        {
            public string Convert(int input) => input.ToString();
            public bool TryParse(string input, out int result) => int.TryParse(input, out result);
        }

        public class FloatConverter : IMemberConverter<float>
        {
            public string Convert(float input) => input.ToString();
            public bool TryParse(string input, out float result) => float.TryParse(input, out result);
        }

        public class BoolConverter : IMemberConverter<bool>
        {
            public string Convert(bool input) => input.ToString();
            public bool TryParse(string input, out bool result) => bool.TryParse(input, out result);
        }

        [NonDefault]
        public class AngleConverter : IMemberConverter<float>
        {
            public string Convert(float input) => MathHelper.ToDegrees(input).ToString();
            public bool TryParse(string input, out float result)
            {
                if (!float.TryParse(input, out result))
                    return false;

                result = MathHelper.ToRadians(result);
                return true;
            }
        }

        [NonDefault]
        public class Vector2Converter : IMemberConverter<Vector2>
        {
            public string Convert(Vector2 input) => $"{input.X},{input.Y}";
            public bool TryParse(string input, out Vector2 result)
            {
                string[] xy = input.Split(',');
                if (xy.Length != 2)
                {
                    result = default;
                    return false;
                }

                if (!float.TryParse(xy[0], out float x) || !float.TryParse(xy[1], out float y))
                {
                    result = default;
                    return false;
                }

                result = new(x, y);
                return true;
            }
        }

        #endregion
    }

    public abstract class QuestElement : ChapterElement
    {
        [JsonIgnore]
        public abstract Quest Quest { get; }
    }
}
