namespace QuestBooks.Quests.VanillaQuests;

/// <summary>
/// Intended for internal QuestBooks use.<br/>
/// Shorthands the localization category based on namespace instead of <c>QuestBooks</c>.
/// </summary>
public abstract class QBQuest : Quest
{
    private string _localizationCategory = null;

    public override string LocalizationCategory
    {
        get
        {
            _localizationCategory ??= GetType().Namespace[GetType().Namespace.LastIndexOf("Book")..];
            return _localizationCategory;
        }
    }
}
