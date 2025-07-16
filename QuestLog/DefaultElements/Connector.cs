using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultLogStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace QuestBooks.QuestLog.DefaultElements
{
    [ElementTooltip("Connector")]
    public class Connector : ChapterElement
    {
        public override float DrawPriority => 0.25f;

        [ElementTooltip("ConnectorThickness")]
        public float LineThickness { get; set; } = 6f;

        public IConnectable Source { get; set; } = null;

        public IConnectable Destination { get; set; } = null;

        public override bool IsHovered(Vector2 mousePosition, ref string mouseTooltip)
        {
            if (!BasicQuestLogStyle.UseDesigner)
                return false;

            mousePosition -= BasicQuestLogStyle.QuestAreaOffset;
            Vector2 lineAngle = Destination.ConnectorAnchor - Source.ConnectorAnchor;
            Vector2 mouseAngle = lineAngle.RotatedBy(-MathHelper.PiOver2);

            Vector2 intersection = GetPointOfIntersection(Destination.ConnectorAnchor, lineAngle, mousePosition, mouseAngle);
            if ((intersection - mousePosition).Length() > LineThickness)
                return false;

            Rectangle span = CenteredRectangle(Source.ConnectorAnchor + (lineAngle * 0.5f), new(Math.Abs(lineAngle.X), Math.Abs(lineAngle.Y)));
            return span.CreateMargin((int)(LineThickness * 0.5f)).Contains(mousePosition.ToPoint());
        }

        public override bool VisibleOnCanvas() => (Source.ConnectionVisible(Destination) && Destination.CompleteConnection(Source)) || BasicQuestLogStyle.UseDesigner;

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered)
        {
            Color color =
                selected ? Color.Red :
                hovered ? Color.Yellow :
                Source.ConnectionActive(Destination) || BasicQuestLogStyle.UseDesigner ? Color.White : Color.DimGray;

            DrawConnection(spriteBatch, Source.ConnectorAnchor, Destination.ConnectorAnchor, color);
        }

        public override void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition, Vector2 canvasViewOffset)
        {
            if (Source is null)
            {
                spriteBatch.Draw(QuestAssets.Connector, mousePosition - canvasViewOffset, Color.White);
                return;
            }

            Vector2 source = Source.ConnectorAnchor;
            Vector2 destination = BasicQuestLogStyle.HoveredElement is IConnectable connection ? connection.ConnectorAnchor : mousePosition - canvasViewOffset;
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

    [ElementTooltip("ConnectorPoint")]
    public class ConnectorPoint : ChapterElement, IConnectable
    {
        // +0.01 over the Connector element, draws on top of lines
        public override float DrawPriority => 0.26f;

        [ElementTooltip("ConnectorPointSize")]
        public float Size { get; set; } = 6f;

        [ElementTooltip("ConnectorPointFeeds")]
        public int RequiredFeeds { get; set; } = 1;

        public Vector2 CanvasPosition { get; set; }

        public Vector2 ConnectorAnchor => CanvasPosition - BasicQuestLogStyle.QuestAreaOffset;

        public List<Connector> Connections { get; set; } = [];

        public bool CompleteConnection(IConnectable source) => source != this && Connections.Any(x => x.Source == this && x.Destination.CompleteConnection(this));

        public bool ConnectionVisible(IConnectable destination) => (destination != this && Connections.Any(x => x.Destination == this && x.Source.ConnectionVisible(destination))) || BasicQuestLogStyle.UseDesigner;

        public bool ConnectionActive(IConnectable destination) => destination != this && Connections.Count(x => x.Destination == this && x.Source.ConnectionActive(destination)) >= RequiredFeeds;

        public override bool IsHovered(Vector2 mousePosition, ref string mouseTooltip)
        {
            return BasicQuestLogStyle.UseDesigner && CenteredRectangle(CanvasPosition, new Vector2(Size)).Contains(mousePosition.ToPoint());
        }

        public override bool VisibleOnCanvas() => BasicQuestLogStyle.UseDesigner || Connections.Any(x => x.VisibleOnCanvas());

        public override void DrawToCanvas(SpriteBatch spriteBatch, Vector2 canvasViewOffset, bool selected, bool hovered)
        {
            Color color =
                selected ? Color.Red :
                hovered ? Color.Yellow :
                Connections.Where(x => x.Destination == this).All(x => x.Source.ConnectionActive(x.Destination)) || BasicQuestLogStyle.UseDesigner ? Color.White : Color.DimGray;

            Texture2D texture = QuestAssets.ConnectorPoint;
            spriteBatch.Draw(texture, CanvasPosition - canvasViewOffset, null, color, 0f, texture.Size() * 0.5f, Size / texture.Width, SpriteEffects.None, 0f);
        }

        public override bool PlaceOnCanvas(BookChapter chapter, Vector2 mousePosition)
        {
            CanvasPosition = mousePosition;
            return true;
        }

        public override void DrawPlacementPreview(SpriteBatch spriteBatch, Vector2 mousePosition, Vector2 canvasViewOffset)
        {
            Texture2D texture = QuestAssets.ConnectorPoint;
            spriteBatch.Draw(texture, mousePosition - canvasViewOffset, null, Color.White with { A = 230 }, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
        }

        public override void DrawDesignerIcon(SpriteBatch spriteBatch, Rectangle iconArea) => DrawSimpleIcon(spriteBatch, QuestAssets.ConnectorPoint, iconArea);

        public override void OnDelete()
        {
            // Clone the collection to allow modified enumeration
            foreach (var connection in Connections.ToArray())
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

        public bool CompleteConnection(IConnectable source);

        public bool ConnectionVisible(IConnectable destination);

        public bool ConnectionActive(IConnectable destination);
    }
}
