using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.DB
{
    public class RedisCore
    {
        private readonly string _connectionString;
        private readonly int _dbIndex;
        public readonly IDatabase _redisDB;


        public RedisCore(string connectionString, int dbIndex)
        {
            _connectionString = connectionString;
            _dbIndex = dbIndex;
            var conn = ConnectionMultiplexer.Connect(_connectionString);
            _redisDB = conn.GetDatabase(_dbIndex);
        }
    }
}
