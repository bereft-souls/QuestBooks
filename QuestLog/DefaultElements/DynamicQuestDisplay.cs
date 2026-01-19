using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Quests;
using QuestBooks.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QuestBooks.QuestLog.DefaultElements
{
    [ElementTooltip("DynamicQuestDisplay")]
    public class DynamicQuestDisplay : QuestDisplay
    {
        [HideInDesigner]
        public override string Texture { get => base.Texture; set => base.Texture = value; }

        [HideInDesigner]
        public override string OutlineTexture { get => base.OutlineTexture; set => base.OutlineTexture = value; }

        [HideInDesigner]
        public override string IncompleteTexture { get => base.IncompleteTexture; set => base.IncompleteTexture = value; }

        [HideInDesigner]
        public override string LockedTexture { get => base.LockedTexture; set => base.LockedTexture = value; }

        [UseConverter(typeof(DynamicQuestChecker))]
        public override string QuestKey { get => base.QuestKey; set => base.QuestKey = value; }

        [JsonIgnore]
        public DynamicQuest DynamicQuest { get => base.Quest as DynamicQuest; }

        public override void Update()
        {
            if (DynamicQuest is null)
                return;

            _completedTexture = DynamicQuest.Texture;
            _outlineTexture = DynamicQuest.OutlineTexture;
            _incompleteTexture = DynamicQuest.IncompleteTexture;
            _lockedTexture = DynamicQuest.LockedTexture;
        }

        public class DynamicQuestChecker : IMemberConverter<string>
        {
            public string Convert(string input) => input;
            public bool TryParse(string input, out string result)
            {
                result = input;
                return QuestManager.TryGetQuest(input, out var quest) && quest is DynamicQuest;
            }
        }
    }
}
