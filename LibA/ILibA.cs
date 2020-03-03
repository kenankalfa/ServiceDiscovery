using Models;
using System;
using System.Collections.Generic;

namespace LibA
{
    public interface ILibA
    {
        LibAModel GetById(int id);
        List<LibAModel> Paged(int pageSize, int pageNumber);
    }
}
