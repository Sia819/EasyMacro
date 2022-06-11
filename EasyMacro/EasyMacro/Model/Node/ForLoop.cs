﻿using EasyMacro.Model.Node.Compiler;
using EasyMacro.ViewModel.Node.Editors;

namespace EasyMacro.Model.Node
{
    public class ForLoop : IStatement
    {
        public IStatement LoopBody { get; set; }
        public IStatement LoopEnd { get; set; }

        public int LowerBound { get; set; }
        public int UpperBound { get; set; }

        public InlineVariableDefinition<int> CurrentIndex { get; } = new InlineVariableDefinition<int>();

        public IntegerValueEditorViewModel currentIndexEditor;

        public ForLoop(IntegerValueEditorViewModel current)
        {
            currentIndexEditor = current;
        }

        public string Compile(CompilerContext context)
        {
            context.EnterNewScope("For loop");

            string code = "";

            currentIndexEditor.Value = 0;

            for(int i = 0; i < UpperBound; i++)
            {
                currentIndexEditor.Value = currentIndexEditor.Value + 1;
                LoopBody.Compile(context);
            }
            LoopEnd.Compile(context);

            context.LeaveScope();
            return code;
        }
    }
}