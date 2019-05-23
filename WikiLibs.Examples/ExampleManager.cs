﻿using System;
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
            return (Set.Where(e => e.SymbolId == symbol).Where(e => e.RequestId == null));
        }

        public override async Task<Example> PatchAsync(long key, Example mdl)
        {
            var ex = await GetAsync(key);

            ex.LastModificationDate = mdl.LastModificationDate;
            ex.Description = mdl.Description;
            Context.RemoveRange(ex.Code);
            foreach (var code in mdl.Code)
            {
                ex.Code.Add(code);
            }
            await SaveChanges();
            return (ex);
        }
    }
}
