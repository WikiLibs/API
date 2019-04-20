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
    class APIKeyManager : BaseCRUDOperations<Context, APIKey, string>, IAPIKeyManager
    {
        public APIKeyManager(Context ctx) : base(ctx)
        {
        }

        public override async Task<APIKey> PostAsync(APIKey mdl)
        {
            mdl.Id = Guid.NewGuid().ToString();
            return (await base.PostAsync(mdl));
        }

        public bool Exists(string key)
        {
            return (Set.Any(x => x.Id == key));
        }

        public IQueryable<APIKey> GetAll()
        {
            return (Set.AsQueryable());
        }

        public override async Task<APIKey> PatchAsync(string key, APIKey mdl)
        {
            var apiKey = await GetAsync(key);

            apiKey.Description = mdl.Description;
            apiKey.Flags = mdl.Flags;
            apiKey.ExpirationDate = mdl.ExpirationDate;
            apiKey.UseNum = mdl.UseNum;
            await SaveChanges();
            return (apiKey);
        }

        public async Task UseAPIKey(string key)
        {
            var apiKey = await GetAsync(key);

            if (apiKey.UseNum != -1 && apiKey.UseNum > 0)
                --apiKey.UseNum;
            if (apiKey.UseNum == 0 || DateTime.UtcNow > apiKey.ExpirationDate) // The API Key has expired
                await DeleteAsync(apiKey);
            await SaveChanges();
        }
    }
}
