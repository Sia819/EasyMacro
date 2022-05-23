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

using GalaSoft.MvvmLight.Command;
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
        /// 매크로 흐름에 대한 정보입니다.
        /// </summary>
        public ObservableCollection<Model.IMacro> LogicList { get; set; } = new();

        /// <summary>
        /// 디자이너에서만 보여질 리스트이며, 디자인 전용으로 만들어졌습니다.
        /// </summary>
        public ObservableCollection<Model.IMacro> DesignerList { get; set; }

        /// <summary>
        /// Properties Editor에서 선택된 매크로 타입
        /// </summary>
        public Model.MacroDisplayType SelectedMacro { get; set; }
        /// <summary>
        /// List형식의 View를 보여줄것인지, Node형식의 View를 보여줄것인지
        /// </summary>
        public bool ToggleState { get; set; }
        /// <summary>
        /// 좌측 툴 상자패널의 열림, 접힘 여부 / ToggleFoldToolboxCommand 메서드로 인해 변동.
        /// </summary>
        public bool IsToolboxFolded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TitleHeight { get; set; }
        public int MaximizeButton_Height { get; set; }
        public int MaximizeButton_Width { get; set; }

        public NodeListViewModel ListViewModel { get; } = new NodeListViewModel();
        public NetworkViewModel NetworkViewModel { get; } = new NetworkViewModel();
        public string ValueLabel { get; set; } 
        #endregion


        #region Public Command
        /// <summary>
        /// 매크로 로직 저장 버튼 커맨드
        /// </summary>
        public RelayCommand SaveCommand => new(Save);
        /// <summary>
        /// 매크로 로직 로드 버튼 커맨드
        /// </summary>
        public RelayCommand LoadCommand => new(Load);
        /// <summary>
        /// 매크로 로직 시작 버튼 커맨드
        /// </summary>
        public RelayCommand StartCommand => new(Start);
        /// <summary>
        /// 매크로의 새로운 로직을 추가하는 버튼 커맨드.
        /// </summary>
        public RelayCommand AddCommand => new(Add);
        /// <summary>
        /// 디버그용 테스트 버튼 커맨드
        /// </summary>
        public RelayCommand TestCommand => new(Test);
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand ToggleFoldToolboxCommand => new(ToggleFoldToolbox);
        #endregion


        #region Private Field
        // EasyMacroAPI 라이브러리의 MacroManager, 매크로 로직 관리 매니저
        private MacroManager macroManager;
        #endregion


        #region Constructor
        public MainWindowViewModel()
        {
            // MainWindow의 기본이 되는 프로퍼티 설정
            TitleHeight = 8;
            MaximizeButton_Height = 29;
            MaximizeButton_Width = 45;

            // NodeEditPage의 기본이 되는 프로퍼티 설정
            IsToolboxFolded = false; // 기본적으로 도구상자는 열려있습니다.

            // 매크로에 대한 초기화
            macroManager = MacroManager.Instance;
            // 디자이너에서만 보여지는 템플릿
            DesignerList = new ObservableCollection<IMacro>()
            {
                new SleepMacro(new EasyMacroAPI.Command.Delay(251)),
                new MouseMacro(new EasyMacroAPI.Command.MouseMove(200, 230)),
                new MouseMacro(new EasyMacroAPI.Command.MouseClick(200, 230)),
            };

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

        /// <summary>
        /// 각종 디버그용 테스트 로직을 여기에 놓으세요.
        /// </summary>
        private void Test()
        {
            MacroManager macroManager = MacroManager.Instance;
            //파일명 넣을것
            //macroManager.InsertList(new EasyMacroAPI.Command.TempletMatch(@"C:\target.png", "로컬 디스크 (C:)"));
            //macroManager.DoOnce(0);
        }

        /// <summary>
        /// 현재까지 작업된 리스트를 파일로 저장합니다.
        /// </summary>
        private void Save()
        {
            macroManager.SaveData();
        }

        /// <summary>
        /// 매크로 파일을 OpenFileDialog로 로드하여 리스트에 덮어씁니다.
        /// </summary>
        private void Load()
        {
            string macroPath = null;
            OpenFileDialog dlgOpenFile = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),  // OpenFileDialog가 기본으로 표시하는 경로는 바탕화면입니다.
                Filter = "MacroFiles (*.xml, *.em) | *.xml; *.em; | All files (*.*) | *.*"
            };
            

            if (dlgOpenFile.ShowDialog() == true)
            {// OpenFileDialog에서 경로가 지정 됨
                macroPath = dlgOpenFile.FileName;
            }
            else
            {// 파일 열기를 취소함
                MessageBox.Show("파일 열기 작업이 취소되었습니다.");
            }

            macroManager.LoadData(macroPath);
            LogicList.Clear();

            // 로직의 실체는 MacroManager로드 기능에 의해 이미 로드되었고, UI에는 아직 표시되지 않았으므로
            // 로드된 매크로를 기반으로, 각각 종류에 맞게 UI에 표시되도록 합니다.
            foreach (var macroAction in macroManager.ActionList)
            {
                switch (macroAction.MacroType)
                {
                    case EasyMacroAPI.Model.MacroTypes.Delay:
                        // 딜레이
                        LogicList.Add(new SleepMacro(macroAction));
                        break;
                    case EasyMacroAPI.Model.MacroTypes.MouseMove:
                        // 마우스 이동
                        LogicList.Add(new MouseMacro(macroAction)); // 마우스 매크로는 어떤 종류의 마우스매크로인지 따로 체킹한다.
                        break;
                    case EasyMacroAPI.Model.MacroTypes.MouseClick:
                        // 마우스 클릭
                        LogicList.Add(new MouseMacro(macroAction));
                        break;
                    default:
                        // 구현되지 않은 매크로
                        if (IsDebugMode)
                            throw new NotImplementedException($"지정하지 않은 매크로 타입\"{macroAction}\"이 있습니다.\n구현해 주세요!");
                        LogicList.Add(new UndefinedMacro(macroAction));
                        break;
                }
            }
        }

        /// <summary>
        /// 매크로 편집기에서 리스트의 새 매크로로 추가합니다.
        /// </summary>
        /// <param name="macro"></param>
        private void Add()
        {
            LogicList.Add(new UndefinedMacro() { DisplayText = "새 매크로" });
        }

        /// <summary>
        /// 리스트로 로드된 모든 매크로를 실행합니다.
        /// </summary>
        private void Start()
        {
            MessageBox.Show("Start Commend Excuted!");

            //macroManager.DoOnce(0);
            macroManager.StartMacro();
        }

        private void ToggleFoldToolbox()
        {
            this.IsToolboxFolded = !this.IsToolboxFolded;
        }


        #endregion

    }
}
