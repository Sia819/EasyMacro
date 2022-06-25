using System;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using EasyMacro.Model;
using EasyMacro.Model.DisplayMacro;
using EasyMacro.ViewModel.Node.NodeObject;
using static EasyMacro.Common.DebugState;
using EasyMacroAPI;
using Microsoft.Win32;
using DynamicData;
using NodeNetwork.Toolkit.NodeList;
using NodeNetwork.ViewModels;
using NodeNetwork.Toolkit;
using NodeNetwork;

using ReactiveUI;


namespace EasyMacro.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        static MainWindowViewModel()
        {
            // View Model Locator
            Splat.Locator.CurrentMutable.Register(() => new EasyMacro.View.MainWindow(), typeof(ReactiveUI.IViewFor<>));
        }

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Public Properties

        /// <summary>
        /// List형식의 View를 보여줄것인지, Node형식의 View를 보여줄것인지
        /// </summary>
        public bool ToggleState { get; set; }

        /// <summary>
        /// 좌측 툴 상자패널의 열림, 접힘 여부 / ToggleFoldToolboxCommand 메서드로 인해 변동.
        /// </summary>
        public bool IsToolboxFolded { get; set; }

        public NodeListViewModel ListViewModel { get; } = new NodeListViewModel();

        public NetworkViewModel NetworkViewModel { get; } = new NetworkViewModel();
        public string ValueLabel { get; set; } 
        #endregion


        #region Private Field
        // EasyMacroAPI 라이브러리의 MacroManager, 매크로 로직 관리 매니저
        //private MacroManager macroManager;
        #endregion


        #region Constructor
        public MainWindowViewModel()
        {
            // NodeEditPage의 기본이 되는 프로퍼티 설정
            IsToolboxFolded = false; // 기본적으로 도구상자는 열려있습니다.

            // Node 초기화
            ListViewModel.AddNodeType(() => new SumNodeViewModel());
            ListViewModel.AddNodeType(() => new DivisionNodeViewModel());

            OutputNodeViewModel output = new OutputNodeViewModel();
            NetworkViewModel.Nodes.Add(output);

            // 로직작성에 제한을 둚. 어떤 제한을 둘지 함수를 작성해서 (함수타입의)속성으로 등록
            NetworkViewModel.Validator = network =>
            {
                // 노드는 루프형식을 띄면 안됨
                bool containsLoops = GraphAlgorithms.FindLoops(network).Any();
                if (containsLoops)
                {
                    return new NetworkValidationResult(false, false, new ErrorMessageViewModel("Network contains loops!"));
                }

                // 블록은 이어져있어야 함?
                bool containsDivisionByZero = GraphAlgorithms.GetConnectedNodesBubbling(output)
                    .OfType<DivisionNodeViewModel>()
                    .Any(n => n.Input2.Value == 0);
                if (containsDivisionByZero)
                {
                    return new NetworkValidationResult(false, true, new ErrorMessageViewModel("Network contains division by zero!"));
                }

                // 이외는 정상작동
                return new NetworkValidationResult(true, true, null);
            };

            output.ResultInput.ValueChanged
                .Select(v => (NetworkViewModel.LatestValidation?.IsValid ?? true) ? v.ToString() : "Error")
                .BindTo(this, vm => vm.ValueLabel);
        }
        #endregion


        #region Private Command Function

        private void ToggleFoldToolbox()
        {
            this.IsToolboxFolded = !this.IsToolboxFolded;
        }


        #endregion

    }
}
