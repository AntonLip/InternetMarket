using System;
using InternetMarket.Models.DbModels;
using Microsoft.AspNetCore.Http;

namespace InternetMarket.Interfaces.IService;

public interface IConnectionParamsService : IServices<ConnectionParams, Guid>
{
    ConnectionParams AddFromContext(HttpContext context);
}