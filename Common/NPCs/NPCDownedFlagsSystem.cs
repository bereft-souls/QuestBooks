namespace QuestBooks.Common.NPCs;

public sealed class NPCDownedFlagsSystem : ModSystem
{
    private sealed class NPCDownedFlagsSystemGlobalNPC : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => lateInstantiation && tracked[entity.type];

        public override void OnKill(NPC npc) => Mark(npc.type);
    }

    private static bool[] flags;

    private static bool[] tracked;

    public override void PostSetupContent()
    {
        tracked = NPCID.Sets.Factory.CreateBoolSet
        (
            NPCID.SandElemental,
            NPCID.IceGolem,
            NPCID.GoblinSummoner,
            NPCID.Mothron,
            NPCID.WyvernHead,
            NPCID.PirateShip,
            NPCID.BigMimicCorruption,
            NPCID.BigMimicCrimson,
            NPCID.BigMimicHallow,
            NPCID.BigMimicJungle,
            NPCID.ChaosElemental,
            NPCID.RainbowSlime
        );

        flags = NPCID.Sets.Factory.CreateBoolSet();
    }

    public override void ClearWorld() => Array.Clear(flags, 0, flags.Length);

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
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public static bool Downed(int type)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        return flags[type];
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
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Any of the values in <paramref name="types"/> is negative or zero.
    /// </exception>
    public static bool DownedAny(params int[] types)
    {
        foreach (var type in types)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

            if (flags[type])
                return true;
        }

        return false;
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
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Any of the values in <paramref name="types"/> is negative or zero.
    ///     >
    /// </exception>
    public static bool DownedAll(params int[] types)
    {
        foreach (var type in types)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

            if (!flags[type])
                return false;
        }

        return true;
    }

    /// <summary>
    ///     Marks the NPC with the specified type to be tracked for being killed at least once.
    /// </summary>
    /// <param name="type">
    ///     The type of the NPC to mark.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public static void Track(int type)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        tracked[type] = true;
    }

    /// <summary>
    ///     Marks the NPC with the specified type as killed at least once.
    /// </summary>
    /// <param name="type">
    ///     The type of the NPC to mark.
    /// </param>
    /// <param name="synchronize">
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public static void Mark(int type, bool synchronize = true)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        flags[type] = true;

        if (!synchronize)
            return;

        NPC.SetEventFlagCleared(ref flags[type], -1);
    }
}