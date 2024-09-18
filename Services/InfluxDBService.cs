﻿using System;
using System.Threading.Tasks;
using InfluxDB.Client;
using Microsoft.Extensions.Configuration;

namespace InfluxDBTest.Services
{
    public class InfluxDBService
    {
        private readonly string _token;

        public InfluxDBService(IConfiguration configuration)
        {
            _token = configuration.GetValue<string>("InfluxDB:Token");
        }

        public void Write(Action<WriteApi> action)
        {
            using var client = InfluxDBClientFactory.Create("url here", _token);
            using var write = client.GetWriteApi();
            action(write);
        }

        public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
        {
            using var client = InfluxDBClientFactory.Create("url here", _token);
            var query = client.GetQueryApi();
            return await action(query);
        }
    }
}