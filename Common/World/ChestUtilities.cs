namespace QuestBooks.Common.World;

public static class ChestExtensions
{
    /// <summary>
    ///     Determines whether the chest has been naturally generated.
    /// </summary>
    /// <param name="chest">
    ///     The chest to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the chest has been naturally generated; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool IsNatural(this Chest chest) => ChestSystem.IsNatural(chest.x, chest.y);

    /// <summary>
    ///     Determines whether the chest has been explored by a player.
    /// </summary>
    /// <param name="chest">
    ///     The chest to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the chest has been explored by a player; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool IsExplored(this Chest chest) => ChestSystem.IsExplored(chest.x, chest.y);
}