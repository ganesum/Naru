using System.Threading.Tasks;

namespace Naru.WPF.ViewModel
{
    public interface ISupportInitialisation
    {
        Task OnInitialise();
    }
}