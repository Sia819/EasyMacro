using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;
using System;
using System.Drawing;
using System.Reactive.Linq;

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
        private static ImageManagerViewModel _imgMgr;
        private ImageManagerViewModel imgMgr
        {
            get
            {
                if (_imgMgr is null)
                    _imgMgr = new ImageManagerViewModel();
                return _imgMgr;
            }
        }

        public ValueNodeInputViewModel<string> BitmapDir { get; }

        public StringValueEditorViewModel dirEditor = new StringValueEditorViewModel();

        public ValueNodeInputViewModel<string> WindowName { get; }

        public StringValueEditorViewModel winnameEditor = new StringValueEditorViewModel();

        public ValueNodeInputViewModel<bool?> IsWantKeepFind { get; }

        public ValueNodeInputViewModel<int?> RetryTimes { get; }

        public ValueNodeInputViewModel<int?> Accuracy { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOutOption1 { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOutOption2 { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        Action Func()
        {
            Action action = () =>
            {
                CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

                templetMatch.TargetImg = new Bitmap(BitmapDir.Value);
                templetMatch.screenCapture.WindowName = WindowName.Value;
                templetMatch.isWantKeepFinding = IsWantKeepFind.Value ?? false;
                templetMatch.retryTimes = RetryTimes.Value ?? 0;
                templetMatch.accuracy = (Accuracy.Value ?? 80) / 100;
                
                templetMatch.Do();

                ValueListNodeInputViewModel<IStatement> selectedFlowout;

                if (templetMatch.result)
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
            };
            return action;
        }

        Action ImgBindToApi()
        {
            Action action = () =>
            {
                templetMatch.TargetImg = imgMgr.CopyImg(dirEditor.Value);
            };
            return action;
        }

        Action winnameToApi()
        {
            Action action = () =>
            {
                templetMatch.screenCapture.WindowName = winnameEditor.Value;
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
                Editor = dirEditor,
                Port = null,
            };
            this.Inputs.Add(BitmapDir);

            dirEditor.ValueChanged.Select(_ => ImgBindToApi());

            WindowName = new ValueNodeInputViewModel<string>()
            {
                Name = "프로세스 이름",
                Editor = winnameEditor,
                Port = null
            };
            this.Inputs.Add(WindowName);

            winnameEditor.ValueChanged.Select(_ => winnameToApi());

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
                Editor = new IntegerValueEditorViewModel(1,100),
                Port = null,
            };
            this.Inputs.Add(Accuracy);

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

            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
            };
            this.Inputs.Add(FlowOut);

            FlowOutOption1 = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "이미지 찾기 성공",
            };
            this.Inputs.Add(FlowOutOption1);
            
            FlowOutOption2 = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "이미지 찾기 실패",
            };
            this.Inputs.Add(FlowOutOption2);
        }
    }
}
