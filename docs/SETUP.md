# QuestBooks API Setup

## Adding QuestBooks to Your Mod

Before you can start working with QuestBooks, you first need to add it as either a mod dependency or weak reference to your mod.
  
First, head on over to the [QuestBooks releases tab](https://github.com/bereft-souls/QuestBooks/releases), find the latest release, and download `QuestBooks.dll`.
  
Next, in your mod's source folder, add a folder titled `lib/`, and place `QuestBooks.dll` into this folder.

<img width="656" height="288" alt="image" src="https://github.com/user-attachments/assets/2aad7029-67fa-4194-95b8-1bd77accaeae" />
<img width="629" height="96" alt="image" src="https://github.com/user-attachments/assets/9496286e-13e6-4879-9cf3-c01c9096bc71" /><br/>

In your mod's `build.txt` add either `modReferences = QuestBooks`, or `weakReferences = QuestBooks`. The former will add QuestBooks as a hard dependency, whereas the second will add it as a soft reference.

<img width="628" height="286" alt="image" src="https://github.com/user-attachments/assets/9b1bd74e-eaf5-4021-804d-363b4d7c061c" /><br/>
<img width="232" height="149" alt="image" src="https://github.com/user-attachments/assets/219e56d9-95d8-478f-9a08-6e6cbcc1378c" />

Lastly, open your mod's source code project, and add `QuestBooks.dll` as a project reference.

<img width="587" height="500" alt="image" src="https://github.com/user-attachments/assets/b1d9cda0-be57-43c7-a418-22cb95ac13e4" />
<img width="1062" height="720" alt="image" src="https://github.com/user-attachments/assets/db288bbb-40f8-4800-a7d5-b5867b2e4668" />

Now you're ready to begin adding your own quest lines!

# Usage

You can find documentation on the various aspects of QuestBooks below:
