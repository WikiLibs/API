using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiLibs.Shared.Modules
{
    public interface IExampleManager : ICRUDOperations<Data.Models.Examples.Example>
    {
        IQueryable<Data.Models.Examples.Example> GetForSymbolAsync(int symbol);
    }
}
