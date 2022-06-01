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
    public class KeyRecordEditorViewModel : ValueEditorViewModel<Keys>
    {
        static KeyRecordEditorViewModel()
        {   //TODO : KeyRecordEditorView 개발
            Splat.Locator.CurrentMutable.Register(() => new PointRecordEditorView(), typeof(IViewFor<KeyRecordEditorViewModel>));
        }

        public KeyRecordEditorViewModelReactiveObject ReactiveObject { get; }
        public ReactiveCommand<Unit, Unit> GetKeyboardInput_Command { get; }

        public KeyRecordEditorViewModel()
        {
            ReactiveObject = new();
            Value = ReactiveObject.MyKey = Keys.None;
            this.GetKeyboardInput_Command = ReactiveCommand.Create(GetMousePos_ExcuteCommand);
        }

        void GetMousePos_ExcuteCommand()
        {
            HookLib.GlobalKeyboardHook.StartKeyboardHook();
            // 마우스 오른쪽 키가 눌려졌을 때, 등록한 콜백함수가 호출됨.
            //HookLib.GlobalKeyboardHook.KeyboardCallBack(Keys.None);
        }

        private void keyboardCallback(Keys key)
        {
            this.Value = this.ReactiveObject.MyKey = key;

            HookLib.GlobalKeyboardHook.StopKeyboardHook();
        }

        public class KeyRecordEditorViewModelReactiveObject : ReactiveObject
        {
            private Keys myKey;
            public Keys MyKey
            {
                get => myKey;
                set { this.RaiseAndSetIfChanged(ref myKey, value); Console.WriteLine(value); }
            }
        }
    }
}
