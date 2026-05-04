using Microsoft.Diagnostics.Tracing;

namespace EtwExtractor.Mapper
{
    public class EventToStructMapper : IMapper<TraceEvent, RawEventStruct>
    {
        public RawEventStruct Convert(TraceEvent evt)
        {
            return new RawEventStruct
            {
                EventName       = evt.EventName,
                Time            = evt.TimeStamp,
                Level           = TraceEventLevel.Informational,
                ProcessID       = evt.ProcessID,
                ProcessName     = evt.ProcessName,
                ProviderGuid    = evt.ProviderGuid,
                ProviderName    = evt.ProviderName,
                MachineName     = Environment.MachineName
            };
        }
    }
}
