using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;

namespace QuestBooks.QuestLog.DefaultLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private static bool previewElementInfo = false;
        private static readonly object[] defaultMembers = typeof(ChapterElement)
            .GetProperties().Cast<MemberInfo>().Concat(typeof(ChapterElement).GetFields().Cast<MemberInfo>())
            .Select<MemberInfo, object>(x => x is FieldInfo field ? field.FieldHandle : x).ToArray();

        private void HandleElementProperties(Rectangle area, Vector2 mousePosition)
        {
            Point mouseCanvas = mousePosition.ToPoint();

            if (previewElementInfo)
            {
                DrawTasks.Add(SelectedElement.DrawInfoPage);
                goto DrawPreviewButton;
            }

            Rectangle elementProperties = area.CookieCutter(new(0f, 0.1f), new(0.95f, 0.85f));
            Rectangle propertyTitle = elementProperties.CookieCutter(new(-0.13f, -1.06f), new(0.85f, 0.1f));

            AddRectangle(elementProperties, Color.Red);
            DrawTasks.Add(sb => sb.DrawOutlinedStringInRectangle(propertyTitle, FontAssets.DeathText.Value, Color.White, Color.Black, "Element Properties:", clipBounds: false));

            var elementType = SelectedElement.GetType();
            var ignoredAttribute = typeof(ChapterElement.HideInDesignerAttribute);

            var properties = elementType.GetProperties();
            var fields = elementType.GetFields();

            IEnumerable<MemberInfo> members = properties.Where(x => x.CanWrite).Cast<MemberInfo>().Concat(fields.Cast<MemberInfo>())
                .Where(x => !(x is FieldInfo field ? defaultMembers.Contains(field.FieldHandle) : defaultMembers.Contains(x)))
                .Where(x => Attribute.GetCustomAttribute(x, ignoredAttribute) is null);

        DrawPreviewButton:

            Rectangle previewToggle = area.CookieCutter(new(0.85f, -0.85f), new(0.125f, 0.075f));
            AddRectangle(previewToggle, Color.Blue, fill: true);

            if (previewElementInfo)
                AddRectangle(previewToggle, Color.White);

            if (previewToggle.Contains(mouseCanvas))
            {
                MouseTooltip = "Toggle Preview";

                if (LeftMouseJustReleased)
                    previewElementInfo = !previewElementInfo;
            }
        }
    }
}
