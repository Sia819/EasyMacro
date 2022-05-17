using EasyMacro.View.Node.Editors;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

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
