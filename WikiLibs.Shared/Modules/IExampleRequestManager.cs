using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules
{
    public interface IExampleRequestManager : ICRUDOperations<Data.Models.Examples.ExampleRequest>
    {
        IQueryable<Data.Models.Examples.ExampleRequest> GetAll(int symbol);
        IQueryable<Data.Models.Examples.ExampleRequest> GetAll();
        Task ApplyRequest(int id);
    }
}
