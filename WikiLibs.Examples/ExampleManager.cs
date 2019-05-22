using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models.Examples;
using WikiLibs.Shared;
using WikiLibs.Shared.Modules.Examples;

namespace WikiLibs.Examples
{
    class ExampleManager : BaseCRUDOperations<Context, Example>, IExampleManager
    {
        public ExampleManager(Context ctx) : base(ctx)
        {
        }

        public IQueryable<Example> GetForSymbolAsync(int symbol)
        {
            throw new NotImplementedException();
        }

        public override Task<Example> PatchAsync(long key, Example mdl)
        {
            throw new NotImplementedException();
        }
    }
}
