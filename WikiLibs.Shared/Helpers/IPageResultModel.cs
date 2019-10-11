using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Helpers
{
    public interface IPageResultModel<Me, DataModel>
        where Me : IPageResultModel<Me, DataModel>
    {
        Me Map(DataModel model);
    }
}
