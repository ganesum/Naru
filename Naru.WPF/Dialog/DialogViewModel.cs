using System.Collections.Generic;
using System.Linq;

using Common.Logging;

using Naru.WPF.Command;
using Naru.WPF.MVVM;
using Naru.WPF.Scheduler;
using Naru.WPF.ViewModel;

namespace Naru.WPF.Dialog
{
    [UseView(typeof(DialogView))]
    public class DialogViewModel<T> : Workspace, IUserInteractionViewModel
    {
        public BindableCollection<DialogItemViewModel<T>> Answers { get; private set; }

        public T SelectedAnswer { get; private set; }

        public object Content { get; private set; }

        public DelegateCommand<DialogItemViewModel<T>> ExecuteCommand { get; private set; }

        public DialogViewModel(ILog log, ISchedulerProvider scheduler, IStandardDialog standardDialog,
                               BindableCollection<DialogItemViewModel<T>> answersCollection)
            : base(log, scheduler, standardDialog)
        {
            Answers = answersCollection;

            ExecuteCommand = new DelegateCommand<DialogItemViewModel<T>>(x =>
                                                                         {
                                                                             SelectedAnswer = x.Response;

                                                                             ClosingStrategy.Close();
                                                                         });
        }

        public void Initialise(DialogType dialogType, List<T> answers, string title, object content)
        {
            this.SetupHeader(Scheduler, string.Format("{0} - {1}", dialogType, title));
            Content = content;

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