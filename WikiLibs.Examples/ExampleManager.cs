using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models;
using WikiLibs.Data.Models.Examples;
using WikiLibs.Shared;
using WikiLibs.Shared.Modules.Examples;
using WikiLibs.Shared.Service;

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
            var data = await base.PostAsync(mdl);
            data.User.Points += ExampleRequestManager.NB_POINTS_ACCEPT;
            await SaveChanges();
            return data;
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

        public async Task UpVote(IUser user, long exampleId)
        {
            var tbl = await Context.ExampleVotes.FindAsync(new object[] { user.UserId, exampleId });

            if (tbl != null)
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceType = typeof(ExampleVote),
                    ResourceName = "ExampleVote",
                    ResourceId = exampleId.ToString(),
                    MissingPermission = "User has already voted"
                };
            var ex = await GetAsync(exampleId);
            ++ex.VoteCount;
            await SaveChanges();
            await Context.ExampleVotes.AddAsync(new ExampleVote()
            {
                UserId = user.UserId,
                ExampleId = exampleId
            });
            await SaveChanges();
        }

        public async Task DownVote(IUser user, long exampleId)
        {
            var tbl = await Context.ExampleVotes.FindAsync(new object[] { user.UserId, exampleId });

            if (tbl != null)
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceType = typeof(ExampleVote),
                    ResourceName = "ExampleVote",
                    ResourceId = exampleId.ToString(),
                    MissingPermission = "User has already voted"
                };
            var ex = await GetAsync(exampleId);
            --ex.VoteCount;
            await SaveChanges();
            await Context.ExampleVotes.AddAsync(new ExampleVote()
            {
                UserId = user.UserId,
                ExampleId = exampleId
            });
            await SaveChanges();
        }

        public bool HasAlreadyVoted(IUser user, long exampleId)
        {
            var tbl = Context.ExampleVotes.Find(new object[] { user.UserId, exampleId });

            return tbl != null;
        }
    }
}
