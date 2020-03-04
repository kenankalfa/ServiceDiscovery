using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using LibB;
using Models;

namespace SrvB.Controllers
{
    [Route("api/city")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private ILibB _dataSource;
        private static string _apiSource = "City-Source1";
        public CityController(ILibB dataSource)
        {
            _dataSource = dataSource;
        }

        [HttpGet("ById/{id}")]
        public Response<LibBModel> GetById(int id)
        {
            #region exceptional code
            if (id > 1)
            {
                Random random = new Random();

                var next = random.Next(10000, 100000);

                if (next % 3 == 0)
                {
                    throw new Exception("good-bad-ugly");
                }
            } 
            #endregion

            Response<LibBModel> response = new Response<LibBModel>();

            response.Result = _dataSource.GetById(id);
            response.Source = _apiSource;

            return response;
        }

        [HttpGet("Paged/{pageSize}/{pageNumber}")]
        public Response<List<LibBModel>> Paged(int pageSize, int pageNumber)
        {
            #region exceptional code
            if (pageSize > 1)
            {
                Random random = new Random();

                var next = random.Next(10000, 100000);

                if (next % 3 == 0)
                {
                    throw new Exception("good-bad-ugly");
                }
            } 
            #endregion

            Response<List<LibBModel>> response = new Response<List<LibBModel>>();

            response.Result = _dataSource.Paged(pageSize, pageNumber);
            response.Source = _apiSource;

            return response;
        }
    }
}