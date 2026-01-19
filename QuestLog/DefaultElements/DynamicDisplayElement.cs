using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using QuestBooks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestBooks.QuestLog.DefaultElements
{
    public class DynamicDisplayElement : DisplayElement, IConnectable
    {
        [ElementTooltip("ShowConnections")]
        public virtual bool ShowConnections { get; set; } = false;

        [ElementTooltip("DisplayPrerequisites")]
        public virtual int RequiredFeeds { get; set; } = 1;

        [JsonIgnore]
        public int IncomingFeeds => Connections.Count(x => x.Destination == this && x.Source.ConnectionActive(this));

        public virtual Vector2 ConnectorAnchor => CanvasPosition - QuestManager.ActiveStyle.QuestAreaOffset;

        public List<Connector> Connections { get; set; } = [];

        public bool CompleteConnection(IConnectable source) => ShowConnections;

        public bool ConnectionActive(IConnectable destination) => IncomingFeeds >= RequiredFeeds;

        public bool ConnectionVisible(IConnectable destination) => ShowConnections;
    }
}
