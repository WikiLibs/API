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
    public class ExampleCommentManager : BaseCRUDOperations<Data.Context, ExampleComment>, IExampleCommentsManager
    {
        private Config _cfg;

        public ExampleCommentManager(Context ctx, Config cfg) : base(ctx)
        {
            _cfg = cfg;
        }

        public override IQueryable<ExampleComment> OrderBy(IQueryable<ExampleComment> models)
        {
            return (models.OrderByDescending(e => e.CreationDate));
        }

        public PageResult<ExampleComment> GetByExample(long example, PageOptions options)
        {
            options.EnsureValid(typeof(ExampleComment), "ExampleComment", _cfg.MaxExampleRequestsPerPage);
            return (ToPageResult(options, OrderBy(Set.Where(e => e.ExampleId == example))));
        }

        public override async Task<ExampleComment> PatchAsync(long key, ExampleComment mdl)
        {
            var mm = await GetAsync(key);
            mm.Data = mdl.Data;
            await SaveChanges();
            return (mm);
        }
    }
}
