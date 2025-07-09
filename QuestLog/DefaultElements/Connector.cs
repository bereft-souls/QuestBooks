using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultLogStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.QuestLog.DefaultElements
{
    public class Connector : ChapterElement
    {
        public override float DrawPriority => 0.25f;

        public float LineThickness { get; set; } = 8f;

        public IConnectable Source { get; set; } = null;

        public IConnectable Destination { get; set; } = null;

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered)
        {
            Color color =
                selected ? Color.Red :
                hovered ? Color.Yellow :
                Source.ConnectionActive() ? Color.White : Color.Gray;

            DrawConnection(spriteBatch, Source.ConnectorAnchor, Destination.ConnectorAnchor, color);
        }

        public override bool IsHovered(Vector2 mousePosition)
        {
            if (!BasicQuestLogStyle.UseDesigner)
                return false;

            Vector2 lineAngle = Destination.ConnectorAnchor - Source.ConnectorAnchor;
            Vector2 mouseAngle = lineAngle.RotatedBy(-MathHelper.PiOver2);

            Vector2 intersection = GetPointOfIntersection(Destination.ConnectorAnchor, lineAngle, mousePosition, mouseAngle);
            if ((intersection - mousePosition).Length() > LineThickness)
                return false;

            Rectangle span = CenteredRectangle(Source.ConnectorAnchor + (lineAngle * 0.5f), new(Math.Abs(lineAngle.X), Math.Abs(lineAngle.Y)));
            return span.Contains(mousePosition.ToPoint());
        }

        public override bool VisibleOnCanvas() => Source.ConnectionVisible();

        public override void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition)
        {
            if (Source is null)
            {
                spriteBatch.Draw(QuestAssets.Connector, mousePosition, Color.White);
                return;
            }

            Vector2 source = Source.ConnectorAnchor;
            Vector2 destination = BasicQuestLogStyle.HoveredElement is IConnectable connection ? connection.ConnectorAnchor : mousePosition;
            DrawConnection(spriteBatch, source, destination, Color.LightGray);
        }

        protected void DrawConnection(SpriteBatch spriteBatch, Vector2 source, Vector2 destination, Color color)
        {
            Vector2 line = destination - source;
            Vector2 center = source + (line * 0.5f);

            float width = line.Length();
            float rotation = line.ToRotation();

            Texture2D texture = QuestAssets.BigPixel;
            spriteBatch.Draw(texture, center, null, color, rotation, new Vector2(1f), new Vector2(width * 0.5f, LineThickness * 0.5f), SpriteEffects.None, 0f);
        }

        public override bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition)
        {
            IConnectable connection = BasicQuestLogStyle.HoveredElement as IConnectable;

            if (Source is null || Source == connection)
            {
                if (connection is not null)
                    Source = connection;

                return false;
            }

            if (connection is not null)
            { 
                Destination = connection;

                Source.Connections.Add(this);
                Destination.Connections.Add(this);

                return true;
            }

            return false;
        }

        public override void OnDelete()
        {
            Source.Connections.Remove(this);
            Destination.Connections.Remove(this);
        }

        public override void DrawDesignerIcon(SpriteBatch spriteBatch, Rectangle iconArea) => DrawSimpleIcon(spriteBatch, QuestAssets.Connector, iconArea);
    }

    public class ConnectorPoint : ChapterElement, IConnectable
    {
        // +0.01 over the Connector element, draws on top of lines
        public override float DrawPriority => 0.26f;

        public float Size { get; set; } = 8f;

        public Vector2 CanvasPosition { get; set; }

        public Vector2 ConnectorAnchor => CanvasPosition - BasicQuestLogStyle.QuestAreaOffset;

        public List<Connector> Connections { get; set; } = [];

        public bool ConnectionVisible() => Connections.Any(x => x.Destination == this && x.Source.ConnectionVisible());

        public bool ConnectionActive() => Connections.Any(x => x.Destination == this && x.Source.ConnectionActive());

        public override bool IsHovered(Vector2 mousePosition)
        {
            return BasicQuestLogStyle.UseDesigner && CenteredRectangle(CanvasPosition, new Vector2(Size)).Contains(mousePosition.ToPoint());
        }

        public override bool VisibleOnCanvas() => ConnectionVisible();

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered)
        {
            Color color =
                selected ? Color.Red :
                hovered ? Color.Yellow :
                ConnectionActive() ? Color.White : Color.Gray;

            spriteBatch.End();
            spriteBatch.GetDrawParameters(out var blend, out var sampler, out var depth, out var raster, out var effect, out var matrix);
            spriteBatch.Begin(SpriteSortMode.Deferred, blend, SamplerState.PointClamp, depth, raster, effect, matrix);

            Texture2D texture = QuestAssets.ConnectorPoint;
            spriteBatch.Draw(texture, CanvasPosition - canvasViewOffset, null, color, 0f, texture.Size() * 0.5f, Size / texture.Width, SpriteEffects.None, 0f);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, blend, sampler, depth, raster, effect, matrix);
        }

        public override bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition)
        {
            CanvasPosition = mousePosition;
            return true;
        }

        public override void DrawDesignerIcon(SpriteBatch spriteBatch, Rectangle iconArea) => DrawSimpleIcon(spriteBatch, QuestAssets.ConnectorPoint, iconArea);

        public override void OnDelete()
        {
            foreach (var connection in Connections)
            {
                BasicQuestLogStyle.SelectedChapter.Elements.Remove(connection);
                connection.OnDelete();
            }
        }
    }

    public interface IConnectable
    {
        [JsonIgnore]
        public Vector2 ConnectorAnchor { get; }

        public List<Connector> Connections { get; set; }

        public bool ConnectionVisible();

        public bool ConnectionActive();
    }
}
