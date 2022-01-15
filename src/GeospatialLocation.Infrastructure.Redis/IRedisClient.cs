﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeospatialLocation.Infrastructure.Redis
{
    public interface IRedisClient
    {
        void AddToSet(string key, Guid id);
        Task SetAsync<T>(string key, T data);
        Task<IEnumerable<T>> GetSortedAsync<T>(string key, string get);
    }
}