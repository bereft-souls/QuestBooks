namespace QuestBooks.Quests.VanillaQuests;

/// <summary>
/// Intended for internal QuestBooks use.<br/>
/// Shorthands the localization category based on namespace instead of <c>QuestBooks</c>.
/// </summary>

public abstract class VanillaQuest : Quest
{
    private string _localizationCategory = null;

    public override string TextureCategory => $"{Mod.Name}/Assets/Textures/Quests/InfoPages";

    public override string LocalizationCategory
    {
        get
        {
            _localizationCategory ??= GetType().Namespace[GetType().Namespace.LastIndexOf("Book")..];
            return _localizationCategory;
        }
    }
}

/// <summary>
/// Intended for internal QuestBooks use.<br/>
/// Acts as a quest that is marked as completed once it has been opened.
/// </summary>
public abstract class InfoQuest : VanillaQuest
{
    public override QuestType QuestType => QuestType.Player;

    public bool Read { get; set; } = false;

    public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
    {
        base.MakeSimpleInfoPage(out title, out contents, out texture);
        Read = true;
    }

    public override bool CheckCompletion() => Read;
}

internal class Placeholder : Quest
{
    public override bool CheckCompletion() => true;

    public override string LocalizationCategory => "Tooltips";
}