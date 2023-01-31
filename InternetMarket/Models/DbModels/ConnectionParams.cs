using System;
using InternetMarket.Interfaces;

namespace InternetMarket.Models.DbModels;

public class ConnectionParams : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string RemoteIpAddr { get; set; }
    public string UserAgent { get; set; }
    public DateTime ConnectionTime { get; set; }
    public int TimeToCaptch { get; set; }
    public string Hash { get; set; }
    public string Url { get; set; }
    public string RemotePort { get; set; }
    public bool IsBot { get; set; }
}