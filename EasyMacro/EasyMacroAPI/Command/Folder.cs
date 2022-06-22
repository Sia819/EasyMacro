using System.Collections.Generic;
using EasyMacroAPI.Model;

namespace EasyMacroAPI.Command
{
    class Folder : IAction
    {
        public MacroTypes MacroType => MacroTypes.Folder;

        public string FolderName { get; set; }
        public List<IAction> ActionList { get; set; }
        public IAction Parent { get; set; }

        public Folder(string folderName = "new Folder")
        {
            this.FolderName = folderName;
            ActionList = new List<IAction>();
        }

        public Folder(IAction parent, string folderName = "new Folder")
        {
            this.Parent = parent;
            this.FolderName = folderName;
            ActionList = new List<IAction>();
        }

        public void InsertList(IAction insertAction)
        {
            ActionList.Add(insertAction);
        }

        public void DeleteList(int index)
        {
            ActionList.RemoveAt(index);
        }

        public void DoOnce(int index)
        {
            ActionList[index].Do();
        }

        public void Do()
        {
            for(int i = 0; i < ActionList.Count; i++)
            {
                ActionList[i].Do();
            }
        }
    }
}
