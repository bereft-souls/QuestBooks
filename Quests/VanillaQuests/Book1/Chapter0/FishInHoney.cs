using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter0;

public class FishInHoney : QBQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class FishInHoneyCheck : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.bobber;

        public override void AI(Projectile projectile)
        {
            if (!projectile.honeyWet)
                return;

            QuestManager.MarkComplete<FishInHoney>();
        }
    }
}