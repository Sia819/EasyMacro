using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    class Folder : IAction
    {
        public MacroTypes MacroType => MacroTypes.Folder;

        public string folderName;
        public List<IAction> actionList;


        public Folder(string folderName = "new Folder")
        {
            this.folderName = folderName;
            actionList = new List<IAction>();
        }

        public void Do()
        {
            for(int i = 0; i < actionList.Count; i++)
            {
                actionList[i].Do();
            }
        }
    }
}
