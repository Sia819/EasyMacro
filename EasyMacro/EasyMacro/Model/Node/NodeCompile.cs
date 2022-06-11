using EasyMacro.Model.Node.Compiler;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using System;
using System.Reactive.Concurrency;

namespace EasyMacro.Model.Node
{
    public class NodeCompile : ReactiveObject, IStatement// : INodeFlow
    {
        public Action MyAction;

        private IObservable<string> _log;
        public IObservable<string> Log
        {
            get => _log;
            set => this.RaiseAndSetIfChanged(ref _log, value);
        }
        public string CurrentValue => _currentValue.Value;
        private readonly ObservableAsPropertyHelper<string> _currentValue;

        public string Compile(CompilerContext context)
        {
            MyAction.Invoke();
            return CurrentValue;//"NodeCompile를 실행했습니다.";// new StringLiteral { Value = "NodeCompile를 실행했습니다." } 
        }

        public NodeCompile(Action action)
        {
            this.MyAction = action;
            this.WhenAnyObservable(vm => vm.Log).ToProperty(this, vm => vm.CurrentValue, out _currentValue, false, Scheduler.Immediate);
        }
    }
}
