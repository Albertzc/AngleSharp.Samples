﻿namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using AngleSharp.Scripting.JavaScript;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    sealed class ReplViewModel : BaseViewModel, ITabViewModel
    {
        readonly String prompt;
        readonly ObservableCollection<String> items;
        readonly JavaScriptEngine engine;
        IDocument document;
        Boolean readOnly;

        public ReplViewModel()
        {
            readOnly = false;
            engine = new JavaScriptEngine();
            prompt = "$ ";
            items = new ObservableCollection<String>();
            ClearCommand = new RelayCommand(() => Clear());
            ResetCommand = new RelayCommand(() => Reset());
            ExecuteCommand = new RelayCommand(cmd => Run(cmd.ToString()));
        }

        public Boolean IsReadOnly
        {
            get { return readOnly; }
            private set
            {
                readOnly = value;
                RaisePropertyChanged();
            }
        }

        public IDocument Document
        {
            get
            {
                return document;
            }
            set
            {
                Reset();
                document = value;
            }
        }

        public ObservableCollection<String> Items
        {
            get { return items; }
        }

        public String Prompt
        {
            get { return prompt; }
        }

        public ICommand ClearCommand
        {
            get;
            private set;
        }

        public ICommand ResetCommand
        {
            get;
            private set;
        }

        public ICommand ExecuteCommand
        {
            get;
            private set;
        }

        void Clear()
        {
            items.Clear();
        }

        void Reset()
        {
            engine.Reset();
            Clear();
        }

        void Run(String command)
        {
            items.Add(prompt + command);
            engine.Evaluate(command, new ScriptOptions
            {
                Document = document,
                Context = document.DefaultView
            });
            var lines = engine.Result.ToString();

            foreach (var line in lines.Split(new [] { "\n" }, StringSplitOptions.None))
                items.Add(line);
        }
    }
}