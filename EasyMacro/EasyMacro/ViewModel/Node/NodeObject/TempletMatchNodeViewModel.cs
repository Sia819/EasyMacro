using DynamicData;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using EasyMacro.View.Node;
using EasyMacro.ViewModel.Node.Editors;
using EasyMacroAPI.Command;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace EasyMacro.ViewModel.Node.NodeObject
{
    public class TempletMatchNodeViewModel : CodeGenNodeViewModel, INodeSerializable, IExtendedXmlCustomSerializer
    {
        static TempletMatchNodeViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new CodeGenNodeView(), typeof(IViewFor<TempletMatchNodeViewModel>));
        }

        private static TempletMatch _templetMatch;
        private TempletMatch templetMatch
        {
            get
            {
                if (_templetMatch is null)
                    _templetMatch = new TempletMatch();
                return _templetMatch;
            }
        }

        public ValueNodeInputViewModel<string> BitmapDir { get; }

        public ValueNodeInputViewModel<string> WindowName { get; }

        public ValueNodeInputViewModel<bool?> IsWantKeepFind { get; }

        public ValueNodeInputViewModel<int?> RetryTimes { get; }

        public ValueNodeInputViewModel<int?> Accuracy { get; }

        public ValueNodeInputViewModel<int?> Delay { get; }

        public ValueNodeOutputViewModel<IStatement> FlowIn { get; }

        public ValueNodeOutputViewModel<System.Drawing.Point> ResultPoint { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOutOption1 { get; }

        public ValueListNodeInputViewModel<IStatement> FlowOutOption2 { get; }

        public ValueNodeInputViewModel<int?> RunButton { get; }

        public bool IsCanExcute { get; set; } = true;

        public override void Serializer(XmlWriter xmlWriter, object obj)
        {
            NodeSerializer.SerializerOfNodeViewModel(ref xmlWriter, ref obj);

            TempletMatchNodeViewModel instance = obj as TempletMatchNodeViewModel;

            xmlWriter.WriteElementString(nameof(BitmapDir), instance.BitmapDir.Value.ToString());

            //프로세스 찾기

            xmlWriter.WriteElementString(nameof(RetryTimes), instance.RetryTimes.Value.ToString());
            xmlWriter.WriteElementString(nameof(IsWantKeepFind), instance.IsWantKeepFind.Value.ToString());
            xmlWriter.WriteElementString(nameof(Accuracy), instance.Accuracy.Value.ToString());
            xmlWriter.WriteElementString(nameof(Delay), instance.Delay.Value.ToString());
        }

        public override object Deserialize(XElement xElement)
        {
            TempletMatchNodeViewModel instance = (TempletMatchNodeViewModel)NodeSerializer.DeserializeOfNoveViewModel(ref xElement, new TempletMatchNodeViewModel());

            Dictionary<string, XElement> dictionary = NodeSerializer.XElementToDictionary(xElement);
            (instance.BitmapDir.Editor as ImageManagerSelectorViewModel).Value = dictionary["BitmapDir"].Value;

            (instance.RetryTimes.Editor as IntegerValueEditorViewModel).Value = int.TryParse(dictionary["RetryTimes"].Value, out int retryTimes) ? retryTimes : 0;
            (instance.IsWantKeepFind.Editor as CheckBoxEditorViewModel).Value = bool.TryParse(dictionary["IsWantKeepFind"].Value, out bool IsWantKeepFind) ? IsWantKeepFind : false;
            (instance.Accuracy.Editor as IntegerValueEditorViewModel).Value = int.TryParse(dictionary["Accuracy"].Value, out int Accuracy) ? Accuracy : 80;
            (instance.Delay.Editor as IntegerValueEditorViewModel).Value = int.TryParse(dictionary["Delay"].Value, out int delay) ? delay : 1000;
            return instance;
        }

        public override void Connect()
        {
            throw new NotImplementedException();
        }

        public TempletMatchNodeViewModel() : base(NodeType.Function)
        {
            var controlFlowGroupIn = new EndpointGroup("");
            var controlFlowGroupOut1 = new EndpointGroup(controlFlowGroupIn);
            var controlFlowGroupOut2 = new EndpointGroup(controlFlowGroupOut1);

            base.Name = "이미지 찾기";

            //TODO : 구조변경
            BitmapDir = new ValueNodeInputViewModel<string>()
            {
                Name = "사진파일 경로",
                Editor = new ImageManagerSelectorViewModel(),
                Port = null,
            };
            BitmapDir.ValueChanged.Select(_ => ImgBindToApi()).Do(action => action.Invoke()).Subscribe();


            WindowName = new ValueNodeInputViewModel<string>()
            {
                Name = "프로세스 이름",
                Editor = new StringValueEditorViewModel() { },
                Port = null
            };
            (WindowName.Editor as StringValueEditorViewModel).ValueChanged.Select(_ => WinnameToApi());


            RetryTimes = new ValueNodeInputViewModel<int?>()
            {
                Name = "재시도 횟수",
                Editor = new IntegerValueEditorViewModel(),
                Port = null,
            };

            IsWantKeepFind = new ValueNodeInputViewModel<bool?>()
            {
                Name = "못 찾으면 넘어가지 않기",
                Editor = new CheckBoxEditorViewModel(),
                Port = null,
            };
            IsWantKeepFind.ValueChanged.Do(isChecked =>
            {
                (RetryTimes.Editor as IntegerValueEditorViewModel).Editable = !(bool)isChecked;
            }).Subscribe();



            Accuracy = new ValueNodeInputViewModel<int?>()
            {
                Name = "비교 정확도",
                Editor = new IntegerValueEditorViewModel(1, 100) { Value = 80 },
                Port = null,
            };


            Delay = new ValueNodeInputViewModel<int?>()
            {
                Name = "지연시간 (1초 = 1000)",
                Editor = new IntegerValueEditorViewModel(1000),
                Port = null,
            };


            this.RunButton = new ValueNodeInputViewModel<int?>()
            {
                Port = null,

                Editor = new RunButtonViewModel()
                {
                    RunScript = ReactiveCommand.Create
                    (
                        Func(),
                        this.WhenAnyValue(vm => vm.IsCanExcute)
                    )
                }
            };

            FlowIn = new CodeGenOutputViewModel<IStatement>(PortType.Execution)
            {
                Name = "",
                Value = this.RunButton.ValueChanged.Select(_ => new NodeCompile(this.Func())
                {
                    Log = Observable.Merge(BitmapDir.ValueChanged.Select(dir => $"TempletMatch - ({dir}), ({this.WindowName.Value})"),
                                           WindowName.ValueChanged.Select(windowname => $"TempletMatch - ({this.BitmapDir.Value}), ({windowname})"))
                }),
                Group = controlFlowGroupIn
            };

            FlowOutOption1 = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "이미지 찾기 성공",
                MaxConnections = 1,
                Group = controlFlowGroupOut1
            };

            ResultPoint = new CodeGenOutputViewModel<System.Drawing.Point>(PortType.Point)
            {
                Name = "Point",
                Editor = new PointRecordEditorViewModel()
                {
                    Editable = false,
                    ButtonEnable = false
                },
                Group = controlFlowGroupOut1
            };
            ResultPoint.Value = (ResultPoint.Editor as PointRecordEditorViewModel).ValueChanged;

            FlowOutOption2 = new CodeGenListInputViewModel<IStatement>(PortType.Execution)
            {
                Name = "이미지 찾기 실패",
                MaxConnections = 1,
                Group = controlFlowGroupOut2
            };

            this.Inputs.Add(BitmapDir);         // ComboBox
            this.Inputs.Add(WindowName);        // 
            this.Inputs.Add(IsWantKeepFind);
            this.Inputs.Add(RetryTimes);
            this.Inputs.Add(Accuracy);
            this.Inputs.Add(Delay);
            this.Inputs.Add(this.RunButton); // 

            this.Outputs.Add(FlowIn);        // Flow 들어옴

            this.Inputs.Add(FlowOutOption1); // 찾기 성공
            this.Outputs.Add(ResultPoint);   // 반환 포인터

            this.Inputs.Add(FlowOutOption2); // 찾기 실패
        }

        private Action Func()
        {
            Action action = () =>
            {
                if (CodeSimViewModel.Instance.IsRunning || Thread.CurrentThread.IsBackground is false)
                {
                    if (BitmapDir.Value != null)
                    {
                        CodeSimViewModel.Instance.Print((FlowIn.CurrentValue as NodeCompile).CurrentValue);

                        templetMatch.ScreenCapture.WindowName = WindowName.Value;
                        templetMatch.IsWantKeepFinding = IsWantKeepFind.Value ?? false;
                        templetMatch.retryTimes = RetryTimes.Value ?? 0;
                        templetMatch.Accuracy = (double)(Accuracy.Value ?? 80) / 100;
                        templetMatch.SetDelayTime(Delay.Value ?? 1000);

                        templetMatch.Do();
                        Application.Current.Dispatcher.Invoke(() => (ResultPoint.Editor as PointRecordEditorViewModel).Value = templetMatch.FoundPoint); // 멀티스레드에서 UI 수정

                        ValueListNodeInputViewModel<IStatement> selectedFlowout;

                        if (templetMatch.Result)
                        {
                            selectedFlowout = FlowOutOption1;
                        }
                        else
                        {
                            selectedFlowout = FlowOutOption2;
                        }

                        foreach (var a in selectedFlowout.Values.Items)
                        {
                            a.Compile(new CompilerContext());
                        }
                    }
                }
            };
            return action;
        }

        // TODO : 이미지를 EasyMacroAPI의 템플릿매치에 사용될이미지로 지정합니다.
        private Action ImgBindToApi()
        {
            Action action = () =>
            {
                if (String.IsNullOrEmpty((BitmapDir.Editor as ImageManagerSelectorViewModel).Value) is false)
                {
                    // dispose 작업은 ImageManagerSelectorView의 imageSelector_SelectionChanged() 에서 함.
                    templetMatch.TargetImg = (BitmapDir.Editor as ImageManagerSelectorViewModel).SelectedBitmap;
                }
            };
            return action;
        }

        private Action WinnameToApi()
        {
            Action action = () =>
            {
                // TODO : TextBox의 String을 받는 구조 -> 창 찾기 컨트롤의 값을 받는 구조.
                templetMatch.ScreenCapture.WindowName = (this.WindowName.Editor as StringValueEditorViewModel).Value;
            };
            return action;
        }

        

        
    }
}
