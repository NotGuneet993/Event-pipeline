namespace Consumer.Models
{
    public class EtwRecordEntity
    {
        public long Id { get; init; }
        public required string EventName { get; init; }
        public required string Time { get; init; }
        public int Level { get; init; }
        public int ProcessID { get; init; }
        public required string ProcessName { get; init; }
        public required string ProviderGuid { get; init; }
        public required string ProviderName { get; init; }
        public required string MachineName { get; init; }
    }
}
