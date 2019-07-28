using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryMetaMessage : Entry
    {
        public string Message { get; }

        public EntryMetaMessage(DateTime timestamp, Event @event, string message) : base(timestamp, @event)
        {
            this.Message = message;
        }
    }
}
