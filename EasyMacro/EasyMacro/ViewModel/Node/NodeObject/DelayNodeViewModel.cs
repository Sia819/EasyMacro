﻿using DynamicData;
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
using System.Reactive.Linq;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class DelayNodeViewModel : CodeGenNodeViewModel
    {
        static DelayNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<DelayNodeViewModel>));
        }

        private static Delay _delay;
        private Delay delay
        {
            get
            {
                if (_delay is null)
                    _delay = new Delay(0);
                return _delay;
            }
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        public ValueNodeInputViewModel<int?> Input { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }
        
        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;
        
        Action Func()
        {
            Action action = () =>
            {
                CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

                delay.Time = (int)Input.Value < 0 ? 0 : (int)Input.Value;
                delay.Do();

                foreach (var a in FlowOut.Values.Items)
                {
                    a.Compile(new CompilerContext());
                }
            };
            return action;
        }

        public DelayNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "지연시키기";

            //TODO : 나머지 구현
            Input = new ValueNodeInputViewModel<int?>()
            {
                Name = "지연시간",
                Editor = new IntegerValueEditorViewModel(0),
                Port = null,
            };
            this.Inputs.Add(Input);

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
                    Log = Input.ValueChanged.Select(input => $"Delay - {input ?? 0}")
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
