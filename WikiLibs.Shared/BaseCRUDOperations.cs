using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Helpers;

namespace WikiLibs.Shared
{
    public abstract class BaseCRUDOperations<DbContext, DataModel, KeyType> : ICRUDOperations<DataModel, KeyType>
        where DataModel : Data.Model<KeyType>
        where DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected DbSet<DataModel> Set { get; }
        protected DbContext Context { get; }
        public int MaxResults { get; set; } = 15;

        public BaseCRUDOperations(DbContext ctx)
        {
            Context = ctx;
            Set = Context.Set<DataModel>();
        }

        public async Task<int> SaveChanges()
        {
            try
            {
                return (await Context.SaveChangesAsync());
            }
            catch (DbUpdateException e)
            {
                await OnDbUpdateException(e);
                throw new Exceptions.InvalidResource()
                {
                    PropertyName = "",
                    ResourceName = typeof(DataModel).Name,
                    ResourceType = typeof(DataModel)
                };
            }
        }

        public virtual async Task<DataModel> DeleteAsync(DataModel mdl)
        {
            Set.Remove(mdl);
            await SaveChanges();
            return (mdl);
        }

        public virtual async Task<DataModel> DeleteAsync(KeyType key)
        {
            var mdl = await GetAsync(key);

            if (mdl == null)
                throw new Exceptions.ResourceNotFound()
                {
                    ResourceId = key.ToString(),
                    ResourceName = typeof(DataModel).Name,
                    ResourceType = typeof(DataModel)
                };
            return (await DeleteAsync(mdl));
        }

        public virtual async Task<DataModel> GetAsync(KeyType key)
        {
            var mdl = await Set.FindAsync(new object[] { key });

            if (mdl == null)
                throw new Exceptions.ResourceNotFound()
                {
                    ResourceId = key.ToString(),
                    ResourceName = typeof(DataModel).Name,
                    ResourceType = typeof(DataModel)
                };
            return (mdl);
        }
                
        public abstract Task<DataModel> PatchAsync(KeyType key, DataModel mdl);

        public virtual async Task<DataModel> PostAsync(DataModel mdl)
        {
            Set.Add(mdl);
            await SaveChanges();
            return (mdl);
        }

        public virtual Task OnDbUpdateException(DbUpdateException e)
        {
            return (Task.CompletedTask);
        }

        public virtual IQueryable<DataModel> OrderBy(IQueryable<DataModel> models)
        {
            return (models);
        }

        public virtual IQueryable<DataModel> Get(Expression<Func<DataModel, bool>> expression)
        {
            return (OrderBy(Set.Where(expression)));
        }

        public virtual PageResult<T> ToPageResult<T>(PageOptions options, IQueryable<DataModel> models)
            where T : IPageResultModel<T, DataModel>, new()
        {
            return (ToPageResult<T, DataModel>(options, models));
        }

        public virtual PageResult<DataModel> ToPageResult(PageOptions options, IQueryable<DataModel> models)
        {
            return (ToPageResult1(options, models));
        }

        public virtual PageResult<T> ToPageResult<T, DM>(PageOptions options, IQueryable<DM> models)
            where T : IPageResultModel<T, DM>, new()
        {
            options.EnsureValid(typeof(DM), typeof(DM).Name, MaxResults);
            var data = models.Skip((options.Page.Value - 1) * options.Count.Value);
            bool next = data.Count() > options.Count.Value;
            var arr = data.Take(options.Count.Value).Select(e => new T().Map(e));
            return (new PageResult<T>()
            {
                Data = arr,
                HasMorePages = next,
                Page = options.Page.Value,
                Count = options.Count.Value
            });
        }

        private PageResult<DM> ToPageResult1<DM>(PageOptions options, IQueryable<DM> models)
        {
            options.EnsureValid(typeof(DM), typeof(DM).Name, MaxResults);
            var data = models.Skip((options.Page.Value - 1) * options.Count.Value);
            bool next = data.Count() > options.Count.Value;
            var arr = data.Take(options.Count.Value);
            return (new PageResult<DM>()
            {
                Data = arr,
                HasMorePages = next,
                Page = options.Page.Value,
                Count = options.Count.Value
            });
        }
    }

    public abstract class BaseCRUDOperations<DbContext, DataModel> : BaseCRUDOperations<DbContext, DataModel, long>
        where DataModel : Data.Model<long>
        where DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public BaseCRUDOperations(DbContext ctx) : base(ctx)
        {
        }

        public override abstract Task<DataModel> PatchAsync(long key, DataModel mdl);
    }
}
