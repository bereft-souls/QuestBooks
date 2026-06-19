using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace QuestBooks.Utilities;

/// <summary>
///     Provides <see cref="Player"/> extensions.
/// </summary>
public static class PlayerExtensions
{
#region Inventory
    /// <summary>
    ///     Determines whether the player has an item of any of the specified types in their inventory.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="types">
    ///     The types of the items to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has an item of any of the specified types in their
    ///     inventory; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Any of the values in <paramref name="types"/> is negative or zero.
    /// </exception>
    public static bool HasItem(this Player player, params int[] types)
    {
        foreach (var type in types)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

            if (player.HasItem(type))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Determines whether the player has at least the specified amount of an item of any of the
    ///     specified types in their inventory.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="entries">
    ///     The types and amounts of the items to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has at least the specified amount of an item of any of the
    ///     specified types in their inventory; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Any of the values in <paramref name="entries"/> is negative or zero.
    /// </exception>
    public static bool HasItem(this Player player, params (int Type, int Stack)[] entries)
    {
        foreach (var (type, stack) in entries)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(stack);

            if (player.HasItem(type, stack))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Determines whether the player has at least the specified amount of an item of the specified
    ///     type in their inventory.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="amount"></param>
    /// <typeparam name="TModItem">
    ///     The type of the item to check for.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true"/> if the player has at least the specified amount of an item of the
    ///     specified type in their inventory; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasItem<TModItem>(this Player player, int amount) where TModItem : ModItem
    {
        return player.HasItem(ModContent.ItemType<TModItem>(), amount);
    }

    /// <summary>
    ///     Determines whether the player has at least the specified amount of an item of the specified
    ///     type in their inventory.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="type">
    ///     The type of the item to check for.
    /// </param>
    /// <param name="amount">
    ///     The amount of the item to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has at least the specified amount of an item of the
    ///     specified type in their inventory; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> or <paramref name="amount"/> is negative or zero.
    /// </exception>
    public static bool HasItem(this Player player, int type, int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        return player.HasItem(type) && player.CountItem(type) >= amount;
    }

    /// <summary>
    ///     Enumerates the items in the player's inventory along with their respective indices.
    /// </summary>
    /// <param name="player">
    ///     The player whose inventory to enumerate.
    /// </param>
    /// <returns>
    ///     An enumerable of tuples, where each tuple contains an <see cref="Item"/> from the player's
    ///     inventory and its corresponding index in the player's inventory array.
    /// </returns>
    public static IEnumerable<(Item Item, int Index)> EnumerateInventory(this Player player)
    {
        for (var i = 0; i < 58; i++)
        {
            var item = player.inventory[i];

            if (item.IsAir)
            {
                continue;
            }

            yield return (item, i);
        }
    }
#endregion

#region Armor
    /// <summary>
    ///     Determines whether the player is wearing the specified leggings.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <typeparam name="TModItem">
    ///     The type of the leggings to check for.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing the specified leggings; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasLeggings<TModItem>(this Player player) where TModItem : ModItem
    {
        return player.HasLeggings(ModContent.ItemType<TModItem>());
    }

    /// <summary>
    ///     Determines whether the player is wearing the specified leggings.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="type">
    ///     The type of the leggings to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing the specified leggings; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public static bool HasLeggings(this Player player, int type)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        return player.armor[2].type == type;
    }

    /// <summary>
    ///     Determines whether the player is wearing the specified chestplate.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <typeparam name="TModItem">
    ///     The type of the chestplate to check for.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing the specified chestplate; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasChestplate<TModItem>(this Player player) where TModItem : ModItem
    {
        return player.HasChestplate(ModContent.ItemType<TModItem>());
    }

    /// <summary>
    ///     Determines whether the player is wearing the specified chestplate.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="type">
    ///     The type of the chestplate to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing the specified chestplate; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public static bool HasChestplate(this Player player, int type)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        return player.armor[1].type == type;
    }

    /// <summary>
    ///     Determines whether the player is wearing the specified helmet.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <typeparam name="TModItem">
    ///     The type of the helmet to check for.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing the specified helmet; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasHelmet<TModItem>(this Player player) where TModItem : ModItem
    {
        return player.HasHelmet(ModContent.ItemType<TModItem>());
    }

    /// <summary>
    ///     Determines whether the player is wearing the specified helmet.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="type">
    ///     The type of the helmet to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing the specified helmet; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public static bool HasHelmet(this Player player, int type)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        return player.armor[0].type == type;
    }

    /// <summary>
    ///     Determines whether the player is wearing the specified armor set.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <typeparam name="THelmet">
    ///     The type of the helmet to check for.
    /// </typeparam>
    /// <typeparam name="TChestplate">
    ///     The type of the chestplate to check for.
    /// </typeparam>
    /// <typeparam name="TLeggings">
    ///     The type of the leggings to check for.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing the specified armor set; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasArmorSet<THelmet, TChestplate, TLeggings>(this Player player) where THelmet : ModItem where TChestplate : ModItem where TLeggings : ModItem
    {
        return player.HasArmorSet(ModContent.ItemType<THelmet>(), ModContent.ItemType<TChestplate>(), ModContent.ItemType<TLeggings>());
    }

    /// <summary>
    ///     Determines whether the player is wearing the specified armor set.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="helmet">
    ///     The type of the helmet to check for.
    /// </param>
    /// <param name="chestplate">
    ///     The type of the chestplate to check for.
    /// </param>
    /// <param name="leggings">
    ///     The type of the leggings to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing the specified armor set; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasArmorSet(this Player player, int helmet, int chestplate, int leggings)
    {
        return player.HasHelmet(helmet) && player.HasChestplate(chestplate) && player.HasLeggings(leggings);
    }
#endregion

#region Accessories
    /// <summary>
    ///     Determines whether the player is wearing an accessory of the specified type.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <typeparam name="TModItem">
    ///     The type of the accessory to check for.
    /// </typeparam>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing an accessory of the specified type; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasAccessory<TModItem>(this Player player) where TModItem : ModItem
    {
        return player.HasAccessory(ModContent.ItemType<TModItem>());
    }

    /// <summary>
    ///     Determines whether the player is wearing an accessory of the specified type.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="type">
    ///     The type of the accessory to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing an accessory of the specified type; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="type"/> is negative or zero.
    /// </exception>
    public static bool HasAccessory(this Player player, int type)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

        return player.EnumerateAccessories().Any(accessory => accessory.Item.type == type);
    }

    /// <summary>
    ///     Enumerates the accessories the player is wearing along with their respective indices.
    /// </summary>
    /// <param name="player">
    ///     The player whose accessories to enumerate.
    /// </param>
    /// <returns>
    ///     An enumerable of tuples, where each tuple contains an <see cref="Item"/> representing an
    ///     accessory the player is wearing and its corresponding index in the player's armor array.
    /// </returns>
    public static IEnumerable<(Item Item, int Index)> EnumerateAccessories(this Player player)
    {
        for (var i = 3; i < 8 + player.extraAccessorySlots; i++)
        {
            var item = player.armor[i];

            if (item.IsAir)
            {
                continue;
            }

            yield return (item, i);
        }
    }
#endregion
}