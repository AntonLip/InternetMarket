using InternetMarket.Interfaces;
using InternetMarket.Interfaces.IService;
using InternetMarket.Interfaces.Repository;
using System;
using System.Collections.Generic;

namespace InternetMarket.Services
{
    public class BaseService<TModel> : IServices<TModel, Guid>
        where TModel : class, IEntity<Guid>
    {
        protected readonly IRepository<TModel, Guid> _repository;

        public BaseService(IRepository<TModel, Guid> repository)
        {
            _repository = repository;
        }

        public virtual TModel Add(TModel model)
        {
            if (model is null)
                throw new ArgumentNullException();
            _repository.Add(model);
            return model;
        }

        public List<TModel> GetAll()
        {
            var products = _repository.GetAll();
            return products is null ? throw new ArgumentNullException() : (List<TModel>)products;  
        }
        public TModel GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException();

            var model = _repository.GetById(id);
            return model is null ? throw new ArgumentNullException() : model;
        }
        public TModel Remove(Guid modelId)
        {
            if (modelId == Guid.Empty)
                throw new ArgumentNullException();
            var product = _repository.GetById(modelId);
            if (product is null)
                throw new Exception("entity don't exist");
            _repository.Remove(product);
            return product;
        }

        public TModel Update(Guid modelId, TModel model)
        {
            if (modelId == Guid.Empty)
                throw new ArgumentNullException();
            if (model is null)
                throw new ArgumentNullException();
            if (modelId != model.Id)
                throw new ArgumentException();
            _repository.Update(model);

            return model;
        }
    }
}
