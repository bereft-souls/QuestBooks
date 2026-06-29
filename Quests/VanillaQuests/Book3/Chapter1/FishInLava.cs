using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book3.Chapter1;

public class FishInLava : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public override bool CheckCompletion() => false;

    public class FishInLavaCheck : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.bobber;

        public override void AI(Projectile projectile)
        {
            if (!projectile.lavaWet)
                return;

            QuestBooksMod.MarkComplete<FishInLava>();
        }
    }
}