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
        public IAction parent;

        public Folder(string folderName = "new Folder")
        {
            this.folderName = folderName;
            actionList = new List<IAction>();
        }

        public Folder(IAction parent, string folderName = "new Folder")
        {
            this.parent = parent;
            this.folderName = folderName;
            actionList = new List<IAction>();
        }

        public void InsertList(IAction insertAction)
        {
            actionList.Add(insertAction);
        }

        public void DeleteList(int index)
        {
            actionList.RemoveAt(index);
        }

        public void DoOnce(int index)
        {
            actionList[index].Do();
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
