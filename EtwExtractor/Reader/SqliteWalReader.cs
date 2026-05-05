using EtwExtractor.WriteAheadLog;
using Microsoft.Data.Sqlite;
using Microsoft.Diagnostics.Utilities;
using System.Runtime.CompilerServices;

namespace EtwExtractor.Reader
{
    public class SqliteWalReader : IReader<EtwRecordEntity>, IDisposable
    {
        private SqliteConnection connection;
        private long offset;
        private bool disposed;

        public SqliteWalReader()
        {
            disposed = false;
            var wal = SqliteWal.Instance;
            connection = (SqliteConnection)wal.GetConnection();
            offset = GetOffset();
        }

        public async IAsyncEnumerable<IReadOnlyList<EtwRecordEntity>> ReadAsync([EnumeratorCancellation]CancellationToken ct = default)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(250));

            while (!ct.IsCancellationRequested)
            {
                await timer.WaitForNextTickAsync(ct);
                var batch = FetchBatch();

                if (batch.Count == 0) continue;

                yield return batch;
                offset = batch[^1].Id;

                // keep flushing
                while (batch.Count == 250)
                {
                    batch = FetchBatch();
                    if (batch.Count == 0) break;
                    yield return batch;
                    offset = batch[^1].Id;
                }
            }
        }

        private long GetOffset()
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT offset FROM etw_kafka_offset LIMIT 1;";
            var result = cmd.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                cmd.CommandText = "SELECT MIN(id) FROM etw_events WHERE id > @lastOffset;";
                cmd.Parameters.AddWithValue("@lastOffset", offset);
                result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    return 0;
                }
            }

            return Convert.ToInt64(result);
        }

        private IReadOnlyList<EtwRecordEntity> FetchBatch()
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
               SELECT id, timestamp, event_name, provider_name, provider_guid,
                    process_id, process_name, level, machine_name
                FROM etw_events
                WHERE id > @lastOffset
                ORDER BY id
                LIMIT 250";
            cmd.Parameters.AddWithValue("@lastOffset", offset);

            var readList = new List<EtwRecordEntity>();
            using var reader = cmd.ExecuteReader();

            // ordinals
            var idOrd = reader.GetOrdinal("id");
            var eventNameOrd = reader.GetOrdinal("event_name");
            var timestampOrd = reader.GetOrdinal("timestamp");
            var levelOrd = reader.GetOrdinal("level");
            var processIdOrd = reader.GetOrdinal("process_id");
            var processNameOrd = reader.GetOrdinal("process_name");
            var providerGuidOrd = reader.GetOrdinal("provider_guid");
            var providerNameOrd = reader.GetOrdinal("provider_name");
            var machineNameOrd = reader.GetOrdinal("machine_name");

            while (reader.Read())
            {
                readList.Add(new EtwRecordEntity
                {
                    Id = reader.GetInt64(idOrd),
                    EventName = reader.GetString(eventNameOrd),
                    Time = reader.GetString(timestampOrd),
                    Level = reader.GetInt32(levelOrd),
                    ProcessID = reader.GetInt32(processIdOrd),
                    ProcessName = reader.GetString(processNameOrd),
                    ProviderGuid = reader.GetString(providerGuidOrd),
                    ProviderName = reader.GetString(providerNameOrd),
                    MachineName = reader.GetString(machineNameOrd)
                });
            }

            return readList.AsReadOnly();
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            connection.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }
        
    }
}
