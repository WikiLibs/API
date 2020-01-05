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
    public class ExampleManager : BaseCRUDOperations<Context, Example>, IExampleManager
    {
        public ExampleManager(Context ctx) : base(ctx)
        {
        }

        public override IQueryable<Example> OrderBy(IQueryable<Example> models)
        {
            return (models.OrderBy(e => e.CreationDate));
        }

        public IQueryable<Example> GetForSymbol(long symbol)
        {
            return (Get(e => e.SymbolId == symbol).Where(e => e.RequestId == null));
        }

        public override async Task<Example> PostAsync(Example mdl)
        {
            if (mdl.User == null)
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "User",
                    ResourceName = mdl.Description,
                    ResourceType = typeof(Example)
                };
            if (mdl.Symbol == null)
                throw new Shared.Exceptions.InvalidResource()
                {
                    PropertyName = "Symbol",
                    ResourceName = mdl.Description,
                    ResourceType = typeof(Example)
                };
            return await base.PostAsync(mdl);
        }

        public override async Task<Example> PatchAsync(long key, Example mdl)
        {
            var ex = await GetAsync(key);

            ex.LastModificationDate = mdl.LastModificationDate;
            ex.Description = mdl.Description;
            if (mdl.Code != null)
            {
                Context.RemoveRange(ex.Code);
                foreach (var code in mdl.Code)
                {
                    ex.Code.Add(code);
                }
            }
            await SaveChanges();
            return (ex);
        }
    }
}
