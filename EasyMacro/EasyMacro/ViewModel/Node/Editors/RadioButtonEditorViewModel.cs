﻿using EasyMacro.View.Node.Editors;
using EasyMacroAPI.Model;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMacro.ViewModel.Node.Editors
{
    public class RadioButtonEditorViewModel : ValueEditorViewModel<int?>
    {
        static RadioButtonEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new RadioButtonEditorView(), typeof(IViewFor<RadioButtonEditorViewModel>));
        }

        public ObservableCollection<MyListItem> MyList { get; }

        public string RadioGroupInstanceHash { get; set; }

        public int RadioSelectedIndex
        {
            get
            {
                for (int i = 0; i < MyList.Count; i++)
                {
                    if (MyList[i].IsChecked == true)
                    {
                        return i;
                    }
                }
                return 0; // 아무것도 선택되지 않았을 때, 첫번째 인덱스
            }
            set
            {
                for (int i = 0; i < MyList.Count; i++)
                {
                    MyList[i].IsChecked = false;
                }

                MyList[value].IsChecked = true;
                this.RaisePropertyChanged("MyList");
            }
        }

        public RadioButtonEditorViewModel()
        {
            Value = 0;
            this.RadioGroupInstanceHash = Common.HashGen.RandomHashGen(10); // 현재 그룹명의 해시, 해시 길이
            MyList = new ObservableCollection<MyListItem>(); // 더비 벨류
        }

        public class MyListItem
        {
            public MyListItem(bool isChecked, string clickType, string groupHash)
            {
                this.IsChecked = isChecked;
                this.ClickType = clickType;
                this.GroupHash = groupHash;
            }
            public bool IsChecked { get; set; }
            public string ClickType { get; set; }
            public string GroupHash { get; set; }
        }

    }
}
