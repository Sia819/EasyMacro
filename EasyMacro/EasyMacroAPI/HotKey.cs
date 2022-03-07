using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EasyMacroAPI
{
    class HotKey
    {
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        int handle;

        public HotKey()
        {
            handle = FindWindow(null, "MainWindow");
        }

        const int HOTKEY_ID = 31197; //Any number to use to identify the hotkey instance

        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        const int WM_HOTKEY = 0x0312;

        private void button1_Click(object sender, EventArgs e)
        {
            // 핫키 등록
            //RegisterHotKey((IntPtr)handle, HOTKEY_ID, KeyModifiers.Control | KeyModifiers.Shift, Keys.N);
            RegisterHotKey((IntPtr)handle, HOTKEY_ID, KeyModifiers.None, Keys.F9);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //핫키 해제
            UnregisterHotKey((IntPtr)handle, HOTKEY_ID);
        }
    }
}
