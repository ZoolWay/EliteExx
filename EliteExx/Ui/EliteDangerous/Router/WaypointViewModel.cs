using System;
using Caliburn.Micro;

namespace Zw.EliteExx.Ui.EliteDangerous.Router
{
    public class WaypointViewModel : Screen
    {
        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string name;

        public string Name
        {
            get => this.name;
            set
            {
                if (String.Equals(value, this.name)) return;
                this.name = value;
                NotifyOfPropertyChange();
            }
        }

        public void Save()
        {
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
