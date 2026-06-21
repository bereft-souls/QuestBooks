using QuestBooks.Systems;
using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class KillMarriage : QBQuest
{
    public override bool CheckCompletion() => false;

    public class KillMarriageCheck : GlobalNPC
    {
        public bool Groom { get; private set; }

        public bool Bride { get; private set; }

        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.TheGroom || entity.type == NPCID.TheBride;

        public override void OnKill(NPC npc)
        {
            Groom |= npc.type == NPCID.TheGroom;
            Bride |= npc.type == NPCID.TheBride;

            if (!Groom || !Bride)
                return;

            QuestManager.MarkComplete<KillMarriage>();
        }

        public override void SaveData(NPC npc, TagCompound tag)
        {
            tag[nameof(Groom)] = Groom;
            tag[nameof(Bride)] = Bride;
        }

        public override void LoadData(NPC npc, TagCompound tag)
        {
            Groom = tag.GetBool(nameof(Groom));
            Bride = tag.GetBool(nameof(Bride));
        }
    }
}