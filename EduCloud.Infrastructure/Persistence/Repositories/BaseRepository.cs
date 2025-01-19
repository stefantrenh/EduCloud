using Dapper;
using System.Data;

namespace EduCloud.Infrastructure.Persistence.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IDbConnection _dbConnection;

        protected BaseRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            return await _dbConnection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            return await _dbConnection.QueryAsync<T>(sql, parameters);
        }

        protected async Task ExecuteAsync(string sql, object parameters = null)
        {
            await _dbConnection.ExecuteAsync(sql, parameters);
        }
    }
}
