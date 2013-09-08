using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Naru.WPF.MVVM;
using Common.Logging;

namespace $rootnamespace$.$fileinputname$
{
    public class $fileinputname$ViewModel : Workspace
    {
        private I$fileinputname$Service _service;

        public $fileinputname$ViewModel(ILog log, IDispatcherService dispatcherService, I$fileinputname$Service service)
            : base(log, dispatcherService)
        {
            _service = service;

            Disposables.Add(service);
        }
    }
}