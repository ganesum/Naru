namespace Naru.WPF.Dialog
{
    public class DialogItemViewModel<T>
    {
        public T Response { get; set; }

        public bool IsDefault { get; set; }

        public bool IsCancel { get; set; }
    }
}