# Other Uses

You may find that you want to reference the QuestBooks logic outside of the quest log itself. To do this, the `QuestBooksMod` class contains a public-facing API which allows easy access to all of QuestBooks' relevant data.<br/>
Some of this data is also available via the use of mod calls.

---
## GetQuest
`void QuestBooksMod.GetQuest()` allows you to retrieve the active instance for a given quest type. Note that quests are only active in the world, and attempting to call this outside of a world may result in an error being thrown. If you need this to be safe, see TryGetQuest instead.

#### Overloads
- `QuestBooksMod.GetQuest(string questName)` - retrieves the active quest instance with the specified `questName` as its key.
- `QuestBooksMod.GetQuest<T>()` - retrieves the active quest instance of the specified quest type `T`.

### Example
```cs
var bloodMoonQuest = QuestBooksMod.GetQuest<BloodMoonDefeated>();

if (!bloodMoonQuest.Completed)
  return;

// Some other logic here
```

---
## TryGetQuest
`bool QuestBooksMod.TryGetQuest()` allows you to safely retrieve the active instance for a given quest type. If the method succeeds, it will return true, and the `out` variable will contain the active quest instance. If it fails, it will return false.

#### Overloads
- `QuestBooksMod.TryGetQuest(string questName, out Quest result)` - safely attempts to retrieve the active quest instance with the specified `questName` as its key.
- `QuestBooksMod.TryGetQuest<T>(out Quest result)` - safely attempts to retrieve the active quest instance of the specified quest type `T`.

### Example
```cs
// public override void GlobalTile.KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)

// This can be called outside of a world, so we use safe retrieval here
if (QuestBooksMod.TryGetQuest<Break10kTiles>(out var quest))
  quest.TileBroken++;
```

---
## CompleteQuest
`void CompleteQuest()` allows you to manually trigger the completion of a quest. Note that this method will error if it is called outside of a world.

#### Overloads
- `QuestBooksMod.CompleteQuest(string questName)` - Manually completes the quest with the specified `questName` as its key.
- `QuestBooksMod.CompleteQuest<T>()` - Manually completes the quest of the specified quest type `T`.
- `QuestBooksMod.CompleteQuest(Quest quest)` - Manually completes the specified quest.

### Example
```cs
// public override void GlobalNPC.OnKill(NPC npc)

if (npc.type != NPCID.BloodNautilus)
  return;

QuestBooksMod.CompleteQuest<DreadnautilusDefeated>();
```

---
## MarkComplete
`void MarkComplete()` allows you to mark a quest as completed without firing and of the quest's `OnComplete` methods. This is useful for updating the state of a quest that was previously completed, but did not trigger its completion for any reason (i.e. code updated, QuestBooks unloaded). Note that this method will error if it is called outside of a world.

#### Overloads
- `QuestBooksMod.MarkComplete(string questName)` - Marks the quest with the specified `questName` as its key as completed.
- `QuestBooksMod.MarkComplete<T>()` - Marks the quest of the specified quest type `T` as completed.
- `QuestBooksMod.MarkComplete(Quest quest)` - Marks the specified quest as completed.

### Example
```cs
// public override void ModSystem.PostUpdateTime()

if (customQuestShouldBeMarkedComplete && QuestBooksMod.TryGetQuest<CustomQuest>(out var quest) && !quest.Completed)
  QuestBooksMod.MarkComplete(quest);
```

---
## MarkIncomplete
`void MarkIncomplete()` allows you to reset a quest's completion status. Note that this method will error if it is called outside of a world

#### Overloads
- `QuestBooksMod.MarkIncomplete(string questName)` - Resets the completion of the quest with the specified `questName` as its key.
- `QuestBooksMod.MarkIncomplete<T>()` - Resets the completion of the quest of the specified quest type `T`.
- `QuestBooksMod.MarkIncomplete(Quest quest)` - Resets the completion of the specified quest.

### Example
```cs
// I have no idea where this could be useful but it probably is somewhere
if (customQuestShouldReset)
  QuestBooksMod.MarkIncomplete<CustomQuest>();
```

---
# Mod Calls

All of the above mentioned methods are available via the use of mod calls.<br/>
QuestBooks makes use of a uniquely powerful tool to allow access to typed members without the use of strong typing.

Wherever you see `DynamicDict(object)`, the returned object can be cast into an `IDictionary<string, object>` in which all values can also be cast into an `IDictionary<string, object>`. You can use this dictionary structure to automatically get, set, and invoke any field, property, or method on the object. This can be chained for members that return typed objects.

For example:
```cs
var quest = (IDictionary<string, object>)questBooks.Call("GetQuest", "Break10kTiles");

// Checks quest completion
bool completed = (bool)quest["Completed"];

// Sets the number of broken tiles on the quest
quest["TilesBroken"] = 10;

// Place this line outside the method:
// public delegate object MethodDelegate(object[] args);
bool checkComplete = (bool)((MethodDelegate)quest(null));
```
