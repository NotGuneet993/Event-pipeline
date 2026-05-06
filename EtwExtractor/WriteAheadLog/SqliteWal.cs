using Microsoft.Data.Sqlite;
using System.Data;

namespace EtwExtractor.WriteAheadLog
{
    public class SqliteWal : IWalSingleton
    {
        private static readonly Lazy<SqliteWal> db_instance =
            new Lazy<SqliteWal>(() => new SqliteWal());

        public static SqliteWal Instance => db_instance.Value;
        private readonly string connectionString;

        private SqliteWal()
        {
            connectionString = "Data Source=wal.db";
            InitializeDatabase();
        }

        public IDbConnection GetConnection()
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var walPragma = connection.CreateCommand();
            walPragma.CommandText = "PRAGMA journal_mode=WAL;";
            walPragma.ExecuteNonQuery();

            using var createTable = connection.CreateCommand();
            createTable.CommandText = @"
                CREATE TABLE IF NOT EXISTS etw_events (
                    id              INTEGER PRIMARY KEY,
                    timestamp       TEXT NOT NULL,
                    event_name      TEXT NOT NULL,
                    provider_name   TEXT NOT NULL,
                    provider_guid   TEXT NOT NULL,
                    process_id      INTEGER NOT NULL,
                    process_name    TEXT NOT NULL,
                    level           INTEGER NOT NULL,
                    machine_name    TEXT NOT NULL
                );

                CREATE INDEX IF NOT EXISTS idx_etw_events_timestamp 
                    ON etw_events(timestamp);

                CREATE TABLE IF NOT EXISTS etw_kafka_offset (
                    id          INTEGER PRIMARY KEY,
                    offset      INTEGER NOT NULL,
                    updated_at  TEXT NOT NULL
                );";

            createTable.ExecuteNonQuery();

            using var retention = connection.CreateCommand();
            retention.CommandText = "DELETE FROM etw_events WHERE timestamp < @cutoff;";
            retention.Parameters.AddWithValue("@cutoff",
                DateTime.UtcNow.AddDays(-30).ToString("O"));
            retention.ExecuteNonQuery();
        }
    }
}
