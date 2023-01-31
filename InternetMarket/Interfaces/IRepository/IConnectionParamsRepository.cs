using System;
using System.Collections.Generic;
using InternetMarket.Models.DbModels;

namespace InternetMarket.Interfaces.Repository;

public interface IConnectionParamsRepository : IRepository<ConnectionParams, Guid>
{
        ConnectionParams GetByHash(string hash);
        List<ConnectionParams> GetAllConnectionsByHash(string hash);
        List<ConnectionParams> GetAllConnectionsByIpAddr(string ip);
}