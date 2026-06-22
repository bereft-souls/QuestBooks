using System.Collections.Generic;
using QuestBooks.Content.Sets;
using Terraria.ModLoader.IO;

namespace QuestBooks.Common.Items;

public sealed class ItemObtainmentPlayer : ModPlayer
{
    private const string Tag = "Flags";

    private List<int> flags = [];

    public override bool OnPickup(Item item)
    {
        flags.Add(item.type);

        return true;
    }

    public override void SaveData(TagCompound tag) => tag[Tag] = flags;

    public override void LoadData(TagCompound tag) => flags = new List<int>(tag.GetList<int>(Tag));

    /// <summary>
    ///     Determines whether the player has obtained the specified item type.
    /// </summary>
    /// <param name="type">
    ///     The item type to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has obtained the specified item type; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public bool Obtained(int type)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        return flags.Contains(type);
    }

    /// <summary>
    ///     Determines whether the player has obtained any of the specified item types.
    /// </summary>
    /// <param name="types">
    ///     The item types to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has obtained any of the specified item types; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Any of the values in <paramref name="types"/> is negative or zero.
    /// </exception>
    public bool ObtainedAny(params int[] types)
    {
        foreach (var type in types)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

            if (flags.Contains(type))
                return true;
        }

        return false;
    }

    /// <summary>
    ///     Determines whether the player has obtained any of the item types from the specified set.
    /// </summary>
    /// <param name="set">
    ///     The set of the item types to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has obtained any of the item types from the specified set;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public bool ObtainedAny(bool[] set)
    {
        for (var i = 0; i < set.Length; i++)
            if (set[i] && flags.Contains(i))
                return true;

        return false;
    }
    
    /// <summary>
    ///     Determines whether the player has obtained any of the item types from the specified set.
    /// </summary>
    /// <param name="set">
    ///     The set of the item types to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has obtained any of the item types from the specified set;
    /// </returns>
    public bool ObtainedAny(ContentSet set)
    {
        foreach (var type in set)
            if (flags.Contains(type))
                return true;

        return false;
    }

    /// <summary>
    ///     Determines whether the player has obtained all of the specified item types.
    /// </summary>
    /// <param name="types">
    ///     The item types to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has obtained all of the specified item types; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Any of the values in <paramref name="types"/> is negative or zero.
    /// </exception>
    public bool ObtainedAll(params int[] types)
    {
        foreach (var type in types)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

            if (!flags.Contains(type))
                return false;
        }

        return true;
    }

    /// <summary>
    ///     Determines whether the player has obtained all of the item types from the specified set.
    /// </summary>
    /// <param name="set">
    ///     The set of the item types to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has obtained all of the item types from the specified set;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public bool ObtainedAll(bool[] set)
    {
        for (var i = 0; i < set.Length; i++)
            if (set[i] && !flags.Contains(i))
                return false;

        return true;
    }

    /// <summary>
    ///     Determines whether the player has obtained all of the item types from the specified set.
    /// </summary>
    /// <param name="set">
    ///     The set of the item types to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has obtained all of the item types from the specified set;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public bool ObtainedAll(ContentSet set)
    {
        foreach (var type in set)
            if (!flags.Contains(type))
                return false;

        return true;
    }
}