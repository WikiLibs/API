using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.DTO
{
    public interface IPatchDTO<DataModel>
    {
        DataModel CreatePatch(in DataModel current);
    }
}
