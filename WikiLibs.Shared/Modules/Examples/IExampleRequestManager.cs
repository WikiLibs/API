using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared.Modules.Examples
{
    public interface IExampleRequestManager : ICRUDOperations<Data.Models.Examples.ExampleRequest>
    {
        IQueryable<Data.Models.Examples.ExampleRequest> GetForSymbol(long symbol);
        PageResult<Data.Models.Examples.ExampleRequest> GetAll(PageOptions options);
        Task ApplyRequest(long key);
    }
}
