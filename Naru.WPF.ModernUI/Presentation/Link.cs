using Microsoft.Practices.Prism.Commands;

namespace Naru.WPF.ModernUI.Presentation
{
    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class Link : Displayable
    {
        public DelegateCommand Command { get; set; }
    }
}