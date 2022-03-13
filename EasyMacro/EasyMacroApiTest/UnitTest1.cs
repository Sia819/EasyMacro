using EasyMacroAPI;
using EasyMacroAPI.Command;
using Xunit;

namespace EasyMacroApiTest
{
    public class UnitTest1
    {
        [Fact]
        public void SaveTest()
        {
            MacroManager macroManager;
            macroManager = MacroManager.Instance;
            macroManager.InsertList(new MouseMove(100, 100));
            //macroManager.InsertList(new MouseMove(200, 100));
            //macroManager.InsertList(new MouseMove(300, 100));
            macroManager.DoOnce(0);
        }
    }
}

