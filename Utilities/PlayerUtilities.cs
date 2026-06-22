using System.Collections.Generic;
using System.Linq;
using QuestBooks.Content.Sets;

namespace QuestBooks.Utilities;

/// <summary>
///     Provides <see cref="Player"/> extensions.
/// </summary>
public static class PlayerExtensions
{
    /// <summary>
    ///     Determines whether the player has an item of all of the types from the specified set in their inventory.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="set">
    ///     The set of the items to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has an item of all of the types from the specified set in their inventory; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasAllItems(this Player player, ContentSet set)
    {
        foreach (var type in set)
            if (!player.HasItem(type))
                return false;

        return true;
    }
    
    /// <summary>
    ///     Determines whether the player has an item of all of the specified types in their inventory.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="types">
    ///     The types of the items to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has an item of all of the specified types in their
    ///     inventory; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Any of the values in <paramref name="types"/> is negative or zero.
    /// </exception>
    public static bool HasAllItems(this Player player, params int[] types)
    {
        foreach (var type in types)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

            if (!player.HasItem(type))
                return false;
        }

        return true;
    }
    
    /// <summary>
    ///     Determines whether the player has any item of the specified type in their inventory.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <param name="set">
    ///     The set of the items to check for.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has an item of any of the types from the specified set in their inventory; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasAnyItem(this Player player, ContentSet set) => player.EnumerateInventory().Any(item => set.Contains(item.Item.type));

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
    public static bool HasAnyItem(this Player player, params int[] types)
    {
        foreach (var type in types)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(type);

            if (player.HasItem(type))
                return true;
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
    public static bool HasItem<TModItem>(this Player player, int amount) where TModItem : ModItem => player.HasItem(ModContent.ItemType<TModItem>(), amount);

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
                continue;

            yield return (item, i);
        }
    }

    /// <summary>
    ///     Determines whether the player is wearing any leggings.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing any leggings; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasLeggings(this Player player) => !player.armor[2].IsAir;

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
    public static bool HasLeggings<TModItem>(this Player player) where TModItem : ModItem => player.HasLeggings(ModContent.ItemType<TModItem>());

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
    ///     Determines whether the player is wearing any chestplate.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing any chestplate; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasChestplate(this Player player) => !player.armor[1].IsAir;

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
    public static bool HasChestplate<TModItem>(this Player player) where TModItem : ModItem => player.HasChestplate(ModContent.ItemType<TModItem>());

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
    ///     Determines whether the player is wearing any helmet.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing any helmet; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasHelmet(this Player player) => !player.armor[0].IsAir;

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
    public static bool HasHelmet<TModItem>(this Player player) where TModItem : ModItem => player.HasHelmet(ModContent.ItemType<TModItem>());

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
    ///     Determines whether the player is wearing any armor set.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player is wearing any armor set; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasArmorSet(this Player player) => player.HasHelmet() && player.HasChestplate() && player.HasLeggings();

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
    public static bool HasArmorSet<THelmet, TChestplate, TLeggings>(this Player player) where THelmet : ModItem where TChestplate : ModItem where TLeggings : ModItem => player.HasArmorSet
        (ModContent.ItemType<THelmet>(), ModContent.ItemType<TChestplate>(), ModContent.ItemType<TLeggings>());

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
    public static bool HasArmorSet(this Player player, int helmet, int chestplate, int leggings) => player.HasHelmet(helmet) && player.HasChestplate(chestplate) && player.HasLeggings(leggings);

    /// <summary>
    ///     Enumerates the armor pieces the player is wearing along with their respective indices.
    /// </summary>
    /// <param name="player">
    ///     The player whose armor pieces to enumerate.
    /// </param>
    /// <returns>
    ///     An enumerable of tuples, where each tuple contains an <see cref="Item"/> representing an armor
    ///     piece the player is wearing and its corresponding index in the player's armor array.
    /// </returns>
    public static IEnumerable<(Item Item, int Index)> EnumerateArmor(this Player player)
    {
        for (var i = 0; i < 3; i++)
        {
            var item = player.armor[i];

            if (item.IsAir)
                continue;

            yield return (item, i);
        }
    }

    /// <summary>
    ///     Enumerates the armor dyes the player is wearing along with their respective indices.
    /// </summary>
    /// <param name="player">
    ///     The player whose armor dyes to enumerate.
    /// </param>
    /// <returns>
    ///     An enumerable of tuples, where each tuple contains an <see cref="Item"/> representing an armor
    ///     dye the player is wearing and its corresponding index in the player's dye array.
    /// </returns>
    public static IEnumerable<(Item Item, int Index)> EnumerateArmorDyes(this Player player)
    {
        for (var i = 0; i < 3; i++)
        {
            var item = player.dye[i];

            if (item.IsAir)
                continue;

            yield return (item, i);
        }
    }

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
    public static bool HasAccessory<TModItem>(this Player player) where TModItem : ModItem => player.HasAccessory(ModContent.ItemType<TModItem>());

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
                continue;

            yield return (item, i);
        }
    }

    /// <summary>
    ///     Enumerates the accessory dyes the player is wearing along with their respective indices.
    /// </summary>
    /// <param name="player">
    ///     The player whose accessory dyes to enumerate.
    /// </param>
    /// <returns>
    ///     An enumerable of tuples, where each tuple contains an <see cref="Item"/> representing an
    ///     accessory dye the player is wearing and its corresponding index in the player's dye array.
    /// </returns>
    public static IEnumerable<(Item Item, int Index)> EnumerateAccessoryDyes(this Player player)
    {
        for (var i = 3; i < 8 + player.extraAccessorySlots; i++)
        {
            var item = player.dye[i];

            if (item.IsAir)
                continue;

            yield return (item, i);
        }
    }

    /// <summary>
    ///     Determines whether the player has any pet equipped.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has any pet equipped; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasPet(this Player player) => !player.miscEquips[0].IsAir;

    /// <summary>
    ///     Determines whether the player has any light pet equipped.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has any light pet equipped; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasLightPet(this Player player) => !player.miscEquips[1].IsAir;

    /// <summary>
    ///     Determines whether the player has any mount equipped.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has any mount equipped; otherwise, <see langword="false"/>
    ///     .
    /// </returns>
    public static bool HasMount(this Player player) => !player.miscEquips[2].IsAir;
    
    /// <summary>
    ///     Determines whether the player has any minecart equipped.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has any minecart equipped; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasMinecart(this Player player) => !player.miscEquips[3].IsAir;

    /// <summary>
    ///     Determines whether the player has any minecart equipped.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has any minecart equipped; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool HasHook(this Player player) => !player.miscEquips[4].IsAir;

    /// <summary>
    ///     Determines whether the player has any wings equipped.
    /// </summary>
    /// <param name="player">
    ///     The player to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the player has any wings equipped; otherwise, <see langword="false"/>
    ///     .
    /// </returns>
    public static bool HasWings(this Player player) => player.EnumerateAccessories().Any(static accessory => accessory.Item.wingSlot > 0);

    /// <summary>
    ///     Enumerates the equipments the player is wearing along with their respective indices.
    /// </summary>
    /// <param name="player">
    ///     The player whose equipments to enumerate.
    /// </param>
    /// <returns>
    ///     An enumerable of tuples, where each tuple contains an <see cref="Item"/> representing an equipment
    ///     the player is wearing and its corresponding index in the player's equipments array.
    /// </returns>
    public static IEnumerable<(Item Item, int Index)> EnumerateEquipments(this Player player)
    {
        for (var i = 0; i < 4; i++)
        {
            var item = player.miscEquips[i];
            
            if (item.IsAir)
                continue;
            
            yield return (item, i);
        }
    }

    /// <summary>
    ///     Enumerates the equipment dyes the player is wearing along with their respective indices.
    /// </summary>
    /// <param name="player">
    ///     The player whose equipment dyes to enumerate.
    /// </param>
    /// <returns>
    ///     An enumerable of tuples, where each tuple contains an <see cref="Item"/> representing an equipment
    ///     dye the player is wearing and its corresponding index in the player's dye array.
    /// </returns>
    public static IEnumerable<(Item Item, int Index)> EnumerateEquipmentDyes(this Player player)
    {
        for (var i = 0; i < 4; i++)
        {
            var item = player.miscDyes[i];
            
            if (item.IsAir)
                continue;
            
            yield return (item, i);
        }
    }
}