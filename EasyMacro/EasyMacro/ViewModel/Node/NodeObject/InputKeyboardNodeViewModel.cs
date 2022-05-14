﻿using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;
using System;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class InputKeyboardNodeViewModel : CodeGenNodeViewModel
    {
        static InputKeyboardNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<InputKeyboardNodeViewModel>));
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        public ValueNodeInputViewModel<string> Input { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOut { get; }

        public InputKeyboardNodeViewModel() : base(NodeType.Function)
        {
            base.Name = "InputKeyboard";
//TODO : 나머지 구현
            Input = new ValueNodeInputViewModel<string>()
            {
                Name = "char",
                Editor = new StringValueEditorViewModel()
            };
            this.Inputs.Add(Input);

            FlowOut = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
            };
            this.Inputs.Add(FlowOut);
            
            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
            };
            this.Outputs.Add(FlowIn);
            
        }
    }
}
