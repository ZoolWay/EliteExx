using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Zw.EliteExx.EliteDangerous;

namespace Zw.EliteExx.Ui.EliteDangerous
{
    public class MainViewModel : Screen
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly IWindowManager windowManager;
        private string position;
        private BindableCollection<LogEvent> events;

        public MainViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            this.eventAggregator = eventAggregator;
            this.windowManager = windowManager;

            this.position = "-- waiting --";
            this.events = new BindableCollection<LogEvent>();
        }
    }
}
