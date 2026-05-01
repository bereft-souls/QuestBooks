using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class ChopTree : QBQuest
{
    public override bool CheckCompletion()
    {
        return false;  // TODO: checked when player chops tree
    }

    public class PlayerCheck : ModPlayer
    {

    }
}