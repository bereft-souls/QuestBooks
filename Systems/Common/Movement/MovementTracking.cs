using Terraria.ModLoader.IO;

namespace QuestBooks.Systems.Common.Movement;

public sealed class MovementTrackerPlayer : ModPlayer
{
    private const string Tag = "Tiles";

    /// <summary>
    ///     Gets the total number of pixels the player has walked through.
    /// </summary>
    public float Pixels { get; private set; }

    /// <summary>
    ///     Gets the total number of tiles the player has walked through.
    /// </summary>
    /// <value>
    ///     <see cref="Pixels"/> divided by 16.
    /// </value>
    public int Tiles => (int)(Pixels / 16f);

    /// <summary>
    ///     Gets the total number of miles the player has walked through.
    /// </summary>
    /// <value>
    ///     <see cref="Tiles"/> divided by 2640.
    /// </value>
    public float Miles => Tiles / 2640f;

    // Cached player position last frame
    private Vector2? position;

    public override void PostUpdate()
    {
        position ??= Player.position;
        var distance = Vector2.Distance(position.Value, Player.position);
        Pixels += distance;
        position = Player.position;
    }

    public override void SaveData(TagCompound tag) => tag[Tag] = Pixels;

    public override void LoadData(TagCompound tag) => Pixels = tag.GetFloat(Tag);
}