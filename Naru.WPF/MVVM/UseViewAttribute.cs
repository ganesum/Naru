using System;

namespace Naru.WPF.MVVM
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class UseViewAttribute : Attribute
    {
        public Type ViewType { get; private set; }

        public UseViewAttribute(Type viewType)
        {
            ViewType = viewType;
        }
    }
}