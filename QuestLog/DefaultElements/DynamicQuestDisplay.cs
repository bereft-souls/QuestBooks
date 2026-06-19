using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Quests;
using QuestBooks.Quests.VanillaQuests;
using QuestBooks.Systems;
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
        public DynamicQuestDisplay() => QuestKey = new PlaceholderDynamic().Key;

        public override Quest Quest
        {
            get
            {
                var quest = base.Quest;
                if (quest is Placeholder)
                    return QuestManager.GetQuest<PlaceholderDynamic>();
                return quest;
            }
        }

        [JsonIgnore]
        public DynamicQuest DynamicQuest { get => Quest as DynamicQuest; }

        protected override void DrawCompleted(SpriteBatch spriteBatch, Vector2 canvasOffset, float zoom, bool hovered, bool selected) =>
            DynamicQuest.DrawCompleted(spriteBatch, CanvasPosition, canvasOffset, zoom, hovered, selected);

        protected override void DrawLocked(SpriteBatch spriteBatch, Vector2 canvasOffset, float zoom, bool hovered, bool selected) =>
            DynamicQuest.DrawLocked(spriteBatch, CanvasPosition, canvasOffset, zoom, hovered, selected);

        protected override void DrawIncomplete(SpriteBatch spriteBatch, Vector2 canvasOffset, float zoom, bool hovered, bool selected) =>
            DynamicQuest.DrawIncomplete(spriteBatch, CanvasPosition, canvasOffset, zoom, hovered, selected);

        public override bool IsHovered(Vector2 mousePosition, Vector2 canvasViewOffset, float zoom, ref string mouseTooltip)
        {
            // mousePosition is already in logical canvas coordinates (zoom factored out)
            bool unlocked = HasInfoPage && (Unlocked() || QuestLogDrawer.ActiveStyle.UseDesigner);
            bool hovered = CenteredRectangle(CanvasPosition, DynamicQuest.HoverAreaSize(unlocked)).Contains(mousePosition.ToPoint());

            string tooltip = unlocked ? Quest.HoverTooltip : Quest.LockedTooltip;
            if (hovered && tooltip != null)
                mouseTooltip = tooltip;

            return hovered && unlocked;
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
