using System.Threading.Tasks;

namespace Naru.WPF.MVVM
{
    public interface ISupportInitialisation
    {
        Task OnInitialise();
    }
}