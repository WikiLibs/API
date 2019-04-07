using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.DTO
{
    public interface IGetDTO<DataModel>
    {
        void Map(in DataModel model);
    }
}
