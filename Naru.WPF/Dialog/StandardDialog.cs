using System.Threading.Tasks;

using Naru.Core;

namespace Naru.WPF.Dialog
{
    public class StandardDialog : IStandardDialog
    {
        private readonly IDialogBuilder<Answer> _dialogBuilder;
        private readonly IEventStream _eventStream;

        public StandardDialog(IDialogBuilder<Answer> dialogBuilder, IEventStream eventStream)
        {
            _dialogBuilder = dialogBuilder;
            _eventStream = eventStream;
        }

        public Answer Question(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Question)
                .WithAnswers(Answer.Yes, Answer.No)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Task<Answer> QuestionAsync(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Question)
                .WithAnswers(Answer.Yes, Answer.No)
                .WithTitle(title)
                .WithMessage(message)
                .ShowAsync();
        }

        public Answer Question(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Question)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Task<Answer> QuestionAsync(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Question)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .ShowAsync();
        }

        public Answer Warning(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Warning)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Task<Answer> WarningAsync(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Warning)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithMessage(message)
                .ShowAsync();
        }

        public Answer Warning(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Warning)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Task<Answer> WarningAsync(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Warning)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .ShowAsync();
        }

        public Answer Information(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Information)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Task<Answer> InformationAsync(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Information)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithMessage(message)
                .ShowAsync();
        }

        public Answer Information(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Information)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Task<Answer> InformationAsync(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Information)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .ShowAsync();
        }

        public Answer Error(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Error)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Task<Answer> ErrorAsync(string title, string message)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Error)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithMessage(message)
                .ShowAsync();
        }

        public Answer Error(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Error)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .Show();
        }

        public Task<Answer> ErrorAsync(string title, string message, params Answer[] possibleResponens)
        {
            return _dialogBuilder
                .WithDialogType(DialogType.Error)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithMessage(message)
                .ShowAsync();
        }
    }
}