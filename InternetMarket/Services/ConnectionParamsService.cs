using System;
using InternetMarket.Interfaces.IService;
using InternetMarket.Interfaces.Repository;
using InternetMarket.Models.DbModels;
using Microsoft.AspNetCore.Http;

namespace InternetMarket.Services;

public class ConnectionParamsService : BaseService<ConnectionParams>, IConnectionParamsService
{
    private readonly IConnectionParamsRepository _connectionParamsRepository;
    public ConnectionParamsService(IConnectionParamsRepository repository)
    :base(repository)
    {
        _connectionParamsRepository = repository;
    }
    
    public ConnectionParams AddFromContext(HttpContext context, string path)
    {
        ConnectionParams connectionParams = new ConnectionParams();
        connectionParams.RemoteIpAddr = context.Connection.RemoteIpAddress.ToString();
        connectionParams.RemotePort = context.Connection.RemotePort.ToString();
        connectionParams.ConnectionTime = DateTime.Now;
        connectionParams.Url = path;
        connectionParams.Hash = CreateMD5(context.Request.Headers.UserAgent.ToString());
        connectionParams.UserAgent = context.Request.Headers.UserAgent.ToString();
        
        return base.Add(connectionParams);
    }

    public bool IsBotConnection(HttpContext context)
    {
        try
        {
            AnalyzeConnection(context);
            var item = _connectionParamsRepository.GetByHash(CreateMD5(context.Request.Headers.UserAgent));
            
            return item.IsBot;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public void CheckCaptcha(HttpContext httpContext)
    {
        var item = _connectionParamsRepository.GetByHash(httpContext.Request.Headers.UserAgent);
        if (item.Url.Contains("Captcha"))
        {
            item.TimeToCaptch = (DateTime.Now - item.ConnectionTime).Milliseconds;
            _repository.Update(item);
        }
    }

    private bool AnalyzeConnection(HttpContext httpContext)
    {
        try
        {
            var items = _connectionParamsRepository.GetAllConnectionsByIpAddr(httpContext.Connection.RemoteIpAddress.ToString());
            
            foreach (var item in items)
            {
                if (item.UserAgent.Contains("python"))
                    item.IsBot = true;
                if (item.Hash != items[0].Hash)
                    item.IsBot = true;
                if((DateTime.Now - item.ConnectionTime).Milliseconds < 10)
                    item.IsBot = true;
                if (item.TimeToCaptch > 0 && item.TimeToCaptch < 3)
                    item.IsBot = true;
                _repository.Update(item);
            }

            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private string CreateMD5(string input)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}