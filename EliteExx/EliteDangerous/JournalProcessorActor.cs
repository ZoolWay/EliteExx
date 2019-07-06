using System;
using System.Collections.Generic;
using System.Globalization;
using Akka.Actor;
using Newtonsoft.Json.Linq;
using Zw.EliteExx.Core;
using Zw.EliteExx.EliteDangerous.Journal;

namespace Zw.EliteExx.EliteDangerous
{
    /// <summary>
    /// Processor for journal lines.
    /// </summary>
    internal class JournalProcessorActor : ReceiveActor
    {
        private readonly Akka.Event.ILoggingAdapter log = Akka.Event.Logging.GetLogger(Context);
        private readonly IActorRef connector;

        public JournalProcessorActor(IActorRef connector)
        {
            this.connector = connector;
            Receive((Action<ReaderMessage.ProcessLine>)ReceivedProcessLine);
        }

        private void ReceivedProcessLine(ReaderMessage.ProcessLine message)
        {
            log.Debug("Parsing new line");
            try
            {
                JObject o = JObject.Parse(message.Line);
                string tsText = o["timestamp"].Value<string>();
                string eventText = o["event"].Value<string>();
                if (String.IsNullOrWhiteSpace(tsText)) throw new Exception("Timestamp missing");
                if (String.IsNullOrWhiteSpace(eventText)) throw new Exception("Eventtype missing");
                Event @event = Event.Unknown;
                if (!Enum.TryParse<Event>(eventText, out @event))
                {
                    log.Warning($"Event '{eventText}' not supported (yet)");
                    @event = Event.Unknown;
                }
                DateTime timestamp = DateTime.Parse(tsText, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                Entry entry = CreateEntryFor(o, timestamp, @event);
                if (entry == null) return;
                this.connector.Tell(new ConnectorMessage.JournalEntry(entry));
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Failed to parse new line (index {message.Index} in {message.SourceFile})");
                this.connector.Tell(new ReaderMessage.ProcessError(message.Line, $"Failed to parse line index {message.Index} in '{message.SourceFile}': {ex.Message}"));
            }
        }

        private Entry CreateEntryFor(JObject o, DateTime timestamp, Event @event)
        {
            switch (@event)
            {
                case Event.Scan:
                    return CreateScanEntryFor(o, timestamp, @event);
                case Event.Location:
                    return o.ToObject<EntryLocation>();
                case Event.FSDJump:
                    return o.ToObject<EntryFsdJump>();
                case Event.FSSDiscoveryScan:
                    return o.ToObject<EntryFssDiscoveryScan>();
                case Event.FSSAllBodiesFound:
                    return o.ToObject<EntryFssAllBodiesFound>();
                case Event.Docked:
                    return o.ToObject<EntryDocked>();
                case Event.Undocked:
                    return o.ToObject<EntryUndocked>();
                case Event.Fileheader:
                    return o.ToObject<EntryFileheader>();

                default:
                    log.Warning($"Not (yet) support event type '{@event}' encountered");
                    return new Entry(timestamp, @event);
            }
        }

        private Entry CreateScanEntryFor(JObject o, DateTime timestamp, Event @event)
        {
            string scanTypeText = o["ScanType"].Value<string>();
            ScanType scanType = ScanType.Unknown;
            if (!Enum.TryParse<ScanType>(scanTypeText, out scanType))
            {
                log.Error($"Could not detect scan type for {scanTypeText}");
                scanType = ScanType.Unknown;
            }
            if (scanType == ScanType.AutoScan)
            {
                return o.ToObject<EntryScanAutoScan>();
            }
            else if (scanType == ScanType.Detailed)
            {
                return o.ToObject<EntryScanDetailed>();
            }
            return new Entry(timestamp, @event);
        }
    }
}
