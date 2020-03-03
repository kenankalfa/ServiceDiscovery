using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SrvAi2.Config;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SrvAi2.HostedServices
{
    public class ConsulRegisterHostedService : IHostedService
    {
        private readonly IServer _server;
        private readonly IConsulClient _consulClient;
        private readonly IOptions<ConsulConfig> _consulConfig;
        private string _registrationID;
        private CancellationTokenSource _cts;

        public ConsulRegisterHostedService(IConsulClient consulClient, IServer server, IOptions<ConsulConfig> consulConfig)
        {
            _server = server;
            _consulClient = consulClient;
            _consulConfig = consulConfig;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Random rnd = new Random();
            var rndValue = rnd.Next(1000, 9999).ToString();
            var portNumber = 51238;

            var features = _server.Features;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();

            if (!String.IsNullOrEmpty(address) && address.IndexOf("127.0.0.1") != -1)
            {
                address = address.Replace("127.0.0.1", "localhost");
            }

            var uri = new Uri(address);
            _registrationID = $"{_consulConfig.Value.ServiceID}--{rndValue}";

            var services = await _consulClient.Agent.Services();

            if (services != null && services.Response != null && services.Response.Any() && services.Response.Any(q => q.Value.Address == $"{uri.Scheme}://{uri.Host}" && q.Value.Port == portNumber))
            {
                return;
            }

            var registration = new AgentServiceRegistration()
            {
                ID = _registrationID,
                Name = _consulConfig.Value.ServiceName,
                Address = $"{uri.Scheme}://{uri.Host}",
                Port = portNumber,
                Tags = new[] { "harmony-v1", "person-v1" },
                Check = new AgentServiceCheck()
                {
                    HTTP = $"{uri.Scheme}://{uri.Host}:{portNumber}/api/health/status",
                    Timeout = TimeSpan.FromSeconds(3),
                    Interval = TimeSpan.FromSeconds(10)
                }
            };

            await _consulClient.Agent.ServiceDeregister(registration.ID, _cts.Token);
            await _consulClient.Agent.ServiceRegister(registration, _cts.Token);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();

            try
            {
                await _consulClient.Agent.ServiceDeregister(_registrationID, cancellationToken);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
