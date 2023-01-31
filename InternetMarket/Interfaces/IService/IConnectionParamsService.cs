using System;
using InternetMarket.Models.DbModels;
using Microsoft.AspNetCore.Http;

namespace InternetMarket.Interfaces.IService;

public interface IConnectionParamsService : IServices<ConnectionParams, Guid>
{
    ConnectionParams AddFromContext(HttpContext context, string path);
    bool IsBotConnection(HttpContext context);
    void CheckCaptcha(HttpContext httpContext);
}