using System.Data;

namespace EtwExtractor.WriteAheadLog
{
    public interface IWalSingleton
    {
        public IDbConnection GetConnection();
    }
}
