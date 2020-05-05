using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data;
using WikiLibs.Data.Models;
using WikiLibs.Shared;
using WikiLibs.Shared.Modules.Admin;

namespace WikiLibs.Admin
{
    class ApiKeyManager : BaseCRUDOperations<Context, ApiKey, string>, IApiKeyManager
    {
        public ApiKeyManager(Context ctx) : base(ctx)
        {
        }

        public override async Task<ApiKey> PostAsync(ApiKey mdl)
        {
            mdl.Id = Guid.NewGuid().ToString();
            return (await base.PostAsync(mdl));
        }

        public bool Exists(string key)
        {
            return (Set.Any(x => x.Id == key));
        }

        public IQueryable<ApiKey> GetAll()
        {
            return (Set.AsQueryable());
        }

        public override async Task<ApiKey> PatchAsync(string key, ApiKey mdl)
        {
            var apiKey = await GetAsync(key);

            apiKey.Description = mdl.Description;
            apiKey.Origin = mdl.Origin;
            apiKey.Flags = mdl.Flags;
            apiKey.ExpirationDate = mdl.ExpirationDate;
            apiKey.UseNum = mdl.UseNum;
            await SaveChanges();
            return (apiKey);
        }

        public async Task Use(string key)
        {
            var apiKey = await GetAsync(key);

            if (apiKey.UseNum != -1 && apiKey.UseNum > 0)
                --apiKey.UseNum;
            if (apiKey.UseNum == 0 || DateTime.UtcNow > apiKey.ExpirationDate) // The API Key has expired
                await DeleteAsync(apiKey);
            await SaveChanges();
        }

        public IQueryable<ApiKey> GetAllOfDescription(string desc)
        {
            return (Set.Where(x => x.Description == desc));
        }
    }
}
