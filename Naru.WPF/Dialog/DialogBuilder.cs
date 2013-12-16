using System;
using System.Collections.Generic;

namespace Naru.WPF.Dialog
{
    public class DialogBuilder<T> : IDialogBuilder<T>
    {
        private readonly Func<DialogViewModel<T>> _dialogViewModelFactory;
        private readonly List<T> _answers = new List<T>();

        private DialogType _dialogType;
        private string _title;
        private object _content;

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

        public IDialogBuilder<T> WithContent(object content)
        {
            _content = content;

            return this;
        }

        public DialogViewModel<T> Build()
        {
            var viewModel = _dialogViewModelFactory();
            viewModel.Initialise(_dialogType, _answers, _title, _content);
            return viewModel;
        }
    }
}