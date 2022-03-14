using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyMacro.View
{
    /// <summary>
    /// Interaction logic for EditProperties.xaml
    /// </summary>
    public partial class PropertiesEditor : UserControl
    {
        #region Public Dependency Properties

        public static readonly DependencyProperty MacrosListProperty
            = DependencyProperty.Register("MacrosList", typeof(ObservableCollection<Model.IMacros>), typeof(PropertiesEditor));

        public ObservableCollection<Model.IMacros> MacrosList
        {
            get => (ObservableCollection<Model.IMacros>)GetValue(MacrosListProperty);
            set => SetValue(MacrosListProperty, value);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// ComboBox에 표시되는 ItemsSource, 추가할 수 있는 매크로들의 목록입니다.
        /// </summary>
        public ObservableCollection<Model.PropertiesModel> MacroCommands { get; set; }

        #endregion

        public PropertiesEditor()
        {
            InitializeComponent();

            MacroCommands.Add(new Model.PropertiesModel()
            {
                DisplayName = "Sleep 매크로",
                MacroType = Model.MacroCommand.Sleep,
                TargetViewModel = sleepPropertiesViewModel
            });

        }
    }
}
