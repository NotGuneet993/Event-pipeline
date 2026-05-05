using EtwExtractor.Mapper;
using EtwExtractor.WriteAheadLog;
using Microsoft.Data.Sqlite;
using System.Data;

namespace EtwExtractor.Writer
{
    public class SqliteWalWriter : IWriter<RawEventStruct>, IDisposable
    {
        private SqliteCommand command;
        private bool disposed;

        public SqliteWalWriter()
        {
            disposed = false;
            var wal = SqliteWal.Instance;
            using (var connection = wal.GetConnection())
            { 
                command = (SqliteCommand)connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO etw_events 
                        (timestamp, event_name, provider_name, provider_guid, 
                         process_id, process_name, level, machine_name)
                    VALUES 
                        (@timestamp, @eventName, @providerName, @providerGuid,
                         @processId, @processName, @level, @machineName);";

                command.Parameters.Add("@timestamp", SqliteType.Text);
                command.Parameters.Add("@eventName", SqliteType.Text);
                command.Parameters.Add("@providerName", SqliteType.Text);
                command.Parameters.Add("@providerGuid", SqliteType.Text);
                command.Parameters.Add("@processId", SqliteType.Integer);
                command.Parameters.Add("@processName", SqliteType.Text);
                command.Parameters.Add("@level", SqliteType.Integer);
                command.Parameters.Add("@machineName", SqliteType.Text);

                command.Prepare();
            }
        }

        public bool Write(ref RawEventStruct obj)
        {
            try
            {
                command.Parameters["@eventName"].Value = obj.EventName;
                command.Parameters["@timestamp"].Value = obj.Time.ToString("O");
                command.Parameters["@level"].Value = obj.Level;
                command.Parameters["@processId"].Value = obj.ProcessID;
                command.Parameters["@processName"].Value = obj.ProcessName;
                command.Parameters["@providerGuid"].Value = obj.ProviderGuid;
                command.Parameters["@providerName"].Value = obj.ProviderName;
                command.Parameters["@machineName"].Value = obj.MachineName;

                command.ExecuteNonQuery();
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            command.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
