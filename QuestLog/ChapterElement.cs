using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.Quests;
using ReLogic.Content;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
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

        public virtual bool IsHovered(Vector2 mousePosition) { return false; }

        public virtual bool VisibleOnCanvas() { return true; }

        public abstract void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered);

        public virtual void DrawInfoPage(SpriteBatch spriteBatch)
        {
            Rectangle area = new(10, 0, 420, 540);
            spriteBatch.DrawOutlinedStringInRectangle(area, FontAssets.DeathText.Value, Color.White, Color.Black, "Element does not contain an info page!", clipBounds: false);
        }

        #endregion

        #region Designer Methods

        public abstract bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition);

        public virtual void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition)
        {
            Texture2D texture = QuestAssets.MissingIcon;
            spriteBatch.Draw(texture, mousePosition, null, Color.White with { A = 180 }, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
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
        public sealed class HideInDesignerAttribute : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
        public sealed class UseConverterAttribute(Type propertyConverterType) : Attribute
        {
            public Type PropertyConverterType { get; init; } = propertyConverterType;
        }

        #endregion

        #region Converter Implementation

        public static readonly FrozenDictionary<Type, Type> DefaultConverters =
            typeof(ChapterElement)
            .GetNestedTypes()
            .Where(t => t.GetInterfaces().Length != 0)
            .Select(t => {
                Type conversionType = t.GetInterfaces()[0].GetGenericArguments()[0];
                return new KeyValuePair<Type, Type>(conversionType, t);
            }).ToFrozenDictionary();

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

        #endregion
    }

    public abstract class QuestElement : ChapterElement
    {
        public abstract Quest Quest { get; }
    }
}
