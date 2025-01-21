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

        private void OpenConnectionIfNeeded()
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
            }
        }

        protected IDbTransaction BeginTransaction()
        {
            OpenConnectionIfNeeded();
            return _dbConnection.BeginTransaction();
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            OpenConnectionIfNeeded();
            return await _dbConnection.QueryFirstOrDefaultAsync<T>(sql, parameters, transaction);
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            OpenConnectionIfNeeded();
            return await _dbConnection.QueryAsync<T>(sql, parameters, transaction);
        }

        protected async Task ExecuteAsync(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            OpenConnectionIfNeeded();
            await _dbConnection.ExecuteAsync(sql, parameters, transaction);
        }

        protected async Task<int> ExecuteAndReturnAffectedRowsAsync(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            OpenConnectionIfNeeded();
            return await _dbConnection.ExecuteAsync(sql, parameters, transaction);
        }

        protected async Task CommitTransactionAsync(IDbTransaction transaction)
        {
            await Task.Run(() => transaction.Commit());
        }

        protected async Task RollbackTransactionAsync(IDbTransaction transaction)
        {
            await Task.Run(() => transaction.Rollback());
        }
    }
}
