using Newtonsoft.Json;
using QuestBooks.QuestLog.DefaultQuestLineElements.BaseElements;
using QuestBooks.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;

namespace QuestBooks.QuestLog.DefaultQuestLines
{
    /// <summary>
    /// Represents a basic <see cref="QuestLine"/> implementation, with a chapter name and contained elements.
    /// </summary>
    public class BasicQuestLine : QuestLine
    {
        /// <summary>
        /// The list of all elements contained within this quest line.
        /// </summary>
        [JsonIgnore]
        public override IEnumerable<QuestLineElement> Elements { get => ChapterElements; }

        public List<QuestLineElement> ChapterElements = [];

        [JsonIgnore]
        public virtual string DisplayName { get => Language.GetOrRegister(NameKey).Value; }

        public string NameKey;

        /// <summary>
        /// A collection of all quests contained by elements in this quest line.<br/>
        /// These quests are all the actual implemented instances, and not duplicates or templates - you can access accurate info or methods from them.<br/>
        /// Pulls from <see cref="Elements"/>
        /// </summary>
        [JsonIgnore]
        public IEnumerable<Quest> QuestList => Elements.Where(e => e is QuestElement).Cast<QuestElement>().Select(e => e.Quest);

        /// <summary>
        /// The completion progress of this quest line.<br/>
        /// 0f is no quests completed, and 1f is all quests completed.<br/>
        /// Pulls from <see cref="QuestList"/>.
        /// </summary>
        [JsonIgnore]
        public float Progress => (float)QuestList.Count(q => q.Completed) / Math.Max(1, QuestList.Count());

        /// <summary>
        /// Whether or not all quests in this questline are complete.<br/>
        /// Pulls from <see cref="QuestList"/>
        /// </summary>
        [JsonIgnore]
        public bool Complete => QuestList.All(q => q.Completed);

        /// <summary>
        /// Forwards <see cref="QuestLineElement.Update"/> calls to each element in the quest line.
        /// </summary>
        public override void Update()
        {
            foreach (QuestLineElement questElement in Elements)
                questElement.Update();
        }
    }
}
