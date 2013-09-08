namespace Naru.WPF.TPL
{
    public class Unit
    {
        private static readonly Unit _default = new Unit();

        public static Unit Default
        {
            get { return _default; }
        }
    }
}