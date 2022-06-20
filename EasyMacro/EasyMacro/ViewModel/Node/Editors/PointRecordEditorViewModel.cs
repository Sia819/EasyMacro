using System;
using System.Drawing;
using System.Reactive;
using EasyMacro.View.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using EasyMacroAPI.Command;

namespace EasyMacro.ViewModel.Node.Editors
{
    public class PointRecordEditorViewModel : ValueEditorViewModel<Point>
    {
        static PointRecordEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new PointRecordEditorView(), typeof(IViewFor<PointRecordEditorViewModel>));
        }

        #region ButtonEnable Property
        public bool ButtonEnable
        {
            get => buttonEnable;
            set => this.RaiseAndSetIfChanged(ref buttonEnable, value);
        }
        private bool buttonEnable;
        #endregion ButtonEnable Property

        #region Editable Property
        public bool Editable
        {
            get => editable;
            set => this.RaiseAndSetIfChanged(ref editable, value);
        }
        private bool editable;
        #endregion

        public ReactiveCommand<Unit, Unit> GetMousePos_Command { get; internal set; }


        private FindWindowPosition findWindowPosition;

        private ValueNodeInputViewModel<string> Windowname;

        public PointRecordEditorViewModel(ValueNodeInputViewModel<string> windowname = null)
        {
            findWindowPosition = new FindWindowPosition("");
            Windowname = windowname;
            GetMousePos_Command = ReactiveCommand.Create(GetMousePos_ExcuteCommand);
            ButtonEnable = true;
            Editable = true;

            Value = new Point(0, 0);
        }

        void GetMousePos_ExcuteCommand()
        {
            HookLib.GlobalMouseKeyHook.StartMouseHook();
            // 마우스 오른쪽 키가 눌려졌을 때, 등록한 콜백함수가 호출됨.
            HookLib.GlobalMouseKeyHook.AddMouseHotkey(HookLib.GlobalMouseKeyHook.mouse_button.Right, 
                                                      new HookLib.GlobalMouseKeyHook.MouseHotkeyDelegate(mouseCallback));
        }

        private void mouseCallback(PInvoke.POINT point)
        {
            if (Windowname is null || String.IsNullOrEmpty(Windowname.Value))
            {
                this.Value = point;
            }
            else
            {
                findWindowPosition.WindowName = Windowname.Value;
                findWindowPosition.Do();
                this.Value = new Point(point.x - findWindowPosition.rect.Left, point.y - findWindowPosition.rect.Top);
            }
            
            HookLib.GlobalMouseKeyHook.RemoveMouseHotkey(HookLib.GlobalMouseKeyHook.mouse_button.Right);
        }

    }
}
