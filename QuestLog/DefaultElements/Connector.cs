using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace QuestBooks.QuestLog.DefaultElements
{
    [ElementTooltip("Connector")]
    public class Connector : QuestLogElement
    {
        public override float DrawPriority => 0.25f;

        [ElementTooltip("ConnectorThickness")]
        public float LineThickness { get; set; } = 5f;

        public IConnectable Source { get; set; } = null;

        public IConnectable Destination { get; set; } = null;

        public override bool IsHovered(Vector2 mousePosition, Vector2 canvasViewOffset, float zoom, ref string mouseTooltip)
        {
            if (!QuestLogDrawer.ActiveStyle.UseDesigner)
                return false;

            // mousePosition is already in logical canvas coordinates (zoom factored out)
            // so hitbox sizes should remain in logical units
            mousePosition -= canvasViewOffset;
            Vector2 lineAngle = Destination.ConnectorAnchor - Source.ConnectorAnchor;
            Vector2 mouseAngle = lineAngle.RotatedBy(-MathHelper.PiOver2);

            Vector2 intersection = GetPointOfIntersection(Destination.ConnectorAnchor, lineAngle, mousePosition, mouseAngle);
            if ((intersection - mousePosition).Length() > LineThickness)
                return false;

            Rectangle span = CenteredRectangle(Source.ConnectorAnchor + (lineAngle * 0.5f), new(Math.Abs(lineAngle.X), Math.Abs(lineAngle.Y)));
            return span.CreateMargin((int)(LineThickness * 0.5f)).Contains(mousePosition.ToPoint());
        }

        public override bool VisibleOnCanvas() => (Source.ConnectionVisible(Destination) && Destination.CompleteConnection(Source)) || QuestLogDrawer.ActiveStyle.UseDesigner;

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, float zoom, bool selected, bool hovered)
        {
            Color color =
                selected ? Color.Red :
                hovered ? Color.Yellow :
                Source.ConnectionActive(Destination) || QuestLogDrawer.ActiveStyle.UseDesigner ? Color.White : Color.DimGray;

            DrawConnection(spriteBatch, Source.ConnectorAnchor * zoom, Destination.ConnectorAnchor * zoom, color, zoom);
        }

        public override void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition, Vector2 canvasViewOffset, float zoom)
        {
            if (Source is null)
            {
                Vector2 drawPos = (mousePosition - canvasViewOffset) * zoom;
                spriteBatch.Draw(QuestAssets.Connector, drawPos, null, Color.White, 0f, Vector2.Zero, zoom, SpriteEffects.None, 0f);
                return;
            }

            Vector2 source = Source.ConnectorAnchor * zoom;
            Vector2 destination = QuestLogDrawer.ActiveStyle.HoveredElement is IConnectable connection ? connection.ConnectorAnchor * zoom : (mousePosition - canvasViewOffset) * zoom;
            DrawConnection(spriteBatch, source, destination, Color.LightGray, zoom);
        }

        protected void DrawConnection(SpriteBatch spriteBatch, Vector2 source, Vector2 destination, Color color, float zoom = 1f)
        {
            Vector2 line = destination - source;
            Vector2 center = source + (line * 0.5f);

            float width = line.Length();
            float rotation = line.ToRotation();
            float scaledThickness = LineThickness * zoom;

            Texture2D texture = QuestAssets.BigPixel;
            spriteBatch.Draw(texture, center, null, color, rotation, new Vector2(1f), new Vector2(width * 0.5f, scaledThickness * 0.5f), SpriteEffects.None, 0f);

            if (QuestLogDrawer.ActiveStyle.UseDesigner)
            {
                Texture2D arrow = QuestAssets.ConnectorArrow;
                spriteBatch.Draw(arrow, center, null, color, rotation - MathHelper.PiOver4, arrow.Size() * 0.5f, zoom, SpriteEffects.None, 0f);
            }
        }

        public override bool PlaceOnCanvas(QuestChapter chapter, Vector2 mousePosition, Vector2 canvasViewOffset)
        {
            IConnectable connection = QuestLogDrawer.ActiveStyle.HoveredElement as IConnectable;

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

    [ElementTooltip("ConnectorPoint")]
    public class ConnectorPoint : QuestLogElement, IConnectable
    {
        // +0.01 over the Connector element, draws on top of lines
        public override float DrawPriority => 0.26f;

        [ElementTooltip("ConnectorPointSize")]
        public float Size { get; set; } = 5f;

        [ElementTooltip("ConnectorPointFeeds")]
        public int RequiredFeeds { get; set; } = 1;

        public Vector2 CanvasPosition { get; set; }

        public Vector2 ConnectorAnchor => CanvasPosition - QuestLogDrawer.ActiveStyle.QuestAreaOffset;

        public List<Connector> Connections { get; set; } = [];

        public bool CompleteConnection(IConnectable source) => source != this && Connections.Any(x => x.Source == this && x.Destination.CompleteConnection(this));

        public bool ConnectionVisible(IConnectable destination) => (destination != this && Connections.Any(x => x.Destination == this && x.Source.ConnectionVisible(destination))) || QuestLogDrawer.ActiveStyle.UseDesigner;

        public bool ConnectionActive(IConnectable destination) => destination != this && Connections.Count(x => x.Destination == this && x.Source.ConnectionActive(destination)) >= RequiredFeeds;

        public override bool IsHovered(Vector2 mousePosition, Vector2 canvasViewOffset, float zoom, ref string mouseTooltip)
        {
            // mousePosition is already in logical canvas coordinates (zoom factored out)
            return QuestLogDrawer.ActiveStyle.UseDesigner && CenteredRectangle(CanvasPosition, new Vector2(Size)).Contains(mousePosition.ToPoint());
        }

        public override bool VisibleOnCanvas() => QuestLogDrawer.ActiveStyle.UseDesigner || Connections.Any(x => x.VisibleOnCanvas());

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, float zoom, bool selected, bool hovered)
        {
            Color color =
                selected ? Color.Red :
                hovered ? Color.Yellow :
                Connections.Where(x => x.Destination == this).Count(x => x.Source.ConnectionActive(x.Destination)) >= RequiredFeeds || QuestLogDrawer.ActiveStyle.UseDesigner ? Color.White : Color.DimGray;

            Texture2D texture = QuestAssets.ConnectorPoint;
            Vector2 drawPos = (CanvasPosition - canvasViewOffset) * zoom;
            float scale = Size / texture.Width * zoom;
            spriteBatch.Draw(texture, drawPos, null, color, 0f, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
        }

        public override bool PlaceOnCanvas(QuestChapter chapter, Vector2 mousePosition, Vector2 canvasViewOffset)
        {
            CanvasPosition = mousePosition;
            return true;
        }

        public override void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition, Vector2 canvasViewOffset, float zoom)
        {
            Texture2D texture = QuestAssets.ConnectorPoint;
            Vector2 drawPos = (mousePosition - canvasViewOffset) * zoom;
            spriteBatch.Draw(texture, drawPos, null, Color.White with { A = 230 }, 0f, texture.Size() * 0.5f, zoom, SpriteEffects.None, 0f);
        }

        public override void DrawDesignerIcon(SpriteBatch spriteBatch, Rectangle iconArea) => DrawSimpleIcon(spriteBatch, QuestAssets.ConnectorPoint, iconArea);

        public override void OnDelete() => this.DeleteConnections();
    }

    public interface IConnectable
    {
        [JsonIgnore]
        public Vector2 ConnectorAnchor { get; }

        public List<Connector> Connections { get; set; }

        public bool CompleteConnection(IConnectable source);

        public bool ConnectionVisible(IConnectable destination);

        public bool ConnectionActive(IConnectable destination);
    }

    public static class ConnectableExtensions
    {
        public static void DeleteConnections(this IConnectable connectable)
        {
            // Clone the collection to allow modified enumeration
            foreach (var connection in connectable.Connections.ToArray())
            {
                QuestLogDrawer.ActiveStyle.SelectedChapter.Elements.Remove(connection);
                connection.OnDelete();
            }
        }
    }
}
