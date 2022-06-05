using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using EasyMacro.View.Node;
using ReactiveUI;
using EasyMacro.ViewModel.Node.Editors;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class ForLoopNode : CodeGenNodeViewModel
    {
        static ForLoopNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<ForLoopNode>));
        }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueListNodeInputViewModel<IStatement> LoopBodyFlow { get; }
        public ValueListNodeInputViewModel<IStatement> LoopEndFlow { get; }

        public ValueNodeInputViewModel<ITypedExpression<int>> FirstIndex { get; }
        public ValueNodeInputViewModel<int?> LastIndex { get; }

        public ValueNodeOutputViewModel<ITypedExpression<int>> CurrentIndex { get; }

        public ForLoopNode() : base(NodeType.FlowControl)
        {
            var boundsGroup = new EndpointGroup("Bounds");

            var controlFlowGroup = new EndpointGroup("Control Flow");

            var controlFlowInputsGroup = new EndpointGroup(controlFlowGroup);

            this.Name = "For Loop";

            LoopBodyFlow = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Loop Body",
                Group = controlFlowInputsGroup
            };
            this.Inputs.Add(LoopBodyFlow);

            LoopEndFlow = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "Loop End",
                Group = controlFlowInputsGroup
            };
            this.Inputs.Add(LoopEndFlow);

            
            FirstIndex = new CodeGenInputViewModel<ITypedExpression<int>>(PortType.Integer)
            {
                Name = "First Index",
                Group = boundsGroup
            };
            //this.Inputs.Add(FirstIndex);
            

            LastIndex = new ValueNodeInputViewModel<int?>
            {
                Name = "Last Index",
                Group = boundsGroup,
                Editor = new IntegerValueEditorViewModel(1) { Value = 1 },
                Port = null,
            };
            this.Inputs.Add(LastIndex);

            ForLoop value = new ForLoop();

            var loopBodyChanged = LoopBodyFlow.Values.Connect().Select(_ => Unit.Default).StartWith(Unit.Default);
            var loopEndChanged = LoopEndFlow.Values.Connect().Select(_ => Unit.Default).StartWith(Unit.Default);
            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                Value = Observable.CombineLatest(loopBodyChanged, loopEndChanged, FirstIndex.ValueChanged, LastIndex.ValueChanged,
                        (bodyChange, endChange, firstI, lastI) => (BodyChange: bodyChange, EndChange: endChange, FirstI: firstI, LastI: lastI))
                    .Select(v => {
                        value.LoopBody = new StatementSequence(LoopBodyFlow.Values.Items);
                        value.LoopEnd = new StatementSequence(LoopEndFlow.Values.Items);
                        value.LowerBound = new IntLiteral { Value = 0 };
                        value.UpperBound = new IntLiteral { Value = v.LastI.Value };
                        return value;
                    }),
                Group = controlFlowGroup
            };
            this.Outputs.Add(FlowIn);

            CurrentIndex = new CodeGenOutputViewModel<ITypedExpression<int>>(PortType.Integer)
            {
                Name = "Current Index",
                Value = Observable.Return(new VariableReference<int> { LocalVariable = value.CurrentIndex }),
                Editor = new IntegerValueEditorViewModel(),
                Port = null,
            };
            this.Outputs.Add(CurrentIndex);
        }
    }
}
