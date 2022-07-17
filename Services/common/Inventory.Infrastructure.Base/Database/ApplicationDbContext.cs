using Inventory.Domain.Base.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Inventory.Infrastructure.Base.Database
{
    public abstract class ApplicationDbContext : DbContext, IUnitOfWork 
    {
        protected ApplicationDbContext() : base() { }

        protected ApplicationDbContext(DbContextOptions options) : base(options) { }

        #region Transactions
        private IDbContextTransaction _currentTransaction;
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            //_currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            _currentTransaction = await Database.BeginTransactionAsync();

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion


        #region IUnitOfWork

        #endregion

    }
}
