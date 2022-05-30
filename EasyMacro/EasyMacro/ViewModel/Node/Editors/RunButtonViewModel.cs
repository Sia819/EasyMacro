using System;
using System.Reactive;
using EasyMacro.View.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node.Editors
{
    public class RunButtonViewModel : ValueEditorViewModel<int?>
    {
        static RunButtonViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new RunButtonView(), typeof(IViewFor<RunButtonViewModel>));
        }

        public ReactiveCommand<Unit, Unit> RunScript { get; set; }

        public RunButtonViewModel()
        {
            Value = 0; // 더비 벨류
        }
    }
    
}
