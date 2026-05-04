using Microsoft.Diagnostics.Tracing;

namespace EtwExtractor.Mapper
{
    public readonly record struct RawEventStruct
    {
        public string EventName { get; init; }
        public DateTime Time { get; init; }
        public TraceEventLevel Level { get; init; }
        public int ProcessID { get; init; }
        public string ProcessName { get; init; }
        public Guid ProviderGuid { get; init; }
        public string ProviderName { get; init; }
        public string MachineName { get; init; }
    }
}
