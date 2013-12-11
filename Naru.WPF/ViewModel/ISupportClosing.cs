namespace Naru.WPF.ViewModel
{
    public interface ISupportClosing
    {
        IClosingStrategy ClosingStrategy { get; }
    }
}