using LibConsulClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SrvMain.Controllers
{
    [Route("api/default")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private IConsulWrapper _consulWrapper;
        private IConfiguration _configuration;
        public DefaultController(IConsulWrapper consulWrapper, IConfiguration configuration)
        {
            _consulWrapper = consulWrapper;
            _configuration = configuration;
        }

        [HttpGet("Person/ById/{id}")]
        public async Task<JsonResult> PersonGetById(int id)
        {
            var tag = _configuration["harmony-scope:Person:Tag"];
            var resource = _configuration["harmony-scope:Person:Controller"] + "/" + string.Format(_configuration["harmony-scope:Person:ById"], id);

            var baseUrl = await _consulWrapper.GetUrl(tag);

            var baseUri = new Uri(baseUrl);
            var restClient = new RestClient(baseUri);

            var restRequest = new RestRequest(Method.GET);
            restRequest.Resource = resource;

            var response = default(IRestResponse);

            response = await Policy
                                .HandleResult<IRestResponse>(message => !message.IsSuccessful)
                                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), async (result, timeSpan, retryCount, context) =>
                                {
                                    baseUrl = await _consulWrapper.GetUrl(tag, baseUrl);
                                    restClient.BaseUrl = new Uri(baseUrl);
                                })
                                .ExecuteAsync(() => restClient.ExecuteAsync(restRequest));

            return new JsonResult(JsonConvert.DeserializeObject(response.Content));
        }

        [HttpGet("Person/Paged/{pageSize}/{pageNumber}")]
        public async Task<JsonResult> PersonPaged(int pageSize, int pageNumber)
        {
            var tag = _configuration["harmony-scope:Person:Tag"];
            var resource = _configuration["harmony-scope:Person:Controller"] + "/" + string.Format(_configuration["harmony-scope:Person:Paged"], pageSize, pageNumber);

            var baseUrl = await _consulWrapper.GetUrl(tag);

            var baseUri = new Uri(baseUrl);
            var restClient = new RestClient(baseUri);

            var restRequest = new RestRequest(Method.GET);
            restRequest.Resource = resource;

            var response = default(IRestResponse);

            response = await Policy
                                .HandleResult<IRestResponse>(message => !message.IsSuccessful)
                                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), async (result, timeSpan, retryCount, context) =>
                                {
                                    baseUrl = await _consulWrapper.GetUrl(tag, baseUrl);
                                    restClient.BaseUrl = new Uri(baseUrl);
                                })
                                .ExecuteAsync(() => restClient.ExecuteAsync(restRequest));

            return new JsonResult(JsonConvert.DeserializeObject(response.Content));
        }

        [HttpGet("City/ById/{id}")]
        public async Task<JsonResult> CityGetById(int id)
        {
            var tag = _configuration["harmony-scope:City:Tag"];
            var resource = _configuration["harmony-scope:City:Controller"] + "/" + string.Format(_configuration["harmony-scope:City:ById"], id);

            var baseUrl = await _consulWrapper.GetUrl(tag);

            var baseUri = new Uri(baseUrl);
            var restClient = new RestClient(baseUri);

            var restRequest = new RestRequest(Method.GET);
            restRequest.Resource = resource;

            var response = default(IRestResponse);

            response = await Policy
                                .HandleResult<IRestResponse>(message => !message.IsSuccessful)
                                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), async (result, timeSpan, retryCount, context) =>
                                {
                                    baseUrl = await _consulWrapper.GetUrl(tag, baseUrl);
                                    restClient.BaseUrl = new Uri(baseUrl);
                                })
                                .ExecuteAsync(() => restClient.ExecuteAsync(restRequest));

            return new JsonResult(JsonConvert.DeserializeObject(response.Content));
        }

        [HttpGet("City/Paged/{pageSize}/{pageNumber}")]
        public async Task<JsonResult> CityPaged(int pageSize, int pageNumber)
        {
            var tag = _configuration["harmony-scope:City:Tag"];
            var resource = _configuration["harmony-scope:City:Controller"] + "/" + string.Format(_configuration["harmony-scope:City:Paged"], pageSize, pageNumber);

            var baseUrl = await _consulWrapper.GetUrl(tag);

            var baseUri = new Uri(baseUrl);
            var restClient = new RestClient(baseUri);

            var restRequest = new RestRequest(Method.GET);
            restRequest.Resource = resource;

            var response = default(IRestResponse);

            response = await Policy
                                .HandleResult<IRestResponse>(message => !message.IsSuccessful)
                                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), async (result, timeSpan, retryCount, context) =>
                                {
                                    baseUrl = await _consulWrapper.GetUrl(tag, baseUrl);
                                    restClient.BaseUrl = new Uri(baseUrl);
                                })
                                .ExecuteAsync(() => restClient.ExecuteAsync(restRequest));

            return new JsonResult(JsonConvert.DeserializeObject(response.Content));
        }
    }
}