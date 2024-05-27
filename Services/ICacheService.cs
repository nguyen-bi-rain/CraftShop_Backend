namespace CraftShop.API.Services
{
    public interface ICacheService
    {
        T GetData<T>(string Key);
        bool SetData<T>(string key, T Value, DateTimeOffset ExpirationTime);
        object RemoveData(string key);
    }
}