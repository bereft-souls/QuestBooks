using System.Collections;
using System.IO;
using QuestBooks.Content.Sets;
using QuestBooks.Core.IO;
using Terraria.ModLoader.IO;

namespace QuestBooks.Common.NPCs;

public sealed class NPCDownedFlagsSystem : ModSystem
{
    private sealed class NPCDownedFlagsGlobalNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => lateInstantiation && NPCSets.Telemetry.Downed[entity.type];

        public override void OnKill(NPC npc) => NPC.SetEventFlagCleared(ref flags[npc.type], -1);
    }

    private const string Tag = "Flags";

    private static bool[] flags;

    public override void PostSetupContent() => flags = NPCID.Sets.Factory.CreateBoolSet();

    public override void ClearWorld() => Array.Clear(flags, 0, flags.Length);

    public override void SaveWorldData(TagCompound tag) => tag[Tag] = flags;

    public override void LoadWorldData(TagCompound tag) => flags = tag.GetBoolArray(Tag);

    public override void NetSend(BinaryWriter writer) => Utils.SendBitArray(new BitArray(flags), writer);

    public override void NetReceive(BinaryReader reader) => Utils.ReceiveBitArray(flags.Length, reader).CopyTo(flags, 0);

    /// <summary>
    ///     Determines whether the NPC with the specified type has been killed at least once.
    /// </summary>
    /// <param name="type">
    ///     The type of the NPC to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the NPC with the specified type has been killed at least once;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Downed(int type) => flags[NPCID.FromNetId(type)];

    /// <summary>
    ///     Determines whether any of the NPCs from the specified set has been killed at least once.
    /// </summary>
    /// <param name="set">
    ///     The set of the NPCs to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if any of the NPCs from the specified set has been killed at least once;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool DownedAny(bool[] set)
    {
        for (var i = 0; i < set.Length; i++)
            if (set[i] && flags[i])
                return true;

        return false;
    }

    /// <summary>
    ///     Determines whether any of the NPCs with the specified types has been killed at least once.
    /// </summary>
    /// <param name="types">
    ///     The types of the NPCs to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if any of the NPCs with the specified types has been killed at least
    ///     once; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool DownedAny(params int[] types)
    {
        foreach (var type in types)
            if (flags[NPCID.FromNetId(type)])
                return true;

        return false;
    }

    /// <summary>
    ///     Determines whether any of the NPCs from the specified set has been killed at least once.
    /// </summary>
    /// <param name="set">
    ///     The set of the NPCs to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if any of the NPCs from the specified set has been killed at least once;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool DownedAny(ContentSet set)
    {
        foreach (var type in set)
            if (flags[type])
                return true;

        return false;
    }

    /// <summary>
    ///     Determines whether all of the NPCs from the specified set have been killed at least once.
    /// </summary>
    /// <param name="set">
    ///     The set of the NPCs to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if all of the NPCs from the specified set have been killed at least
    ///     once; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool DownedAll(bool[] set)
    {
        for (var i = 0; i < set.Length; i++)
            if (set[i] && !flags[i])
                return false;

        return true;
    }

    /// <summary>
    ///     Determines whether all of the NPCs with the specified types have been killed at least once.
    /// </summary>
    /// <param name="types">
    ///     The types of the NPCs to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if all of the NPCs with the specified types have been killed at least
    ///     once; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool DownedAll(params int[] types)
    {
        foreach (var type in types)
            if (!flags[NPCID.FromNetId(type)])
                return false;

        return true;
    }

    /// <summary>
    ///     Determines whether any of the NPCs from the specified set has been killed at least once.
    /// </summary>
    /// <param name="set">
    ///     The set of the NPCs to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if any of the NPCs from the specified set has been killed at least once;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool DownedAll(ContentSet set)
    {
        foreach (var type in set)
            if (!flags[type])
                return false;

        return true;
    }
}