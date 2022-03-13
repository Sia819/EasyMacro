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
    class HotKey : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        //public HotKey()
        //{
        //    
        //}

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
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WM_HOTKEY:
                    Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF);
                    KeyModifiers modifier = (KeyModifiers)((int)message.LParam & 0xFFFF);
                    //MessageBox.Show("HotKey Pressed :" + modifier.ToString() + " " + key.ToString());

                    if (KeyModifiers.None == modifier && Keys.F9 == key)
                    {
                        MacroManager.Instance.StopMacro();
                        MessageBox.Show(new Form { TopMost = true }, "HotKey Pressed!");
                    }

                    break;
            }
            base.WndProc(ref message);
        }

        public void RegisterHotKey()
        {
            // 핫키 등록
            //RegisterHotKey((IntPtr)handle, HOTKEY_ID, KeyModifiers.Control | KeyModifiers.Shift, Keys.N);
            RegisterHotKey(this.Handle, HOTKEY_ID, KeyModifiers.None, Keys.F9);
        }

        public void UnregisterHotKey()
        {
            //핫키 해제
            UnregisterHotKey(this.Handle, HOTKEY_ID);
        }
    }
}
