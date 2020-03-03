using System;
using System.Collections.Generic;
using LibRedis;
using Models;
using System.Linq;

namespace LibB
{
    public class LibB : ILibB
    {
        private RedisDataAgent _redis;
        private static readonly string _cacheKey = "city-data";
        private static List<LibBModel> _cached;
        public LibB()
        {
            _redis = new RedisDataAgent();
            Initialize();
        }

        private void Initialize()
        {
            if (_redis.HasKey(_cacheKey))
            {
                return;
            }

            var models = new List<LibBModel>();

            for (int i = 0; i < 1000; i++)
            {
                var model = new LibBModel();
                model.Id = i + 1;
                model.City = Faker.Address.City();

                models.Add(model);
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(models);

            _redis.SetStringValue(_cacheKey, json);
        }

        public LibBModel GetById(int id)
        {
            CachedObjectControl();

            var instance = _cached.FirstOrDefault(q => q.Id == id);

            return instance;
        }

        public List<LibBModel> Paged(int pageSize, int pageNumber)
        {
            CachedObjectControl();
            return _cached.Skip(pageSize * pageNumber).Take(pageSize).ToList();
        }

        private void CachedObjectControl()
        {
            if (_cached == null)
            {
                _cached = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LibBModel>>(_redis.GetStringValue(_cacheKey));
            }
        }
    }
}
