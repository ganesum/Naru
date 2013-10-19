using System.Threading.Tasks;

namespace Naru.WPF.Dialog
{
    public class StandardDialogBuilder : IStandardDialogBuilder
    {
        private readonly IDialogBuilder<Answer> _dialogBuilder;

        public StandardDialogBuilder(IDialogBuilder<Answer> dialogBuilder)
        {
            _dialogBuilder = dialogBuilder;
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