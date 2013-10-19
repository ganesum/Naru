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
        private readonly I$fileinputname$Service _service;

        public $fileinputname$ViewModel(ILog log, IScheduler scheduler, IViewService viewService, IDataSourceSelectorService service)
            : base(log, scheduler, viewService)
        {
            _service = service;

            Disposables.Add(service);
        }
    }
}