﻿using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class TempletMatchNodeViewModel : CodeGenNodeViewModel
    {
        static TempletMatchNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<TempletMatchNodeViewModel>));
        }

        private static TempletMatch _templetMatch;
        private TempletMatch templetMatch
        {
            get
            {
                if (_templetMatch is null)
                    _templetMatch = new TempletMatch();
                return _templetMatch;
            }
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        /// 
        // private static ImageManagerViewModel _imgMgr;
        // private ImageManagerViewModel imgMgr
        // {
        //     get
        //     {
        //         if (_imgMgr is null)
        //             _imgMgr = new ImageManagerViewModel();
        //         return _imgMgr;
        //     }
        // }

        public ValueNodeInputViewModel<string> BitmapDir { get; }

        public ValueNodeInputViewModel<string> WindowName { get; }

        public ValueNodeInputViewModel<bool?> IsWantKeepFind { get; }

        public ValueNodeInputViewModel<int?> RetryTimes { get; }

        public ValueNodeInputViewModel<int?> Accuracy { get; }

        public ValueNodeInputViewModel<int?> Delay { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueNodeOutputViewModel<Point> ResultPoint { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOutOption1 { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOutOption2 { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        private Action Func()
        {
            Action action = () =>
            {
                if (CodeSimViewModel.Instance.IsRunning || Thread.CurrentThread.IsBackground is false)
                {
                    if (BitmapDir.Value != null)
                    {
                        CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

                        templetMatch.ScreenCapture.WindowName = WindowName.Value;
                        templetMatch.IsWantKeepFinding = IsWantKeepFind.Value ?? false;
                        templetMatch.retryTimes = RetryTimes.Value ?? 0;
                        templetMatch.Accuracy = (double)(Accuracy.Value ?? 80) / 100;
                        templetMatch.SetDelayTime(Delay.Value ?? 1000);

                        templetMatch.Do();
                        Application.Current.Dispatcher.Invoke(() => (ResultPoint.Editor as PointRecordEditorViewModel).Value = templetMatch.FoundPoint); // 멀티스레드에서 UI 수정

                        ValueListNodeInputViewModel<IStatement> selectedFlowout;

                        if (templetMatch.Result)
                        {
                            selectedFlowout = FlowOutOption1;
                        }
                        else
                        {
                            selectedFlowout = FlowOutOption2;
                        }

                        foreach (var a in selectedFlowout.Values.Items)
                        {
                            a.Compile(new CompilerContext());
                        }
                    }
                }
            };
            return action;
        }

        // TODO : 이미지를 EasyMacroAPI의 템플릿매치에 사용될이미지로 지정합니다.
        private Action ImgBindToApi()
        {
            Action action = () =>
            {
                if (String.IsNullOrEmpty((BitmapDir.Editor as ImageManagerSelectorViewModel).Value) is false)
                {
                    // dispose 작업은 ImageManagerSelectorView의 imageSelector_SelectionChanged() 에서 함.
                    templetMatch.TargetImg = (BitmapDir.Editor as ImageManagerSelectorViewModel).SelectedBitmap;
                }
            };
            return action;
        }

        private Action WinnameToApi()
        {
            Action action = () =>
            {
                // TODO : TextBox의 String을 받는 구조 -> 창 찾기 컨트롤의 값을 받는 구조.
                templetMatch.ScreenCapture.WindowName = (this.WindowName.Editor as StringValueEditorViewModel).Value;
            };
            return action;
        }

        public TempletMatchNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "이미지 찾기";

            //TODO : 구조변경
            BitmapDir = new ValueNodeInputViewModel<string>()
            {
                Name = "사진파일 경로",
                Editor = new ImageManagerSelectorViewModel(),
                Port = null,
            };
            BitmapDir.ValueChanged.Select(_ => ImgBindToApi()).Do(action => action.Invoke()).Subscribe();

            this.Inputs.Add(BitmapDir);

            WindowName = new ValueNodeInputViewModel<string>()
            {
                Name = "프로세스 이름",
                Editor = new StringValueEditorViewModel() { },
                Port = null
            };
            (WindowName.Editor as StringValueEditorViewModel).ValueChanged.Select(_ => WinnameToApi());
            this.Inputs.Add(WindowName);


            IsWantKeepFind = new ValueNodeInputViewModel<bool?>()
            {
                Name = "못 찾으면 넘어가지 않기",
                Editor = new CheckBoxEditorViewModel(),
                Port = null,
            };
            this.Inputs.Add(IsWantKeepFind);

            RetryTimes = new ValueNodeInputViewModel<int?>()
            {
                Name = "재시도 횟수",
                Editor = new IntegerValueEditorViewModel(),
                Port = null,
            };
            this.Inputs.Add(RetryTimes);

            Accuracy = new ValueNodeInputViewModel<int?>()
            {
                Name = "비교 정확도",
                Editor = new IntegerValueEditorViewModel(1, 100) { Value = 80 },
                Port = null,
            };
            this.Inputs.Add(Accuracy);

            Delay = new ValueNodeInputViewModel<int?>()
            {
                Name = "지연시간 (1초 = 1000)",
                Editor = new IntegerValueEditorViewModel(1000),
                Port = null,
            };
            this.Inputs.Add(Delay);

            ResultPoint = new CodeGenOutputViewModel<Point>(PortType.Point)
            {
                Name = "Point",
                Editor = new PointRecordEditorViewModel() 
                { 
                    Editable = false,
                    ButtonEnable = false
                }
            };
            this.Outputs.Add(ResultPoint);

            this.RunButton = new ValueNodeInputViewModel<int?>()
            {
                Port = null,

                Editor = new RunButtonViewModel()
                {
                    RunScript = ReactiveCommand.Create
                    (
                        Func(),
                        this.WhenAnyValue(vm => vm.IsCanExcute)
                    )
                }
            };
            this.Inputs.Add(this.RunButton);

            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                Value = this.RunButton.ValueChanged.Select(_ => new NodeCompile(this.Func())
                {
                    Log = Observable.Merge(BitmapDir.ValueChanged.Select(dir => $"TempletMatch - ({dir}), ({this.WindowName.Value})"),
                                           WindowName.ValueChanged.Select(windowname => $"TempletMatch - ({this.BitmapDir.Value}), ({windowname})"))
                })
            };
            this.Outputs.Add(FlowIn);

            FlowOutOption1 = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "이미지 찾기 성공",
                MaxConnections = 1
            };
            this.Inputs.Add(FlowOutOption1);

            FlowOutOption2 = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "이미지 찾기 실패",
                MaxConnections = 1
            };
            this.Inputs.Add(FlowOutOption2);
        }
    }
}
