using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Reflection;
using PropertyChanged;
using static EasyMacro.Common.DebugState;

namespace EasyMacro.UC
{
    /// <summary>
    /// Interaction logic for EditProperties.xaml
    /// </summary>
    public partial class PropertiesEditor : UserControl, INotifyPropertyChanged
    {
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Dependency Property

        public static readonly DependencyProperty MacrosListProperty
            = DependencyProperty.Register("MacrosList", typeof(ObservableCollection<Model.IMacro>), typeof(PropertiesEditor));
        public ObservableCollection<Model.IMacro> MacrosList
        {
            get => (ObservableCollection<Model.IMacro>)GetValue(MacrosListProperty);
            set => SetValue(MacrosListProperty, value);
        }

        #endregion


        #region Public Properties
        /// <summary>
        /// 매크로 속성들의 집합입니다, ComboBox에 표시되는 ItemsSource, 추가할 수 있는 매크로들의 목록입니다.
        /// </summary>
        public ObservableCollection<Model.PropertiesModel> MacroCommands { get; set; }

        /// <summary>
        /// ComboBox에서 선택된 매크로 입니다.
        /// 이것이 변경되면 MainWindowViewModel의 SelectedMacro프로퍼티에도 영향을 끼칩니다.
        /// </summary>
        public Model.PropertiesModel SelectedMacro
        {
            get => selectedMacro;
            set
            {
                selectedMacro = value;
                var target = (DataContext as EasyMacro.ViewModel.MainWindowViewModel);
                target.SelectedMacro = value.MacroType;
            }
        }

        //public Model.IMacro EditingMacro { get; set; }
        #endregion


        #region Private Field
        private Model.PropertiesModel selectedMacro;
        #endregion 


        #region Constructor
        public PropertiesEditor()
        {
            InitializeComponent();

            // "MacroCommands" : 바인딩 가능한 컬렉션 객체를 초기화합니다.
            // "PropertiesModel" : "매크로 표시이름"과 "매크로의 종류"을 가지는 객체로써 해당하는 열거형 "MacroDisplayType"를 어떻게 표시할지를 나타냅니다.
            // "MacroType"을 key, "DisplayName"을 value로 Dictionary객체로 만들어도 되지만, xaml ComboBox에 보여지기 위하여 컬렉션으로 만듭니다.
            MacroCommands = new ObservableCollection<Model.PropertiesModel>();

            foreach (Model.MacroDisplayType macroType in Enum.GetValues(typeof(Model.MacroDisplayType)))
            {
                switch (macroType)
                {
                    case Model.MacroDisplayType.Sleep:
                        // 딜레이
                        MacroCommands.Add(new Model.PropertiesModel() { DisplayName = "Sleep 매크로", MacroType = Model.MacroDisplayType.Sleep });
                        break;
                    case Model.MacroDisplayType.MouseMove:
                        // 마우스 이동
                        MacroCommands.Add(new Model.PropertiesModel() { DisplayName = "MouseMove 매크로", MacroType = Model.MacroDisplayType.MouseMove });
                        break;
                    case Model.MacroDisplayType.MouseClick:
                        // 마우스 클릭
                        MacroCommands.Add(new Model.PropertiesModel() { DisplayName = "MouseClick 매크로", MacroType = Model.MacroDisplayType.MouseClick });
                        break;
                    default:
                        // 구현되지 않은 매크로
                        if (macroType != Model.MacroDisplayType.Undefined &&
                            IsDebugMode)
                            throw new NotImplementedException($"지정되지 않은 매크로 타입 \"{macroType}\"이 있습니다.\n구현해 주세요!");
                        break;
                }
            }



        }
        #endregion Constructor
    }
}
