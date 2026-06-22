using System.IO;
using Terraria.ModLoader.IO;

namespace QuestBooks.Common.NPCs;

public sealed class DayEmpressOfLightSystem : ModSystem
{
    private sealed class DayEmpressOfLightGlobalNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.HallowBoss;

        public override void OnKill(NPC npc) => NPC.SetEventFlagCleared(ref Downed, -1);
    }

    private const string Tag = "Downed";

    /// <summary>
    ///     A value indicating whether the Empress of Light has been defeated in the world during the day.
    /// </summary>
    public static bool Downed;

    public override void ClearWorld() => Downed = false;

    public override void SaveWorldData(TagCompound tag) => tag[Tag] = Downed;

    public override void LoadWorldData(TagCompound tag) => Downed = tag.GetBool(Tag);

    public override void NetSend(BinaryWriter writer) => writer.WriteFlags(Downed);
    
    public override void NetReceive(BinaryReader reader) => reader.ReadFlags(out Downed);
}