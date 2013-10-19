namespace Naru.WPF.ViewModel
{
    public interface ISupportHeader
    {
        IViewModel Header { get; }

        void SetupHeader(IViewModel headerViewModel);
    }
}