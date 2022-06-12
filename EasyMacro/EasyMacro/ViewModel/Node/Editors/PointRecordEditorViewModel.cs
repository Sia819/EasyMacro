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

        public PointRecordEditorViewModelReactiveObject ReactiveObject { get; }

        private FindWindowPosition findWindowPosition;

        private ValueNodeInputViewModel<string> Windowname;

        public PointRecordEditorViewModel(ValueNodeInputViewModel<string> windowname = null)
        {
            findWindowPosition = new FindWindowPosition("");
            Windowname = windowname;
            ReactiveObject = new PointRecordEditorViewModelReactiveObject();
            ReactiveObject.GetMousePos_Command = ReactiveCommand.Create(GetMousePos_ExcuteCommand);
            ReactiveObject.Editable = false;
            ReactiveObject.Editable = false;

            Value = ReactiveObject.MyPoint = new Point(0, 0);

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
                this.Value = this.ReactiveObject.MyPoint = point;
            }
            else
            {
                findWindowPosition.WindowName = Windowname.Value;
                findWindowPosition.Do();
                this.Value = this.ReactiveObject.MyPoint = new Point(point.x - findWindowPosition.rect.Left, point.y - findWindowPosition.rect.Top);
            }
            HookLib.GlobalMouseKeyHook.StopMouseHook();
            HookLib.GlobalMouseKeyHook.RemoveMouseHotkey(HookLib.GlobalMouseKeyHook.mouse_button.Right);
        }

        public class PointRecordEditorViewModelReactiveObject : ReactiveObject
        {
            #region MyPoint
            public Point MyPoint
            {
                get => myPoint;
                set => this.RaiseAndSetIfChanged(ref myPoint, value);
            }
            private Point myPoint;
            #endregion

            public bool ButtonEnable
            {
                get => buttonEnable;
                set => this.RaiseAndSetIfChanged(ref buttonEnable, value);
            }
            private bool buttonEnable;

            public bool Editable 
            { 
                get => editable;
                set => this.RaiseAndSetIfChanged(ref editable, value);
            }
            private bool editable;

            public ReactiveCommand<Unit, Unit> GetMousePos_Command { get; internal set; }
        }
    }
}
