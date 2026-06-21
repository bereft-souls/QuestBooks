namespace QuestBooks.Common.NPCs;

public static class NPCUtilities
{
    /// <summary>
    ///     Determines whether any of the given NPC types are present in the world.
    /// </summary>
    /// <param name="types">
    ///     The NPC types to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if any of the given NPC types are present in the world; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Any of the values in <paramref name="types"/> is negative or zero.
    /// </exception>
    public static bool AnyNPCs(params int[] types)
    {
        foreach (var type in types)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

            if (NPC.AnyNPCs(type))
                return true;
        }

        return false;
    }

    /// <summary>
    ///     Determines whether all of the given NPC types are present in the world.
    /// </summary>
    /// <param name="types">
    ///     The NPC types to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if all of the given NPC types are present in the world; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Any of the values in <paramref name="types"/> is negative or zero.
    /// </exception>
    public static bool AllNPCs(params int[] types)
    {
        foreach (var type in types)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

            if (!NPC.AnyNPCs(type))
                return false;
        }

        return true;
    }
}