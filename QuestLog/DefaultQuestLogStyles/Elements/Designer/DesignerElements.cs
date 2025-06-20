using FullSerializer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using QuestBooks.Assets;
using QuestBooks.QuestLog.DefaultQuestBooks;
using QuestBooks.QuestLog.DefaultQuestLines;
using QuestBooks.Systems;
using ReLogic.Graphics;
using SDL2;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities.FileBrowser;
using static System.Net.Mime.MediaTypeNames;

namespace QuestBooks.QuestLog.DefaultQuestLogStyles
{
    public partial class BasicQuestLogStyle
    {
        private void UpdateDesigner(Rectangle books, Rectangle chapters, Rectangle questArea)
        {
            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, CustomBlendState, SamplerState.LinearClamp, CustomDepthStencilState, CustomRasterizerState);
            });

            HandleRenaming(books, chapters, questArea);
            HandleAddDeleteButtons(books, chapters, questArea);
            HandleSaveLoadButtons();
            HandleTypeSelection();

            DrawTasks.Add(sb =>
            {
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, CustomBlendState, CustomSamplerState, CustomDepthStencilState, CustomRasterizerState);
            });
        }
    }
}
