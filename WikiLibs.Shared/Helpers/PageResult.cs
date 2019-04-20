using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiLibs.Shared.Helpers
{
    public class PageResult<ModelType>
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public bool HasMorePages { get; set; }
        public IQueryable<ModelType> Data { get; set; }
    }
}
