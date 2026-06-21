using MonoMod.Cil;
using QuestBooks.Systems;

namespace QuestBooks.Quests.VanillaQuests.Book1.Chapter1;

public class TorchGod : QBQuest
{
    public override bool CheckCompletion() => false;

    public class TorchGodCheck : ModSystem
    {
        public override void Load() => IL_Player.TorchAttack += Edit;

        private static void Edit(ILContext context)
        {
            var cursor = new ILCursor(context);

            if (!cursor.TryGotoNext(MoveType.After, static i => i.MatchLdarg0(), static i => i.MatchLdfld<Player>("numberOfTorchAttacksMade"), static i => i.MatchLdcI4(95)))
                throw new Exception();

            cursor.EmitDelegate(static () => QuestManager.MarkComplete<TorchGod>());
        }
    }
}