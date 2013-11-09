using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Naru.TPL;

namespace Naru.WPF.Wizard
{
    public class WizardContext : IWizardContext
    {
        private readonly Subject<Unit> _canBeFinishedChangedSubject = new Subject<Unit>();
        private bool _canFinish;

        public IObservable<Unit> CanBeFinishedChanged { get { return _canBeFinishedChangedSubject.AsObservable(); } }

        public bool CanFinish
        {
            get { return _canFinish; }
            protected set
            {
                _canFinish = value;
                _canBeFinishedChangedSubject.OnNext(Unit.Default);
            }
        }
    }
}