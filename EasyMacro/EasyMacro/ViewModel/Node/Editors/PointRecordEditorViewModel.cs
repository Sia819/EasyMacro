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
        public ReactiveCommand<Unit, Unit> GetMousePos_Command { get; }

        private FindWindowPosition findWindowPosition;

        private ValueNodeInputViewModel<string> Windowname;
        private IntegerValueEditorViewModel X;
        private IntegerValueEditorViewModel Y;

        public PointRecordEditorViewModel(IntegerValueEditorViewModel X, IntegerValueEditorViewModel Y, ValueNodeInputViewModel<string> windowname = null)
        {
            this.X = X;
            this.Y = Y;
            findWindowPosition = new FindWindowPosition("");
            Windowname = windowname;
            ReactiveObject = new();
            Value = ReactiveObject.MyPoint = new Point(0, 0);
            this.GetMousePos_Command = ReactiveCommand.Create(GetMousePos_ExcuteCommand);
        }

        void GetMousePos_ExcuteCommand()
        {
            HookLib.GlobalMouseKeyHook.StartMouseHook();
            // 마우스 오른쪽 키가 눌려졌을 때, 등록한 콜백함수가 호출됨.
            HookLib.GlobalMouseKeyHook.RegisterMouseHotkey(HookLib.GlobalMouseKeyHook.mouse_status.Right,
                                                            new HookLib.GlobalMouseKeyHook.HotkeyDelegate(mouseCallback));
        }

        private void mouseCallback(PInvoke.POINT point)
        {
            this.Value = this.ReactiveObject.MyPoint = point;
            if(Windowname is not null && Windowname.Value != "")
            {
                findWindowPosition.WindowName = Windowname.Value;
                findWindowPosition.Do();
                this.X.Value = point.x - findWindowPosition.rect.Left;
                this.Y.Value = point.y - findWindowPosition.rect.Top;
            }
            else
            {
                this.X.Value = point.x;
                this.Y.Value = point.y;
            }
            HookLib.GlobalMouseKeyHook.StopMouseHook();
            HookLib.GlobalMouseKeyHook.UnregisterMouseHotkey(HookLib.GlobalMouseKeyHook.mouse_status.Right);
        }

        public class PointRecordEditorViewModelReactiveObject : ReactiveObject
        {
            private Point myPoint;
            public Point MyPoint
            {
                get => myPoint;
                set { this.RaiseAndSetIfChanged(ref myPoint, value); Console.WriteLine(value); }
            }
        }
    }
}
