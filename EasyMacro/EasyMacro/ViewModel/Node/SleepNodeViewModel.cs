using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.ViewModel.Node
{
    public class SleepNodeViewModel : NodeViewModel
    {
        static SleepNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<SleepNodeViewModel>));
        }

        /// <summary>
        /// Delay Time
        /// </summary>
        public ValueNodeInputViewModel<int?> Input1 { get; }

        public SleepNodeViewModel()
        {
            base.Name = "Sleep";
//TODO : 나머지 구현
            Input1 = new ValueNodeInputViewModel<int?>()
            {
                Name = "Delay Time",
                Editor = new IntegerValueEditorViewModel()
            };
        }
    }
}
