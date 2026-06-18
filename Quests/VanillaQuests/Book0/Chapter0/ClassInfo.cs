using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuestBooks.Assets;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuestBooks.Quests.VanillaQuests.Book0.Chapter0;

public class ClassInfo : InfoQuest
{
    public byte InfoState { get; set; } = 0;

    public override bool DrawCustomInfoPage(SpriteBatch spriteBatch, Vector2 mousePosition, ref Action updateAction)
    {
        var mousePos = mousePosition.ToPoint();
        bool mousePressed = Main.mouseLeft && Main.mouseLeftRelease;

        Rectangle firstButton = new(10, 480, 64, 40);
        Rectangle secondButton = firstButton.CookieCutter(new(2.5f, 0f), Vector2.One);
        Rectangle thirdButton = secondButton.CookieCutter(new(2.5f, 0f), Vector2.One);
        Rectangle fourthButton = thirdButton.CookieCutter(new(2.5f, 0f), Vector2.One);
        Rectangle fifthButton = fourthButton.CookieCutter(new(2.5f, 0f), Vector2.One);

        bool firstHovered = firstButton.Contains(mousePos);
        bool secondHovered = secondButton.Contains(mousePos);
        bool thirdHovered = thirdButton.Contains(mousePos);
        bool fourthHovered = fourthButton.Contains(mousePos);
        bool fifthHovered = fifthButton.Contains(mousePos);

        byte infoState = InfoState;
        InfoState = mousePressed switch
        {
            true when firstHovered => 0,
            true when secondHovered => 1,
            true when thirdHovered => 2,
            true when fourthHovered => 3,
            true when fifthHovered => 4,
            _ => InfoState
        };

        if (infoState != InfoState)
            SoundEngine.PlaySound(SoundID.MenuTick);

        spriteBatch.DrawPatchRectangle(InfoState == 0 ?
            (firstHovered ? QuestAssets.SelectedRectangleHovered : QuestAssets.SelectedRectangle) :
            (firstHovered ? QuestAssets.SimpleRectangleHovered : QuestAssets.SimpleRectangle), firstButton);

        Texture2D texture = ModContent.Request<Texture2D>("Terraria/Images/Item_935").Value;
        spriteBatch.Draw(texture, firstButton.Center(), null, Color.White, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

        spriteBatch.DrawPatchRectangle(InfoState == 1 ?
            (secondHovered ? QuestAssets.SelectedRectangleHovered : QuestAssets.SelectedRectangle) :
            (secondHovered ? QuestAssets.SimpleRectangleHovered : QuestAssets.SimpleRectangle), secondButton);

        texture = ModContent.Request<Texture2D>("Terraria/Images/Item_490").Value;
        spriteBatch.Draw(texture, secondButton.Center(), null, Color.White, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

        spriteBatch.DrawPatchRectangle(InfoState == 2 ?
            (thirdHovered ? QuestAssets.SelectedRectangleHovered : QuestAssets.SelectedRectangle) :
            (thirdHovered ? QuestAssets.SimpleRectangleHovered : QuestAssets.SimpleRectangle), thirdButton);

        texture = ModContent.Request<Texture2D>("Terraria/Images/Item_491").Value;
        spriteBatch.Draw(texture, thirdButton.Center(), null, Color.White, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

        spriteBatch.DrawPatchRectangle(InfoState == 3 ?
            (fourthHovered ? QuestAssets.SelectedRectangleHovered : QuestAssets.SelectedRectangle) :
            (fourthHovered ? QuestAssets.SimpleRectangleHovered : QuestAssets.SimpleRectangle), fourthButton);

        texture = ModContent.Request<Texture2D>("Terraria/Images/Item_489").Value;
        spriteBatch.Draw(texture, fourthButton.Center(), null, Color.White, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

        spriteBatch.DrawPatchRectangle(InfoState == 4 ?
            (fifthHovered ? QuestAssets.SelectedRectangleHovered : QuestAssets.SelectedRectangle) :
            (fifthHovered ? QuestAssets.SimpleRectangleHovered : QuestAssets.SimpleRectangle), fifthButton);

        texture = ModContent.Request<Texture2D>("Terraria/Images/Item_2998").Value;
        spriteBatch.Draw(texture, fifthButton.Center(), null, Color.White, 0f, texture.Size() * 0.5f, 1f, SpriteEffects.None, 0f);

        return false;
    }

    public override void MakeSimpleInfoPage(out string title, out string contents, out Texture2D texture)
    {
        base.MakeSimpleInfoPage(out title, out contents, out texture);
        contents = this.GetLocalization($"Contents{InfoState}").Format(LocalizationArgs);
    }
}