using System;
using EasyMacroAPI;
using EasyMacroAPI.Command;

namespace EasyMacroConsoleTest
{
    public class Program
    {
        /// <summary>
        /// 테스트 할 코드 입니다.
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("EasyMacroConsoleTest 프로젝트의 Main에 진입했습니다.");
            Program program = new();


            program.SaveTest();
        }

        public void SaveTest()
        {
            Console.WriteLine("EasyMacroConsoleTest 프로젝트의 SaveTest에 진입했습니다.");

            MacroManager macroManager = MacroManager.Instance;

            macroManager.InsertList(new MouseMove(111, 111));
            macroManager.InsertList(new Delay(1000));
            macroManager.InsertList(new MouseMove(222, 222));
            macroManager.InsertList(new Delay(2000));

            macroManager.SaveData();
            macroManager.LoadData();

            //macroManager.StartMacro();
            macroManager.DoOnce(0);
        }

        public void ClickTest()
        {
           

            MacroManager macroManager = MacroManager.Instance;

            macroManager.InsertList(new MouseClick(111, 111));
           

            macroManager.SaveData();
            macroManager.LoadData();

            //macroManager.StartMacro();
            macroManager.DoOnce(0);
        }
    }
}
