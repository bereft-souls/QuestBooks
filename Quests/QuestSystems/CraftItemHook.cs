using QuestBooks.Systems;
using Terraria.DataStructures;

namespace QuestBooks.Quests.QuestSystems
{
    /// <summary>
    /// Provides a simple hook into when an item is crafted.<br/>
    /// <br/>
    /// See also:<br/>
    /// <see cref="CraftItemHook{TItemType}"/><br/>
    /// <see cref="CraftItemCheck{TQuest}"/><br/>
    /// <see cref="CraftItemCheck{TQuest, TItemType}"/>
    /// </summary>
    /// <param name="match">Checks whether this hook should fire.</param>
    /// <param name="onComplete">The action you want to perform when matched to a crafted item.</param>
    public abstract class CraftItemHook(Func<Item, ItemCreationContext, bool> match, Action<Item, ItemCreationContext> onComplete) : GlobalItem
    {
        public CraftItemHook(int itemType, Action<Item, ItemCreationContext> onComplete) : this((item, context) => item.type == itemType, onComplete) { }

        public Func<Item, ItemCreationContext, bool> Match { get; init; } = match;

        public Action<Item, ItemCreationContext> OnComplete { get; init; } = onComplete;

        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (context is RecipeItemCreationContext && Match(item, context))
                OnComplete(item, context);
        }
    }

    /// <summary>
    /// Allows you to run a hook every time the specified <typeparamref name="TItemType"/> is crafted.
    /// </summary>
    public abstract class CraftItemHook<TItemType>(Action<Item, ItemCreationContext> onComplete) : CraftItemHook((item, context) => item.type == ModContent.ItemType<TItemType>(), onComplete)
        where TItemType : ModItem;

    /// <summary>
    /// Allows you to set up a hook to automatically complete <typeparamref name="TQuest"/> when an item matching given criteria is crafted.
    /// </summary>
    public abstract class CraftItemCheck<TQuest>(Func<Item, ItemCreationContext, bool> match) : CraftItemHook(match, (_, _) => QuestManager.CompleteQuest<TQuest>())
        where TQuest : Quest
    {
        /// <summary>
        /// Allows you to set up a hook to automatically complete <typeparamref name="TQuest"/> when the specified <paramref name="itemType"/> is crafted.
        /// </summary>
        public CraftItemCheck(int itemType) : this((item, context) => item.type == itemType) { }
    }

    /// <summary>
    /// Allows you to set up a hook to automatically complete <typeparamref name="TQuest"/> when the specified <typeparamref name="TItemType"/> is crafted.
    /// </summary>
    public abstract class CraftItemCheck<TQuest, TItemType>() : CraftItemCheck<TQuest>(ModContent.ItemType<TItemType>())
        where TQuest : Quest
        where TItemType : ModItem;
}