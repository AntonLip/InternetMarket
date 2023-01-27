using InternetMarket.Models.DbModels;
using System;
using System.Collections.Generic;

namespace InternetMarket.Interfaces.IService
{
    public interface IServices<TModel, TId>
            where TModel : IEntity<TId>
    {
        TModel Add(TModel model);
        TModel Remove(Guid poductId);
        TModel Update(Guid poductId, TModel model);
        List<TModel> GetAll();
        TModel GetById(Guid Id);
    }
}
