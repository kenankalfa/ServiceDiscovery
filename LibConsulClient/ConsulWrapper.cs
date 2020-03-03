using Consul;
using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Linq;

namespace LibConsulClient
{
    public interface IConsulWrapper
    {
        Task<string> GetUrl(string tag, string exceptionalUrl="");
    }
    public class ConsulWrapper : IConsulWrapper
    {
        public ConcurrentBag<ServiceUrl> ServiceUrls { get; set; }
        private IConfiguration _configuration;
        private IConsulClient _consulClient;

        public ConsulWrapper(IConfiguration configuration, IConsulClient consulClient)
        {
            _configuration = configuration;
            _consulClient = consulClient;
        }

        public async Task Init()
        {
            ServiceUrls = new ConcurrentBag<ServiceUrl>();

            var services = await _consulClient.Agent.Services();
            foreach (var service in services.Response)
            {
                var isHarmonyScopedApi = service.Value.Tags.Any(t => t == "harmony-v1");
                if (isHarmonyScopedApi)
                {
                    var customTag = service.Value.Tags.Where(q => q != "harmony-v1").FirstOrDefault();
                    var url = $"{service.Value.Address}:{service.Value.Port}";

                    ServiceUrls.Add(new ServiceUrl() { Tag = customTag,Url = url });
                }
            }
        }

        public async Task<string> GetUrl(string tag,string exceptionalUrl="")
        {
            var returnValue = string.Empty;

            if (this.ServiceUrls == null || this.ServiceUrls.Count == 0)
            {
                await Init();
            }

            var serviceUrl = this.ServiceUrls.FirstOrDefault(q => q.Tag == tag && (!string.IsNullOrEmpty(exceptionalUrl) ? q.Url != exceptionalUrl : true));

            if (serviceUrl != null)
            {
                returnValue = serviceUrl.Url;
            }
            else
            {
                if (!String.IsNullOrEmpty(exceptionalUrl))
                {
                    returnValue = exceptionalUrl;
                }
            }

            return returnValue;
        }
    }
}
