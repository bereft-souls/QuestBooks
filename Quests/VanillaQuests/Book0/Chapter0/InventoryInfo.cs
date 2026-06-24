using QuestBooks.Assets;
using Terraria.Audio;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class InventoryInfo : InfoQuest
{
    public byte InfoState { get; set; } = 0;

    public override bool DrawCustomInfoPage(SpriteBatch spriteBatch, Vector2 mousePosition, ref Action updateAction)
    {
        var mousePos = mousePosition.ToPoint();
        bool mousePressed = Main.mouseLeft && Main.mouseLeftRelease;

        Rectangle firstButton = new(10, 480, 80, 40);
        Rectangle secondButton = firstButton.CookieCutter(new(2.5f, 0f), Vector2.One);
        Rectangle thirdButton = secondButton.CookieCutter(new(2.5f, 0f), Vector2.One);
        Rectangle fourthButton = thirdButton.CookieCutter(new(2.5f, 0f), Vector2.One);

        bool firstHovered = firstButton.Contains(mousePos);
        bool secondHovered = secondButton.Contains(mousePos);
        bool thirdHovered = thirdButton.Contains(mousePos);
        bool fourthHovered = fourthButton.Contains(mousePos);

        byte infoState = InfoState;
        InfoState = mousePressed switch
        {
            true when firstHovered => 0,
            true when secondHovered => 1,
            true when thirdHovered => 2,
            true when fourthHovered => 3,
            _ => InfoState
        };

        if (infoState != InfoState)
            SoundEngine.PlaySound(SoundID.MenuTick);

        spriteBatch.DrawPatchRectangle(InfoState == 0 ?
            (firstHovered ? QuestAssets.SelectedRectangleHovered : QuestAssets.SelectedRectangle) :
            (firstHovered ? QuestAssets.SimpleRectangleHovered : QuestAssets.SimpleRectangle), firstButton);

        spriteBatch.DrawPatchRectangle(InfoState == 1 ?
            (secondHovered ? QuestAssets.SelectedRectangleHovered : QuestAssets.SelectedRectangle) :
            (secondHovered ? QuestAssets.SimpleRectangleHovered : QuestAssets.SimpleRectangle), secondButton);

        spriteBatch.DrawPatchRectangle(InfoState == 2 ?
            (thirdHovered ? QuestAssets.SelectedRectangleHovered : QuestAssets.SelectedRectangle) :
            (thirdHovered ? QuestAssets.SimpleRectangleHovered : QuestAssets.SimpleRectangle), thirdButton);

        spriteBatch.DrawPatchRectangle(InfoState == 3 ?
            (fourthHovered ? QuestAssets.SelectedRectangleHovered : QuestAssets.SelectedRectangle) :
            (fourthHovered ? QuestAssets.SimpleRectangleHovered : QuestAssets.SimpleRectangle), fourthButton);

        return false;
    }

    public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
    {
        base.MakeSimpleInfoPage(out title, out contents, out texture);
        contents = this.GetLocalization($"Contents{InfoState}").Format(LocalizationArgs);
    }
}