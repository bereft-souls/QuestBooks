namespace QuestBooks.Content.Sets;

public sealed partial class ItemSetsSystem : ModSystem
{
    public override void PostSetupContent()
    {
        SetupBoots();
        SetupFurniture();
        SetupOres();
    }
}