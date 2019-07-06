using System;

namespace Zw.EliteExx.EliteDangerous.Journal
{
    public class EntryFileheader : Entry
    {
        public EntryFileheader(int part, string language, string gameversion, string build, DateTime timestamp, Event @event) : base(timestamp, @event)
        {
            this.Part = part;
            this.Language = language;
            this.Gameversion = gameversion;
            this.Build = build;
        }

        public int Part { get; }
        public string Language { get; }
        public string Gameversion { get; }
        public string Build { get; }
    }
}
