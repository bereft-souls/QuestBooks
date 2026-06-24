using Terraria.ModLoader.IO;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter3;

public class RunMarathon : QBQuest
{
    /// <summary>
    ///     The number of tiles the player must run in order to complete the quest.
    /// </summary>
    public const int TargetTiles = 69200;

    public const string TagKey = "PlayerPixelsMoved";

    /// <summary>
    ///     Gets the total number of pixels the player has walked through.
    /// </summary>
    public float PixelsTravelled { get; private set; }

    /// <summary>
    ///     Gets the total number of tiles the player has walked through.
    /// </summary>
    /// <value>
    ///     <see cref="PixelsTravelled"/> divided by 16.
    /// </value>
    public int TilesTravelled => (int)(PixelsTravelled / 16f);

    /// <summary>
    ///     Gets the total number of miles the player has walked through.
    /// </summary>
    /// <value>
    ///     <see cref="TilesTravelled"/> divided by 2640.
    /// </value>
    public float MilesTravelled => TilesTravelled / 2640f;

    public override QuestType QuestType => QuestType.Player;

    // Cached player position last frame
    private Vector2? position;

    public override void Update()
    {
        if (Main.dedServ)
            return;

        position ??= Main.LocalPlayer.position;
        var distance = Vector2.Distance(position.Value, Main.LocalPlayer.position);
        PixelsTravelled += distance;
        position = Main.LocalPlayer.position;
    }

    public override bool CheckCompletion() => TilesTravelled >= TargetTiles;

    public override void SaveProgress(TagCompound tag) => tag[TagKey] = PixelsTravelled;
    public override void LoadProgress(TagCompound tag) => PixelsTravelled = tag.GetFloat(TagKey);
}