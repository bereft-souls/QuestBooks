# Making Custom Quest Lines

> [!IMPORTANT]
> Before adding new quests, make sure you follow the [QuestBooks setup guide](https://github.com/bereft-souls/QuestBooks/blob/master/docs/SETUP.md).

---

> ### Quick Reference:<br/>
> [Creating Global Quest Books](https://github.com/bereft-souls/QuestBooks/blob/master/docs/QUESTLINES.md#creating-global-quest-books)<br/>
> [Creating Custom Book/Chapter Types](https://github.com/bereft-souls/QuestBooks/blob/master/docs/QUESTLINES.md#creating-custom-quest-bookchapter-types)<br/>
> [Spicing Up Your Log](https://github.com/bereft-souls/QuestBooks/blob/master/docs/QUESTLINES.md#spicing-up-your-log)<br/>
> [Using the Designer](https://github.com/bereft-souls/QuestBooks/blob/master/docs/QUESTLINES.md#using-the-designer)

---

## Creating a new Quest Log

The first step to making your custom quest log is to enable the QuestBooks designer from your mod's code.

### Step 1.
Start by creating a new `ModSystem` class. This class will be where all of our custom quest line logic is contained.
> Of Note:<br/>
> If you have added QuestBooks as a weak reference instead of a dependency, you will need to mark this class with `JITWhenModsEnabled("QuestBooks")`<br/>

<img width="511" height="241" alt="image" src="https://github.com/user-attachments/assets/f6d32867-4ad7-46f9-9a22-ba7fc3331490" /><br/>

### Step 2.
Next, override `PostSetupContent()`, and call `QuestBooksMod.EnableDesigner` by passing in your mod system's `Mod` property.<br/>
Remember to remove or comment out this line of code before publishing your mod to the workshop.<br/>
<img width="753" height="344" alt="image" src="https://github.com/user-attachments/assets/303cf6e1-0b50-4d2e-a62e-16abee0aef6c" />

### Step 3.
You should now be able to utilize the QuestBooks' in-game designer UI. The first thing we need to do in this UI is to export a copy of the vanilla quest log that we can utilize as a base.<br/>
Head in-game, toggle the designer with the button at the top right of the UI, and choose the "Export Quest Log" button.<br/>
Save the file somewhere within your mod's source code.<br/>
<img width="600" height="336" alt="41cf931615a4e9754ca9b7bc5ae034f1" src="https://github.com/user-attachments/assets/eee06cd2-b21d-4b92-bc44-a965394f1f17" /><br/>

### Step 4.
> [!IMPORTANT]
> If you want your quest books to display inside of other logs as opposed to being it's own quest log, see the section on [Creating Global Quest Books](https://github.com/bereft-souls/QuestBooks/blob/master/docs/QUESTLINES.md#creating-global-quest-books).

Now that you've exported a copy of the vanilla quest log, head back to your mod system code. In the same `PostSetupContent()` method, you will first want to read the text from the file within your source code.
After reading the file text, call `QuestBooksMod.AddQuestLog()`. The first argument, `questLogKey`, should be a unique identifier that will be associated with your quest log. A good idea is to save this key as a static/const property that can be re-used.<br/>
The second should be your decoded text, and the third should be your mod system's `Mod` property.<br/>
<img width="770" height="476" alt="image" src="https://github.com/user-attachments/assets/70e98fb8-cd70-467f-82c1-42d97c88a18a" /><br/>

### Step 5.
A common practice for third-party quest log implementations is to replace the vanilla quest lines, as your mod's content may contain changes that render the default quest log inaccurate.<br/>
In order to disable the vanilla quest log, you can add the following line. If you do not want to disable the vanilla quest log, players will be able to switch between the vanilla log and your log on the cover of the book.<br/>
If you are making a mod pack, you will likely also want to disable any quest lines that come from the mods in your mod pack. You can replace `"Terraria"` with the quest keys of your other mods' quest lines to disable them as well.
<img width="783" height="531" alt="image" src="https://github.com/user-attachments/assets/07a0b59c-90c4-4147-a7a5-6674bef47523" /><br/>

#### Now you're ready to start designing!
If you head back in-game, you should see that... nothing has changed at first. Toggle the designer, make some changes, and then save your quest log to the same file location that you originally exported the vanilla copy to. If you re-build your mod and reload the game, you should see your changes appear!

---

### Creating Global Quest Books

If your mod is not a content mod, and is instead a mod intended to be used alongside other mods (i.e. Magic Storage, Recipe Browser), you may want to consider registering global books instead of a custom quest log. Global quest books are quest books that appear inside of every other quest log, instead of taking up their own dedicated quest log slot.<br/>
To add global quest books, first follow steps 1-3. Read your file text just like the first part of step 4, but instead of calling `QuestBooksMod.AddQuestLog`, call `QuestBooksMod.AddGlobalQuestBooks`.<br/>
<img width="769" height="478" alt="image" src="https://github.com/user-attachments/assets/a168d780-3c1d-4336-ac01-503feef1108f" /><br/>

You can now follow the rest of step 4 and 5 as normal.

> [!IMPORTANT]
> Remember to **remove all quest books** except for your global books before exporting from the in game designer!<br/>
> Failure to remove other books will cause duplicates to be registered.<br/>
> If you want to make this process easier for yourself, you can immediately choose "Load Quest Log" in the designer when opening the game, and then opening your quest log file. This will clear the log and leave on your global book(s).

### Creating Custom Quest Book/Chapter Types

One way to make your quest log feel more engaging is by adding dynamic quest book/chapter types. QuestBooks utilizes this to add chapters that only unlock after reaching a certain stage in progression. You can even hide chapters from the log completely! Start by creating a new class that derives from `QuestBook` or `QuestChapter`. If you want to utilize the vanilla quest book/chapter drawing styles, you can instead inherit from `TabBook` and `ScrollChapter` respectively.
<img width="776" height="364" alt="image" src="https://github.com/user-attachments/assets/e8f10d30-20e4-4a4a-81f8-ee0c2c1de5de" /><br/>

Within these classes, you can override `DisplayName`, `IsUnlocked()`, and `VisibleInLog()` to change how your element behaves.<br/>
`DisplayName` controls the text that displays on-screen.<br/>
`IsUnlocked()` determines whether your element can be clicked on/selected.<br/>
`VisibleInLog()` determines whether your element should display in the log. Note that elements will always display in the log if the designer is toggled.<br/>
<img width="781" height="572" alt="image" src="https://github.com/user-attachments/assets/1738ab8a-c46d-4e33-8619-7e7faf9f7cbf" />

### Spicing Up Your Log

Another way to customize your quest log is to register overrides that draw your mod's title and a custom book cover.<br/>
Your mod's title displays at the top of the book cover when your log is selected, or when the user is choosing between quest logs.<br/>
Your custom book cover only displays when your quest log is selected.<br/>
The title draw delegate is supplied with text representative of your mod's quest log name. By default, this text is the localization entry at `Mods.{YourMod}.QuestBooks.{questLogKey}.Name`, but you can override this as well.
<img width="1312" height="847" alt="image" src="https://github.com/user-attachments/assets/3be4159b-3706-4e09-ac27-e6a38aaef894" />

### Using the Designer

Using the in-game designer should be fairly straight forward. All of the UI buttons have hover text that tell you what they do, as well as integrated undo/redo support with [CTRL+Z] and [CTRL+Y]/[CTRL+SHIFT+Z]. You can move selected elements with the arrow keys, and holding shift while pressing arrows keys will instead move them by the active grid size.

There are some things to be aware of, however. One of the best parts of QuestBooks is its connections between elements, which act not only as visual bridges for users, but as functional bridges for mod developers. **Before you can place connections onto your canvas, you must make sure that all other elements are deselected**.

You will notice that many elements contain fields like `UnlockFeeds` and `DisplayFeeds`. These fields read the number of incoming "active connections" from other elements to change their element's behavior. Quest displays that have their quests completed will send out "active connections", whereas incompleted quests do not. Connections will display their signal direction in the designer. Additionally, `ConnectorPoint` elements can be used not only to redirect connections, but to both join and split signals as well. `ConnectorPoint`s contain a field titled `RequiredFeeds`. If connector points have incoming feeds greater than or equal to `RequiredFeeds`, they will also emit an active signal.

Via the use of splitting and joining connections, in conjunction with the `HiddenQuest` element to implement signal logic that is not visible outside of the designer, you should be able to create virtually any logic that you want for displaying within your chapters.

## If you are having trouble or have questions about how to achieve your desired end result, please do not hesitate to reach out in the [#questbooks-help](https://discord.com/channels/1418289634382712996/1418298220521590906) channel in the [Bereft Souls Discord](https://discord.gg/BPFyqZsnn8), and we will be happy to help - it's what we're paid to do!
