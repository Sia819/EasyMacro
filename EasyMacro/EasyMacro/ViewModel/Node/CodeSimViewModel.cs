﻿using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using EasyMacro.Model.Node;
using EasyMacro.Model.Node.Compiler;
using ReactiveUI;

namespace EasyMacro.ViewModel.Node
{
    public class CodeSimViewModel : ReactiveObject
    {
        #region Singleton
        private static CodeSimViewModel instance;
        public static CodeSimViewModel Instance => instance ??= new CodeSimViewModel();
        #endregion

        #region Code
        public IStatement Code
        {
            get => _code;
            set => this.RaiseAndSetIfChanged(ref _code, value);
        }
        private IStatement _code;
        #endregion

        #region Output
        public string Output
        {
            get => _output;
            set => this.RaiseAndSetIfChanged(ref _output, value);
        }
        private string _output;
        #endregion

        public ReactiveCommand<Unit, Unit> RunScript { get; }

        public ReactiveCommand<Unit, Unit> ClearOutput { get; }

        public bool IsRunning = false;
        public bool ReStart = false;

        private Thread nodeFlowThread;

        private CodeSimViewModel()
        {
            RunScript = ReactiveCommand.Create(RunScript_ExcuteCommand, // Excute
                                               this.WhenAnyValue(vm => vm.Code).Select(code => code != null)); //CanExcute

            nodeFlowThread = new Thread(new ThreadStart(nodeFlowThread_Excute)) { IsBackground = true };

            ClearOutput = ReactiveCommand.Create(() => { Output = ""; });
        }

        public void RunScript_ExcuteCommand()
        {
            if (this.Code is not null && !nodeFlowThread.IsAlive)
            {
                if ((nodeFlowThread.ThreadState & ThreadState.Unstarted) == 0)
                {
                    nodeFlowThread = new Thread(new ThreadStart(nodeFlowThread_Excute)) { IsBackground = true };
                }
                nodeFlowThread.Start();
            }
        }

        public void TerminateThread()
        {
            IsRunning = false;
        }

        void nodeFlowThread_Excute()
        {
            ReStart = true;
            while (ReStart)
            {
                ReStart = false;
                IsRunning = true;
                Code.Compile(new CompilerContext());
            }
        }

        public void Print(string msg)
        {
            Application.Current.Dispatcher.BeginInvoke(() => { Output += msg + "\n"; }, DispatcherPriority.Normal);
        }

    }
}
