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
    class ExampleRequestManager : BaseCRUDOperations<Context, ExampleRequest>, IExampleRequestManager
    {
        public ExampleRequestManager(Context ctx) : base(ctx)
        {
        }

        public async Task ApplyRequest(long key)
        {
            var ex = await GetAsync(key);

            if (ex.Type == ExampleRequestType.POST)
            {
                ex.Data.RequestId = null;
                ex.DataId = null;
                Set.Remove(ex);
                await SaveChanges();
            }
            else if (ex.Type == ExampleRequestType.DELETE)
            {
                Context.Examples.Remove(ex.ApplyTo);
                Set.Remove(ex);
                await SaveChanges();
            }
            else if (ex.Type == ExampleRequestType.PATCH)
            {
                ex.ApplyTo.LastModificationDate = ex.Data.LastModificationDate;
                ex.ApplyTo.Description = ex.Data.Description;
                if (ex.Data.Code != null)
                {
                    Context.RemoveRange(ex.ApplyTo.Code);
                    foreach (var code in ex.Data.Code)
                        ex.ApplyTo.Code.Add(code);
                }
                Set.Remove(ex);
                Context.Examples.Remove(ex.Data);
                await SaveChanges();
            }
        }

        public IQueryable<ExampleRequest> GetAll(long symbol)
        {
            return (Set.Where(e => e.DataId != null).Where(e => e.Data.SymbolId == symbol));
        }

        public IQueryable<ExampleRequest> GetAll()
        {
            return (Set);
        }

        public override async Task<ExampleRequest> PatchAsync(long key, ExampleRequest mdl)
        {
            var ex = await GetAsync(key);

            ex.Message = mdl.Message;
            if (ex.Data != null)
            {
                ex.Data.LastModificationDate = mdl.Data.LastModificationDate;
                ex.Data.Description = mdl.Data.Description;
                if (mdl.Data.Code != null)
                {
                    Context.RemoveRange(ex.Data.Code);
                    foreach (var code in mdl.Data.Code)
                        ex.Data.Code.Add(code);
                }
                await SaveChanges();
            }
            return (ex);
        }

        public override async Task<ExampleRequest> PostAsync(ExampleRequest mdl)
        {
            if (mdl.Data != null)
            {
                if (mdl.Data.User == null)
                    throw new Shared.Exceptions.InvalidResource()
                    {
                        PropertyName = "User",
                        ResourceName = mdl.Data.Description,
                        ResourceType = typeof(ExampleRequest)
                    };
                if (mdl.Data.Symbol == null)
                    throw new Shared.Exceptions.InvalidResource()
                    {
                        PropertyName = "Symbol",
                        ResourceName = mdl.Data.Description,
                        ResourceType = typeof(ExampleRequest)
                    };
            }
            return await base.PostAsync(mdl);
        }

    }
}
