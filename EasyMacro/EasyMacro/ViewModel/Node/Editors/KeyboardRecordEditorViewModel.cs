using System;
using System.Drawing;
using System.Reactive;
using EasyMacro.View.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using EasyMacroAPI.Command;
using EasyMacroAPI.Model;

namespace EasyMacro.ViewModel.Node.Editors
{
    public class KeyboardRecordEditorViewModel : ValueEditorViewModel<Keys>
    {
        static KeyboardRecordEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new KeyboardRecordEditorView(), typeof(IViewFor<KeyboardRecordEditorViewModel>));
        }

        public PointRecordEditorViewModelReactiveObject ReactiveObject { get; }
        public ReactiveCommand<Unit, Unit> GetKeyFormHook_Command { get; }

        public KeyboardRecordEditorViewModel()
        {
            ReactiveObject = new();
            this.ReactiveObject.MyKey = "Click to record";
            this.GetKeyFormHook_Command = ReactiveCommand.Create(GetKeyFormHook_ExcuteCommand);
        }

        void GetKeyFormHook_ExcuteCommand()
        {
            this.ReactiveObject.MyKey = "[ Recording... ]";
            HookLib.GlobalKeyboardHook.StartKeyboardHook();
            HookLib.GlobalKeyboardHook.AddKeyboardHotkey(EasyMacroAPI.Model.Keys.AnyKey, EasyMacroAPI.Model.KeyModifiers.None, HookCallback);
        }

        public void HookCallback(EasyMacroAPI.Model.Keys keys, EasyMacroAPI.Model.KeyModifiers keyModifiers)
        {
            this.Value = keys;
            this.ReactiveObject.MyKey = keys.ToString();
            HookLib.GlobalKeyboardHook.RemoveKeyboardHotkey(EasyMacroAPI.Model.Keys.AnyKey, keyModifiers);
        }

        public class PointRecordEditorViewModelReactiveObject : ReactiveObject
        {
            private string myKey;
            public string MyKey
            {
                get => myKey;
                set { this.RaiseAndSetIfChanged(ref myKey, value); }
            }
        }
    }
}
