using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _ctx;
        private readonly DbSet<T> _set;

        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
            _set = _ctx.Set<T>();
        }

        public Task<T?> GetByIdAsync(object id, CancellationToken ct = default)
            => _set.FindAsync(new[] { id }, ct).AsTask();

        public IQueryable<T> Query() => _set.AsNoTracking();

        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            _set.Add(entity);
            await _ctx.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            await _ctx.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(T entity, CancellationToken ct = default)
        {
            _set.Remove(entity);
            await _ctx.SaveChangesAsync(ct);
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
            => _set.AnyAsync(predicate, ct);
    }
}
