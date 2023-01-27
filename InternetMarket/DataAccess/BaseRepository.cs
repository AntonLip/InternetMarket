using InternetMarket.DataAccess;
using InternetMarket.Interfaces;
using InternetMarket.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Contributors.DataAccess
{
    internal class BaseRepository<TModel> : IRepository<TModel, Guid>
        where TModel : class, IEntity<Guid>
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TModel> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TModel>();
        }
        public void Add(TModel model)
        {
            _dbSet.Add(model);
            _context.SaveChanges();
        }

        public TModel GetFirst(Func<TModel, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).FirstOrDefault();
        }

        public IEnumerable<TModel> GetAll()
        {
            return _dbSet.AsNoTracking().OrderBy(l => l.Id).ToList();
        }

        public TModel GetById(Guid id)
        {
            return _dbSet.First(l => l.Id == id);
        }

        public void Remove(TModel model)
        {
            _context.Entry(model).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void Update(TModel model)
        {
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public List<TModel> GetWithInclude(params Expression<Func<TModel, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }
        public int GetCount()
        {
            return _dbSet.Count();
        }
        public List<TModel> GetWithInclude(Func<TModel, bool> predicate,
            params Expression<Func<TModel, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        private IQueryable<TModel> Include(params Expression<Func<TModel, object>>[] includeProperties)
        {
            IQueryable<TModel> query = _dbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
