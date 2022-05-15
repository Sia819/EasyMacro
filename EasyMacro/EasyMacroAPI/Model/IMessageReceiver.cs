using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacroAPI.Model
{
    public interface IMessageReceiver
    {
        public bool IsConfigured { get; }

        public delegate void HotkeyDelegate();

        public void AddHotkey(Keys keys, KeyModifiers keyModifiers, HotkeyDelegate hotkeyDelegate);

        public void RemoveHotkey(Keys keys, KeyModifiers keyModifiers);
    }
}
