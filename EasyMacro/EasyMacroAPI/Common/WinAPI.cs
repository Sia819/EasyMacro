using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacroAPI.Common
{
    public class WinAPI
    {
        public const uint LBDOWN = 0x00000002;      // 왼쪽 마우스 버튼 눌림
        public const uint LBUP = 0x00000004;        // 왼쪽 마우스 버튼 떼어짐
        public const uint RBDOWN = 0x00000008;      // 오른쪽 마우스 버튼 눌림
        public const uint RBUP = 0x000000010;       // 오른쪽 마우스 버튼 떼어짐
        public const uint MBDOWN = 0x00000020;      // 휠 버튼 눌림
        public const uint MBUP = 0x000000040;       // 휠 버튼 떼어짐
        public const uint WHEEL = 0x00000800;       // 휠 스크롤


        [System.Runtime.InteropServices.DllImport("user32.dll")] // 입력 제어
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);

        /// <summary>
        /// 커서 위치 제어
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);
    }
}
