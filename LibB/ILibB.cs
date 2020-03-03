using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibB
{
    public interface ILibB
    {
        LibBModel GetById(int id);
        List<LibBModel> Paged(int pageSize, int pageNumber);
    }
}
