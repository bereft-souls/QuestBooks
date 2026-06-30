# Adding Quests

> [!IMPORTANT]
> Before adding new quests, make sure you follow the [QuestBooks setup guide](https://github.com/bereft-souls/QuestBooks/blob/master/docs/SETUP.md).

---

> ### Quick Reference:<br/>
> [Quest Types](https://github.com/bereft-souls/QuestBooks/blob/master/docs/ADDING_QUESTS.md#quest-types)<br/>
> [Quest Keys](https://github.com/bereft-souls/QuestBooks/blob/master/docs/ADDING_QUESTS.md#quest-keys)<br/>
> [Localization/Good Practices](https://github.com/bereft-souls/QuestBooks/blob/master/docs/ADDING_QUESTS.md#localization)<br/>
> [Custom Drawing](https://github.com/bereft-souls/QuestBooks/blob/master/docs/ADDING_QUESTS.md#custom-drawing)<br/>
> [Easy Hooks](https://github.com/bereft-souls/QuestBooks/blob/master/docs/ADDING_QUESTS.md#easy-hooks)<br/>
> [Extra Overrides](https://github.com/bereft-souls/QuestBooks/blob/master/docs/ADDING_QUESTS.md#extra-overrides)

---

Adding new quests with QuestBooks is very straightforward.

### Step 1.
Quests are formatted in much the same way that many integrated tModLoader types are. First, create a new class which will represent your quest. It is recommended to place all QuestBooks content into its own designated directory, but this is not required.

<img width="254" height="280" alt="image" src="https://github.com/user-attachments/assets/02eff9fe-3229-4429-ac30-539ed7308ad6" />

### Step 2.
Next, add `using QuestBooks.Quests;` to your imports at the top of the file.

<img width="442" height="323" alt="image" src="https://github.com/user-attachments/assets/b4968c19-12b0-49f5-8ec1-c785984f50b4" />

### Step 3.
Make it so that your new class derives from `Quest`.

<img width="1006" height="514" alt="image" src="https://github.com/user-attachments/assets/965b03b3-e9c7-42c9-b1b7-ae5d1ab03b1e" />

### Step 4.
Implement the `CheckCompletion()` method, which should return true only when your quest has been completed by the player.

<img width="777" height="413" alt="image" src="https://github.com/user-attachments/assets/5cd50a45-13ac-4d47-8c33-656145d2bdd2" />

#### Congratulations, you've added your first quest!
> Of Note:<br/>
> If you have decided to implement QuestBooks as a weakReference rather than a dependency, you will need to tag all of your `Quest` implementations with `[ExtendsFromMod("QuestBooks")]` to prevent the game from crashing when QuestBooks is not present.

While adding new quests is incredibly simple, there are lots of available options to help you tweak and implement your quests in more engaging and intuitive ways.

---

### Quest Types

You may have noticed that some quests are oriented towards the world's state, whereas some are more oriented towards individual players. QuestBooks has integrated functionality for differentiating between these types of quests.

You can override `QuestType` on your quest class to change how your quest is handled. The default is `QuestType.World`.

<img width="802" height="382" alt="image" src="https://github.com/user-attachments/assets/286d3a6a-ce03-4dbf-9d03-f2da54a8a4b8" />

Setting your quest type will change a couple of behaviors:
- Completion tracking and progress tracking is saved to the world's `TagCompound` on world quests, and the player's `TagCompound` on player quests.

- World quests are synced across all clients in multiplayer, whereas player quests are client-individual.

- `Quest.CheckCompletion()` is called on both player and world quests in singleplayer. In multiplayer, world quests will only be checked on the server, and player quests will only be checked on the client.
  - You can still execute quest-specific behavior every frame on both server and client by overriding `Quest.Update()`.
 
---

### Quest Keys

Quests are managed, retrieved, and differentiated by the usage of quest keys.

By default, your quest key is simply the name of your quest class - `this.GetType().Name`.

However, you can change this key if for any reason you need it to be distinct.

<img width="758" height="372" alt="image" src="https://github.com/user-attachments/assets/ef0780bd-5a43-48aa-8cc6-dab02f49e5c7" />

---

### Localization

Quest classes implement both `ModType` and `ILocalizedModType`, meaning you can call any of your familiar localization methods directly from within them.

By default, QuestBooks will search the following localization entries for your quest classes:
- `Mods.{YourMod}.QuestBooks.{Name}.Title`: The title at the top of page in the QuestBooks UI.
- `Mods.{YourMod}.QuestBooks.{Name}.Contents`: The text that displays on the page of your quest in the QuestBooks UI.
- `Mods.{YourMod}.QuestBooks.{Name}.Tooltip`: The text that displays when your quest is unlocked and being hovered in the QuestBooks UI.
- `Mods.{YourMod}.QuestBooks.{Name}.LockedTooltip`: The text that displays when your quest is *not* unlocked and being hovered in the QuestBooks UI.

The only of these two that are actually required are title and contents.
   - If `Tooltip` is not supplied, hover text will not be applied when hovering your quest.
   - If `LockedTooltip` is not supplied, QuestBooks will utilize a default locked tooltip, which in english reads `Complete other quests to unlock!`

> Of Note:<br/>
> `Name` is the property implemented by `ILocalizedModType` that determines your class' content name. By default, it points to `Key`, which is described in the above section on Quest Keys. You can override `Name` to change the localization content name without changing the quest key.

Additionally, you can automatically supply your localization entries with arguments for formatting. By default, only one entry is supplied, which is a color hex code that changes depending on completion status (`14D246` for completed, `FFEE00` for incompleted).

<img width="1166" height="408" alt="image" src="https://github.com/user-attachments/assets/969f2156-cfdf-4b52-a9ae-63e907b1044d" /><br/>

While not strictly tied to localization, quests will also automatically search for an image at `{YourMod}/Assets/Textures/Quests/QuestBooks/{Name}`. If one is found, it will be displayed anchored to the bottom of the quest's info page in the QuestBooks UI.
You can override this by changing your quest's `TextureCategory`.

<img width="781" height="407" alt="image" src="https://github.com/user-attachments/assets/3bbe49bc-7993-4eba-8e76-6e28035c0b3d" /><br/>

#### Good Practices
A common practice to simplify the localizations across your mod's quests and reduce boilerplate code is to create an abstract implementation of `Quest` that overrides the base properties with some new defaults that your mod utilizes.

QuestBooks itself implements this format, and you can see the QuestBooks implementation below:

<img width="827" height="392" alt="image" src="https://github.com/user-attachments/assets/f99c906e-3e8e-4eac-9178-3f491f6e8498" />

---

### Custom Drawing

There are a number of ways that you can achieve custom drawing for your quest.

The first, and most simple, is overriding `MakeSimpleInfoPage`. This will allow you to directly set the text and texture to display on the quest's info page. You can additionally call `base.MakeSimpleInfoPage` within this override to still utilize the base retrieval methods for some of your display properties.<br/>
If you override `MakeSimpleInfoPage`, and do not call the base method, the default localization categories will be ignored.

In a similar manner to this, you can override `HoverTooltip` and `LockedTooltip` for your quest to implement more complex logic than simple localization keys.

<img width="1130" height="569" alt="image" src="https://github.com/user-attachments/assets/57157a12-e72c-445e-bd97-a96b58b90856" /><br/>

The second method is to override `DrawCustomInfoPage`. This method allows you to implement any of your own custom drawing logic. Draw calls here are performed to a special render target that is then drawn to the quest log canvas, so you will not be able to draw beyond the bounds of the page. If you do draw beyond the bounds of the page, the edges of your drawing will be faded to match the rest of the QuestBooks style.

`DrawCustomInfoPage` returns a bool that indicates whether you performed custom drawing. If you return false, indiciating that you did not perform custom draw logic, QuestBooks will then call the default `MakeSimpleInfoPage` retrieval and display logic.

Your draw logic in `DrawCustomInfoPage` does not need to scale with the UI scale, as scaling is already done via matrices and render targets.

Additionally, in the parameters of `DrawCustomInfoPage`, you will find `ref Action updateAction`. Due to QuestBooks' complex rendering system, we have to take a roundabout approach to drawing that does not align with tML's integrated approach. Normally, in tML's UI style, you would be able to set things like mouse text, hover items, etc. directly without issue. However, since our drawing takes place within an odd place in the update/draw loop, we cannot call this logic directly. Instead, we need to assign an `Action` that can then be called at the proper time within the update/draw loop.

The default `MakeSimpleInfoPage` utilizes it in the following way to allow for text snippets to function normally:

<img width="453" height="260" alt="image" src="https://github.com/user-attachments/assets/1fe5c8ae-e1ca-4a72-a9d3-b086f1406e79" />

You can override the above mentioned method like this:

<img width="1045" height="493" alt="image" src="https://github.com/user-attachments/assets/adec6e93-bcf7-419b-98b5-ba20e5fa9309" /><br/>

In addition to how your quest presents its page, you can also change the way your quest renders to the quest line UI.

By overriding `PreTextureDraw`, you can change the drawing logic of your quest to the quest canvas. This is useful for things like world-evil dependent quests, where you may want to present one boss if the world is crimson, and another if the world is corruption.<br/>
`PreTextureDraw` returns a bool that indicates whether you want the default draw logic to be performed. Returning false will prevent the default icon draw, and returning true will draw it as normal.

You can also override `PostTextureDraw` to add some more draw logic on top of the base draw logic. `PostTextureDraw` will be called regardless of whether `PreTextureDraw` returned true or false.

Lastly, you can override `DrawNotification` to change how a notification is drawn when your quest is unlocked for the first time. The default behavior draws a simple exclamation mark in the bottom right-hand corner of the quest icon.

<img width="1296" height="658" alt="image" src="https://github.com/user-attachments/assets/8a428fe6-21e8-46b1-b0a8-679dd4b43dc8" />

---

### Easy Hooks

QuestBooks provides numerous simple, easy to use hooks for common quest completion criteria.

In many cases, you may want a quest to complete when a certain action is performed, rather than when the world reaches a certain state. In events like this, the best approach would usually be to create a new `ModSystem` or entity global that manually completes the quest when a certain trigger is fired. However, implementing many of these system or globals can be incredibly tedious, take up lots of boilerplate, and is generally annoying to have to deal with.

To assist in these cases, QuestBooks provides the following simple hooks, which are all included in the `QuestBooks.Quests.QuestSystems` namespace.
- `BuyItemHook` - triggered when an item is bought from an NPC.
- `CatchFishHook` - triggered when a fish is caught.
- `ChatNPCHook` - triggered when an NPC is talked to.
- `CraftItemHook` - triggered when an item is crafted.
- `KillNPCHook` - triggered when an NPC is killed.
- `KillTileHook` - triggerred when a tile is destroyed.
- `LootChestHook` - triggered when a naturally-generated chest is looted for the first time.
- `PlaceTileHook` - triggered when a tile is placed.

Each of these classes contain a basic implementation which allows you to provide a `Predicate` and `Callback`. The `Callback` is executed when the `Predicate` returns true.

In addition, there are generic overloads of each of these hooks that allow you to pass in a quest type to automatically be completed when the `Predicate` returns true, with numerous constructors that allow for simple pre-defined predicate building.

The available constructors contain overoads for checking item/npc/tile IDs (including modded), checking against content factory bool sets, checking for within a set of IDs, and more.

For example, this is the source code for QuestBooks' "Craft Chlorophye Bars" quest:

<img width="959" height="279" alt="image" src="https://github.com/user-attachments/assets/24eaa42c-ddce-4d30-b8f2-0ae1b947f1cb" /><br/>

By simply defining a type that derives from the `CraftItemHook`, whenever an item is crafted that maches `ItemID.ChlorophyeBar`, the supplemented `CraftChlorophyteBars` quest is automatically completed.

You can pass in modded item IDs, custom predicates/callbacks, really anything you want to into the hook constructors. The hooks are re-instantiated per entity, so the logic you place in the constructor will update in real time as the game is played, meaning you could theoretically create a set of content to check against that dynamically changes while playing.

There is one last hook implementation, which takes in 2 generics instead of one. This allows you to easily specify modded content to check against, without having to type a custom predicate or content set for checking.
```cs
public sealed class CraftModdedItemHook() : CraftItemHook<CraftModdedItemQuest, ModdedItem>();
```

In the event that you do need to supply custom predicates, the `QuestBooks.Utilities.Utils` class contains various `Match` methods which can be used in place of having to type out lambda functions.

This is the code for QuestBooks' Daytime EoL quest:

<img width="971" height="442" alt="image" src="https://github.com/user-attachments/assets/433a78a8-ad52-4b7c-a035-f41966893e73" />

---

### Extra Overrides

There are a couple other methods on quests that you can override to implement custom behavior:
- `void Update()` - called once every frame on every quest, regardless of client vs. server<br/>
- `void OnCompletion()` - called when the quest is completed by the player. This is not called if the quest was completed before the player loads into the world.<br/>
- `void MarkAsComplete()` - called when the quest is marked as complete. This is called even if the quest was completed before the player loads in the world.<br/>
- `void SaveProgress(TagCompound tag)` - called when the player exits the world. The `TagCompound` will belong to the world if `quest.QuestType == QuestType.World`, and the player for `QuestType.Player`. Allows you to save quest progress.<br/>
- `void LoadProgress(TagCompound tag)` - called when the player enters the world. The `TagCompound` will belong to the world if `quest.QuestType == QuestType.World`, and the player for `QuestType.Player`. Allows you to load quest progress.

Note that the progress saving methods share their `TagCompound`s globally with all other quests, so ensure that the key you use is distinct.

---

## Up Next:
Creating custom quest logs<br/>
Creating global quest books<br/>
Building your quest lines<br/>
