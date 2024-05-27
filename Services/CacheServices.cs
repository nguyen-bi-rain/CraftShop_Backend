

using System.Security.Cryptography.Xml;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CraftShop.API.Services;

public class CacheServices : ICacheService
{
    private IDatabase _cacheDB;
    public CacheServices()
    {
        //var options = ConfigurationOptions.Parse("localhost:6379");
        //options.AbortOnConnectFail = false;
        //options.ConnectRetry =10;
        //options.ConnectTimeout = 5000;
        //options.EndPoints = { }

        var redis = ConnectionMultiplexer.Connect("redis-17234.c1.asia-northeast1-1.gce.redns.redis-cloud.com:17234,password=TMyCim6RJbvZwOuEmu9eWNTbTUjV9irw");
        _cacheDB = redis.GetDatabase();
    }
    public T GetData<T>(string Key)
    {
        var value = _cacheDB.StringGet(Key);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        return default;
    }

    public object RemoveData(string key)
    {
        var _exist = _cacheDB.KeyExists(key);
        if(_exist){
            return _cacheDB.KeyDelete(key);
        }
        return false;

    }

    public bool SetData<T>(string key, T Value, DateTimeOffset ExpirationTime)
    {
        
        var exprireTime = ExpirationTime.DateTime.Subtract(DateTime.Now);
        var isSet = _cacheDB.StringSet(key,JsonConvert.SerializeObject(Value),exprireTime);
        return isSet;
    }
}