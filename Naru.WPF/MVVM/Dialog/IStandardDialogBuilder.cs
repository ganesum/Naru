using System.Threading.Tasks;

namespace Naru.WPF.MVVM.Dialog
{
    public interface IStandardDialogBuilder
    {
        Answer Question(string title, string message);
        Task<Answer> QuestionAsync(string title, string message);

        Answer Question(string title, string message, params Answer[] possibleResponens);
        Task<Answer> QuestionAsync(string title, string message, params Answer[] possibleResponens);

        Answer Warning(string title, string message);
        Task<Answer> WarningAsync(string title, string message);

        Answer Warning(string title, string message, params Answer[] possibleResponens);
        Task<Answer> WarningAsync(string title, string message, params Answer[] possibleResponens);

        Answer Information(string title, string message);
        Task<Answer> InformationAsync(string title, string message);

        Answer Information(string title, string message, params Answer[] possibleResponens);
        Task<Answer> InformationAsync(string title, string message, params Answer[] possibleResponens);

        Answer Error(string title, string message);
        Task<Answer> ErrorAsync(string title, string message);

        Answer Error(string title, string message, params Answer[] possibleResponens);
        Task<Answer> ErrorAsync(string title, string message, params Answer[] possibleResponens);
    }
}