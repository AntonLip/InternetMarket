using System;
using InternetMarket.Interfaces.IService;
using InternetMarket.Interfaces.Repository;
using InternetMarket.Models.DbModels;
using Microsoft.AspNetCore.Http;

namespace InternetMarket.Services;

public class ConnectionParamsService : BaseService<ConnectionParams>, IConnectionParamsService
{
    public ConnectionParamsService(IConnectionParamsRepository repository)
    :base(repository)
    {
    }

    public ConnectionParams AddFromContext(HttpContext context)
    {
        ConnectionParams connectionParams = new ConnectionParams();
        connectionParams.RemoteIpAddr = context.Connection.RemoteIpAddress.ToString();
        connectionParams.RemotePort = context.Connection.RemotePort.ToString();
        connectionParams.ConnectionTime = DateTime.Now;
        connectionParams.UserAgent = context.Request.Headers.UserAgent.ToString();
        if (connectionParams.UserAgent.Contains("python"))
        {
            connectionParams.IsBot = true;
        }
        return base.Add(connectionParams);
    }

    public bool IsBotConnection()
    {
        throw new NotImplementedException();
    }
}