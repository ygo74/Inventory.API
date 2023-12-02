using Ardalis.Specification;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Models;
using Inventory.Common.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Database
{
    public class GenericQueryStore<TDbContext, T> : IGenericQueryStore<T>, IAsyncDisposable, IDisposable where T : Entity where TDbContext : DbContext
    {
        protected readonly TDbContext _dbContext;
        protected readonly IDbContextFactory<TDbContext> _dbContextFactory;
        protected readonly IMapper _mapper;

        public GenericQueryStore(TDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public GenericQueryStore(IDbContextFactory<TDbContext> dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContextFactory.CreateDbContext();
            _mapper = mapper;
        }

        #region IDisposable Support
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext?.Dispose();
            }
        }
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_dbContext is not null)
            {
                await _dbContext.DisposeAsync().ConfigureAwait(false);
            }
        }
        #endregion


        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>()
                                    .AsNoTracking()
                                    .Where(x => x.Id == id)
                                    .FirstOrDefaultAsync();
        }

        public Task<TDtoEntity> GetByIdAsync<TDtoEntity>(int id) where TDtoEntity : class
        {
            return _dbContext.Set<T>()
                              .AsNoTracking()
                              .Where(x => x.Id == id)
                              .ProjectTo<TDtoEntity>(_mapper.ConfigurationProvider)
                              .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<T>> ListAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsNoTracking();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<TDtoEntity>> ListAllAsync<TDtoEntity>(params Expression<Func<T, object>>[] includes) where TDtoEntity : class
        {
            var query = _dbContext.Set<T>().AsNoTracking();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query
                    .ProjectTo<TDtoEntity>(_mapper.ConfigurationProvider)
                    .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByCriteriaAsync(IExpressionFilter<T> criteria = null)
        {
            var dbSet = _dbContext.Set<T>()
                                  .AsNoTracking();

            if (criteria is not null && criteria.Predicate is not null)
            {
                dbSet = dbSet.Where(criteria.Predicate);
            }

            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<TDtoEntity>> GetByCriteriaAsync<TDtoEntity>(IExpressionFilter<T> criteria = null) where TDtoEntity : class
        {
            var dbSet = _dbContext.Set<T>()
                                  .AsNoTracking();

            if (criteria is not null && criteria.Predicate is not null)
            {
                dbSet = dbSet.Where(criteria.Predicate);
            }

            return await dbSet.ProjectTo<TDtoEntity>(_mapper.ConfigurationProvider)
                        .ToListAsync(); 


        }

    }
}
