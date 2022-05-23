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

                //templetMatch.Text = Input.Value;
                templetMatch.Do();

                foreach (var a in FlowOut.Values.Items)
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
            base.Name = "TempletMatch";
            //TODO : 구조변경
            BitmapDir = new ValueNodeInputViewModel<string>()
            {
                Name = "BitmapDir",
                Editor = dirEditor,
                Port = null,
            };
            this.Inputs.Add(BitmapDir);

            dirEditor.ValueChanged.Select(_ => ImgBindToApi());

            WindowName = new ValueNodeInputViewModel<string>()
            {
                Name = "WindowName",
                Editor = winnameEditor,
                Port = null
            };
            this.Inputs.Add(WindowName);

            winnameEditor.ValueChanged.Select(_ => winnameToApi());

            this.RunButton = new ValueNodeInputViewModel<int?>()
            {
                Port = null,
                Name = "Run",
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
                Name = "TempletMatch 성공",
            };
            this.Inputs.Add(FlowOutOption1);
            FlowOutOption2 = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "TempletMatch 실패",
            };
            this.Inputs.Add(FlowOutOption2);
        }
    }
}
