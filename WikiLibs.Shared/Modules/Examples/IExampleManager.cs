using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiLibs.Shared.Modules.Examples
{
    public interface IExampleManager : ICRUDOperations<Data.Models.Examples.Example>
    {
        IQueryable<Data.Models.Examples.Example> GetForSymbolAsync(long symbol);
    }
}
