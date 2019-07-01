using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    /// <summary>
    /// Journal entry (base class without specific properties).
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("JournalEntry '{Event}' @ {Timestamp}")]
    public class Entry
    {
        public DateTime Timestamp { get; }
        public Event Event { get; }

        public Entry(DateTime timestamp, Event @event)
        {
            this.Timestamp = timestamp;
            this.Event = @event;
        }
    }
}
