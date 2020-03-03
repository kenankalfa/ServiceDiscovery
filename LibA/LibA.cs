using LibRedis;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibA
{
    public class LibA : ILibA
    {
        private RedisDataAgent _redis;
        private static readonly string _cacheKey = "person-data";
        private static List<LibAModel> _cached;
        public LibA()
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

            var models = new List<LibAModel>();

            for (int i = 0; i < 1000; i++)
            {
                var model = new LibAModel();
                model.Id = i + 1;
                model.PersonName = Faker.Name.FullName();

                models.Add(model);
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(models);

            _redis.SetStringValue(_cacheKey, json);
        }

        public LibAModel GetById(int id)
        {
            CachedObjectControl();

            var instance = _cached.FirstOrDefault(q => q.Id == id);

            return instance;
        }

        public List<LibAModel> Paged(int pageSize, int pageNumber)
        {
            CachedObjectControl();
            return _cached.Skip(pageSize * pageNumber).Take(pageSize).ToList();
        }

        private void CachedObjectControl()
        {
            if (_cached == null)
            {
                _cached = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LibAModel>>(_redis.GetStringValue(_cacheKey));
            }
        }
    }
}
