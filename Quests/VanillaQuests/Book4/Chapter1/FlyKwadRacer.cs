using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book4.Chapter1;

public class FlyKwadRacer : QBQuest
{
    public override bool CheckCompletion() => false;

    public class FlyKwadRacerCheck : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.JimsDrone;

        public override void AI(Projectile projectile)
        {
            if (projectile.position.Y - projectile.height > 16f * Main.offScreenRange / 2f)
                return;

            QuestBooksMod.MarkComplete<FlyKwadRacer>();
        }
    }
}