﻿using System.Collections.Generic;
using System.Linq;

using Common.Logging;

using Microsoft.Practices.Prism.Commands;

namespace Naru.WPF.MVVM.Dialog
{
    [UseView(typeof(DialogView))]
    public class DialogViewModel<T> : Workspace
    {
        public BindableCollection<DialogItemViewModel<T>> Answers { get; private set; }

        public T SelectedAnswer { get; private set; }

        public string Message { get; private set; }

        public DelegateCommand<DialogItemViewModel<T>> ExecuteCommand { get; private set; } 

        public DialogViewModel(ILog log, IDispatcherService dispatcherService, BindableCollectionFactory bindableCollectionFactory) 
            : base(log, dispatcherService)
        {
            Answers = bindableCollectionFactory.Get<DialogItemViewModel<T>>();

            ExecuteCommand = new DelegateCommand<DialogItemViewModel<T>>(x =>
            {
                SelectedAnswer = x.Response;

                Close();
            });
        }

        public void Initialise(DialogType dialogType, List<T> answers, string title, string message)
        {
            DisplayName = string.Format("{0} - {1}", dialogType, title);
            Message = message;

            for (var index = 0; index < answers.Count; index++)
            {
                var answer = new DialogItemViewModel<T>
                {
                    Response = answers[index],
                    IsDefault = index == 0,
                    IsCancel = index == answers.Count - 1
                };
                Answers.Add(answer);
            }

            var lastButton = Answers.LastOrDefault();
            if (lastButton == null) return;

            SelectedAnswer = lastButton.Response;
        }
    }
}