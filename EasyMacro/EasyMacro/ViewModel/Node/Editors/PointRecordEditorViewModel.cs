using System;
using System.Drawing;
using System.Reactive;
using EasyMacro.View.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;

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

        public PointRecordEditorViewModel()
        {
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
