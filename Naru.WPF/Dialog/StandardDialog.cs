using System.Threading.Tasks;

using Naru.TPL;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;

namespace Naru.WPF.Dialog
{
    public class StandardDialog : IStandardDialog
    {
        private readonly IDialogBuilder<Answer> _dialogBuilder;
        private readonly IUserInteraction _userInteraction;
        private readonly IDispatcherSchedulerProvider _scheduler;

        public StandardDialog(IDialogBuilder<Answer> dialogBuilder, IUserInteraction userInteraction, IDispatcherSchedulerProvider scheduler)
        {
            _dialogBuilder = dialogBuilder;
            _userInteraction = userInteraction;
            _scheduler = scheduler;
        }

        public Task<Answer> QuestionAsync(string title, string message)
        {
            var viewModel = _dialogBuilder
                .WithDialogType(DialogType.Question)
                .WithAnswers(Answer.Yes, Answer.No)
                .WithTitle(title)
                .WithContent(message)
                .Build();

            return _userInteraction.ShowModalAsync(viewModel)
                                   .Select(() => viewModel.SelectedAnswer, _scheduler.Dispatcher.TPL);
        }

        public Task<Answer> QuestionAsync(string title, string message, params Answer[] possibleResponens)
        {
            var viewModel = _dialogBuilder
                .WithDialogType(DialogType.Question)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithContent(message)
                .Build();

            return _userInteraction.ShowModalAsync(viewModel)
                                   .Select(() => viewModel.SelectedAnswer, _scheduler.Dispatcher.TPL);
        }

        public Task<Answer> WarningAsync(string title, string message)
        {
            var viewModel = _dialogBuilder
                .WithDialogType(DialogType.Warning)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithContent(message)
                .Build();

            return _userInteraction.ShowModalAsync(viewModel)
                                   .Select(() => viewModel.SelectedAnswer, _scheduler.Dispatcher.TPL);
        }

        public Task<Answer> WarningAsync(string title, string message, params Answer[] possibleResponens)
        {
            var viewModel = _dialogBuilder
                .WithDialogType(DialogType.Warning)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithContent(message)
                .Build();

            return _userInteraction.ShowModalAsync(viewModel)
                                   .Select(() => viewModel.SelectedAnswer, _scheduler.Dispatcher.TPL);
        }

        public Task<Answer> InformationAsync(string title, string message)
        {
            var viewModel = _dialogBuilder
                .WithDialogType(DialogType.Information)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithContent(message)
                .Build();

            return _userInteraction.ShowModalAsync(viewModel)
                                   .Select(() => viewModel.SelectedAnswer, _scheduler.Dispatcher.TPL);
        }

        public Task<Answer> InformationAsync(string title, string message, params Answer[] possibleResponens)
        {
            var viewModel = _dialogBuilder
                .WithDialogType(DialogType.Information)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithContent(message)
                .Build();

            return _userInteraction.ShowModalAsync(viewModel)
                                   .Select(() => viewModel.SelectedAnswer, _scheduler.Dispatcher.TPL);
        }

        public Answer Error(string title, string message)
        {
            var viewModel = _dialogBuilder
                .WithDialogType(DialogType.Error)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithContent(message)
                .Build();

            return _userInteraction.ShowModalAsync(viewModel)
                                   .Select(() => viewModel.SelectedAnswer, _scheduler.Dispatcher.TPL)
                                   .Result;
        }

        public Task<Answer> ErrorAsync(string title, string message)
        {
            var viewModel = _dialogBuilder
                .WithDialogType(DialogType.Error)
                .WithAnswers(Answer.Ok)
                .WithTitle(title)
                .WithContent(message)
                .Build();

            return _userInteraction.ShowModalAsync(viewModel)
                                   .Select(() => viewModel.SelectedAnswer, _scheduler.Dispatcher.TPL);
        }

        public Task<Answer> ErrorAsync(string title, string message, params Answer[] possibleResponens)
        {
            var viewModel = _dialogBuilder
                .WithDialogType(DialogType.Error)
                .WithAnswers(possibleResponens)
                .WithTitle(title)
                .WithContent(message)
                .Build();

            return _userInteraction.ShowModalAsync(viewModel)
                                   .Select(() => viewModel.SelectedAnswer, _scheduler.Dispatcher.TPL);
        }
    }
}