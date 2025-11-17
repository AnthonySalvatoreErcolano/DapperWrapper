using System.Data;

namespace DapperWrapper.Core
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
