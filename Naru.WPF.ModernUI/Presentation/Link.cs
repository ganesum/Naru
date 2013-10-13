using System.Windows.Input;

namespace Naru.WPF.ModernUI.Presentation
{
    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class Link : Displayable
    {
        public ICommand Command { get; set; }
    }
}