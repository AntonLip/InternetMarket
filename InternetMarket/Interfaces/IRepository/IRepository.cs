using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InternetMarket.Interfaces.Repository
{
    public interface IRepository<TModel, TId>
            where TModel : IEntity<TId>
    {
        void Add(TModel obj);
        IEnumerable<TModel> GetAll();
        TModel GetById(TId id);
        void Update(TModel obj);
        public int GetCount();
        TModel GetFirst(Func<TModel, bool> predicate);
        List<TModel> GetWithInclude(params Expression<Func<TModel, object>>[] includeProperties);
        void Remove(TModel model);
        List<TModel> GetWithInclude(Func<TModel, bool> predicate,
            params Expression<Func<TModel, object>>[] includeProperties);

    }
}
