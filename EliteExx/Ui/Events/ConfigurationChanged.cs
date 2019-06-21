using System;

namespace Zw.EliteExx.Ui.Events
{
    /// <summary>
    /// Published to notify UI elements that the configuration has changed.
    /// </summary>
    public class ConfigurationChanged
    {
        public Core.Config.Config NewConfiguration { get; }

        public ConfigurationChanged(Core.Config.Config newConfiguration)
        {
            this.NewConfiguration = newConfiguration;
        }
    }
}
