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
                    _templetMatch = new TempletMatch("");
                return _templetMatch;
            }
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        
        private Bitmap targetBitmap { get; }

        public ValueNodeInputViewModel<string> BitmapDir { get; }

        public ValueNodeInputViewModel<string> WindowName { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

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

        public TempletMatchNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "TempletMatch";
            //TODO : 구조변경
            BitmapDir = new ValueNodeInputViewModel<string>()
            {
                Name = "BitmapDir",
                Editor = new StringValueEditorViewModel(),
                Port = null,
            };
            this.Inputs.Add(BitmapDir);

            WindowName = new ValueNodeInputViewModel<string>()
            {
                Name = "WindowName",
                Editor = new StringValueEditorViewModel(),
                Port = null
            };
            this.Inputs.Add(WindowName);

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
        }
    }
}
