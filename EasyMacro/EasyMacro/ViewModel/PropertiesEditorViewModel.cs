using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace EasyMacro.ViewModel
{
    public class PropertiesEditorViewModel : ViewModelBase
    {
        /// <summary>
        /// ComboBox에 표시되는 ItemsSource, 추가할 수 있는 매크로들의 목록입니다.
        /// </summary>
        public ObservableCollection<Model.PropertiesModel> MacroCommands { get; set; }

        /// <summary>
        /// ComboBox에서 선택된 Context입니다.
        /// </summary>
        public Model.PropertiesModel SelectedMacro
        {
            get => _selectedMacro;
            set => Set(ref _selectedMacro, value);
        }

        private Model.PropertiesModel _selectedMacro;

        public PropertiesEditorViewModel(SleepPropertiesViewModel sleepPropertiesViewModel)
        {
            MacroCommands = new();

            MacroCommands.Add(new Model.PropertiesModel()
            {
                DisplayName = "Sleep 매크로",
                MacroType = Model.MacroCommand.Sleep,
                TargetViewModel = sleepPropertiesViewModel
            });
        }

    }
}
