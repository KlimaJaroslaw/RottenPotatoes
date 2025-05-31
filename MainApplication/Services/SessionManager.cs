using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json;

namespace RottenPotatoes.Services
{
    public class SessionManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Set<T>(string key, T value)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public T Get<T>(string key)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public void Remove(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(key);
        }
    }

}

