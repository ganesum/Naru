using System.Threading.Tasks;

namespace Naru.WPF.Dialog
{
    public interface IStandardDialog
    {
        Task<Answer> QuestionAsync(string title, string message);

        Task<Answer> QuestionAsync(string title, string message, params Answer[] possibleResponens);

        Task<Answer> WarningAsync(string title, string message);

        Task<Answer> WarningAsync(string title, string message, params Answer[] possibleResponens);

        Task<Answer> InformationAsync(string title, string message);

        Task<Answer> InformationAsync(string title, string message, params Answer[] possibleResponens);

        Answer Error(string title, string message);

        Task<Answer> ErrorAsync(string title, string message);

        Task<Answer> ErrorAsync(string title, string message, params Answer[] possibleResponens);
    }
}