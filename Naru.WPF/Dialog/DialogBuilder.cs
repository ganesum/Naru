using System;
using System.Collections.Generic;

using Naru.WPF.MVVM;

namespace Naru.WPF.Dialog
{
    public class DialogBuilder<T> : IDialogBuilder<T>
    {
        private readonly Func<DialogViewModel<T>> _dialogViewModelFactory;
        private readonly List<T> _answers = new List<T>();

        private DialogType _dialogType;
        private string _title;
        private string _message;

        public DialogBuilder(Func<DialogViewModel<T>> dialogViewModelFactory)
        {
            _dialogViewModelFactory = dialogViewModelFactory;
        }

        public IDialogBuilder<T> WithDialogType(DialogType dialogType)
        {
            _dialogType = dialogType;

            return this;
        }

        public IDialogBuilder<T> WithAnswers(params T[] answers)
        {
            foreach (var answer in answers)
            {
                _answers.Add(answer);
            }

            return this;
        }

        public IDialogBuilder<T> WithTitle(string title)
        {
            _title = title;

            return this;
        }

        public IDialogBuilder<T> WithMessage(string message)
        {
            _message = message;

            return this;
        }

        public DialogViewModel<T> Build()
        {
            var viewModel = _dialogViewModelFactory();
            viewModel.Initialise(_dialogType, _answers, _title, _message);
            return viewModel;
        }
    }
}