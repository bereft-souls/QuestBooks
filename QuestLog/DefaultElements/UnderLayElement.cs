using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestBooks.QuestLog.DefaultElements
{
    [ElementTooltip("UnderlayElement")]
    public class UnderlayElement : OverlayElement
    {
        public override float DrawPriority => 0.1f;
    }
}
