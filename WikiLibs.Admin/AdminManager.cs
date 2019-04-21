using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Admin;
using WikiLibs.Shared.Service;

namespace WikiLibs.Admin
{
    [Module(typeof(IAdminManager))]
    public class AdminManager : IAdminManager
    {
        private readonly APIKeyManager _apiKeyManager;
        private readonly GroupManager _groupManager;

        public AdminManager(Data.Context ctx)
        {
            _groupManager = new GroupManager(ctx);
            _apiKeyManager = new APIKeyManager(ctx);
        }

        public IAPIKeyManager APIKeyManager => _apiKeyManager;

        public IGroupManager GroupManager => _groupManager;

        [ModuleInitializer(Debug = true)]
        public static void Initialize(IModuleManager modules, Data.Context ctx, ILogger logger)
        {
            var admin = modules.GetModule<IAdminManager>();

            if (!ctx.APIKeys.Any(x => x.Description == "[WIKILIBS_SUPER_DEV_API_KEY]"))
            {
                admin.APIKeyManager.PostAsync(new Data.Models.APIKey()
                {
                    Flags = AuthorizeApiKey.Authentication | AuthorizeApiKey.Registration | AuthorizeApiKey.Standard,
                    Description = "[WIKILIBS_SUPER_DEV_API_KEY]",
                    ExpirationDate = DateTime.MaxValue,
                    UseNum = -1
                }).Wait();
                logger.LogInformation("A development API Key has been generated");
            }
            var key = ctx.APIKeys.FirstOrDefault(x => x.Description == "[WIKILIBS_SUPER_DEV_API_KEY]");
            logger.LogWarning("Development API Key : " + key.Id);
            logger.LogWarning("This development key is intended for development purposes, it has ALL flags enabled and will never expire");
        }
    }
}
