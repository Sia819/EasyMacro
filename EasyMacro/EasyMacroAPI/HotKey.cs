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
using EasyMacroAPI.Common;

namespace EasyMacroAPI
{
    internal class HotKey : Form
    {
        const int HOTKEY_ID = 31197; //Any number to use to identify the hotkey instance

        const int WM_HOTKEY = 0x0312;
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WM_HOTKEY:
                    Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF);
                    Model.KeyModifiers modifier = (Model.KeyModifiers)((int)message.LParam & 0xFFFF);
                    //MessageBox.Show("HotKey Pressed :" + modifier.ToString() + " " + key.ToString());

                    if (Model.KeyModifiers.None == modifier && Keys.F9 == key)
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
            WinAPI.RegisterHotKey(this.Handle, HOTKEY_ID, KeyModifiers.None, Keys.F9);
        }

        public void UnregisterHotKey()
        {
            //핫키 해제
            WinAPI.UnregisterHotKey(this.Handle, HOTKEY_ID);
        }
    }
}
