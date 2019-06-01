﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules.Examples
{
    public interface IExampleRequestManager : ICRUDOperations<Data.Models.Examples.ExampleRequest>
    {
        IQueryable<Data.Models.Examples.ExampleRequest> GetAll(long symbol);
        IQueryable<Data.Models.Examples.ExampleRequest> GetAll();
        Task ApplyRequest(long key);
    }
}
