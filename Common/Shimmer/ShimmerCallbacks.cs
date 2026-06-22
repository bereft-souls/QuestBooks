namespace QuestBooks.Common.Shimmer;

public sealed class ShimmerCallbacks : ModSystem
{
    private sealed class ShimmerCallbacksPlayer : ModPlayer
    {
        private bool flag;

        public override void PostUpdate()
        {
            var shimmering = Player.shimmering;

            if (shimmering && !flag)
                OnStartPhase?.Invoke(Player);

            if (!shimmering && flag)
                OnEndPhase?.Invoke(Player);

            flag = shimmering;
        }
    }

    /// <summary>
    ///     Invoked when a player enters the shimmer state.
    /// </summary>
    /// <param name="player">
    ///     The player that started shimmering.
    /// </param>
    public delegate void ShimmerStartCallback(Player player);

    /// <summary>
    ///     Invoked when a player exits the shimmer state.
    /// </summary>
    /// <param name="player">
    ///     The player that stopped shimmering.
    /// </param>
    public delegate void ShimmerEndCallback(Player player);

    /// <summary>
    ///     Invoked when a player enters the shimmer state.
    /// </summary>
    public static ShimmerStartCallback OnStartPhase;

    /// <summary>
    ///     Invoked when a player enters the shimmer state.
    /// </summary>
    public static ShimmerEndCallback OnEndPhase;

    public override void Unload()
    {
        OnStartPhase = null;
        OnEndPhase = null;
    }
}