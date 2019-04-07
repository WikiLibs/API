using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.DTO
{
    public interface IPostDTO<DataModel>
    {
        DataModel CreateNew();
    }
}
