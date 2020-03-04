using LibA;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;

namespace SrvAi2.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private ILibA _dataSource;
        private static string _apiSource = "Person-Source2";
        public PersonController(ILibA dataSource)
        {
            _dataSource = dataSource;
        }

        [HttpGet("ById/{id}")]
        public Response<LibAModel> GetById(int id)
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

            Response<LibAModel> response = new Response<LibAModel>();

            response.Result = _dataSource.GetById(id);
            response.Source = _apiSource;

            return response;
        }

        [HttpGet("Paged/{pageSize}/{pageNumber}")]
        public Response<List<LibAModel>> Paged(int pageSize, int pageNumber)
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

            Response<List<LibAModel>> response = new Response<List<LibAModel>>();

            response.Result = _dataSource.Paged(pageSize, pageNumber);
            response.Source = _apiSource;

            return response;
        }
    }
}