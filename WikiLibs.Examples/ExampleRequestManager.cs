using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models.Examples;
using WikiLibs.Shared;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.Examples;

namespace WikiLibs.Examples
{
    public class ExampleRequestManager : BaseCRUDOperations<Context, ExampleRequest>, IExampleRequestManager
    {
        public ExampleRequestManager(Context ctx) : base(ctx)
        {
            MaxResults = 100;
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
                    await SaveChanges(); //EF Core refuses to handle multiple requests anymore
                    foreach (var code in ex.Data.Code)
                        ex.ApplyTo.Code.Add(code);
                }
                await SaveChanges();
                Set.Remove(ex);
                Context.Examples.Remove(ex.Data);
                await SaveChanges();
            }
        }

        public IQueryable<ExampleRequest> GetForSymbol(long symbol)
        {
            return (Set.Where(e => e.DataId != null).Where(e => e.Data.SymbolId == symbol));
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
            if (mdl.Type == ExampleRequestType.POST && (mdl.Data == null || mdl.ApplyToId != null))
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "POST",
                    ResourceName = mdl.Message,
                    ResourceType = typeof(ExampleRequest)
                };
            if (mdl.Type == ExampleRequestType.PATCH && (mdl.Data == null || mdl.ApplyToId == null))
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "PATCH",
                    ResourceName = mdl.Message,
                    ResourceType = typeof(ExampleRequest)
                };
            if (mdl.Type == ExampleRequestType.DELETE && (mdl.Data != null || mdl.ApplyToId == null))
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "DELETE",
                    ResourceName = mdl.Message,
                    ResourceType = typeof(ExampleRequest)
                };

            if (mdl.Data != null)
            {
                if (mdl.Data.User == null)
                    throw new Shared.Exceptions.InvalidResource()
                    {
                        PropertyName = "User",
                        ResourceName = mdl.Message,
                        ResourceType = typeof(ExampleRequest)
                    };
                if (mdl.Data.Symbol == null)
                    throw new Shared.Exceptions.InvalidResource()
                    {
                        PropertyName = "Symbol",
                        ResourceName = mdl.Message,
                        ResourceType = typeof(ExampleRequest)
                    };
                mdl.Data.Request = mdl;
            }
            await base.PostAsync(mdl);
            if (mdl.Data != null)
                mdl.Data.RequestId = mdl.Id;
            await SaveChanges();
            return (mdl);
        }

        public PageResult<ExampleRequest> GetAll(PageOptions options)
        {
            return (base.ToPageResult(options, Set.OrderByDescending(e => e.CreationDate)));
        }
    }
}
