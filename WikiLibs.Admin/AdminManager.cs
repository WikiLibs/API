using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;
using WikiLibs.Shared.Modules.Admin;
using WikiLibs.Shared.Service;

namespace WikiLibs.Admin
{
    [Module(Interface = typeof(IAdminManager))]
    public class AdminManager : IAdminManager
    {
        private readonly APIKeyManager _apiKeyManager;
        private readonly GroupManager _groupManager;
        private readonly Data.Context _context;
        private readonly ILogger<AdminManager> _logger;

        public AdminManager(Data.Context ctx, ILogger<AdminManager> logger)
        {
            _groupManager = new GroupManager(ctx);
            _apiKeyManager = new APIKeyManager(ctx);
            _context = ctx;
            _logger = logger;
        }

        public IAPIKeyManager APIKeyManager => _apiKeyManager;

        public IGroupManager GroupManager => _groupManager;

        [ModuleInitializer(Debug = true, Release = true)]
        public static void Initialize(AdminManager manager)
        {
#if DEBUG
            if (!manager._context.APIKeys.Any(x => x.Description == "[WIKILIBS_SUPER_DEV_API_KEY]"))
            {
                manager.APIKeyManager.PostAsync(new Data.Models.APIKey()
                {
                    Flags = AuthorizeApiKey.Authentication | AuthorizeApiKey.Registration | AuthorizeApiKey.Standard,
                    Description = "[WIKILIBS_SUPER_DEV_API_KEY]",
                    ExpirationDate = DateTime.MaxValue,
                    UseNum = -1
                }).Wait();
                manager._logger.LogInformation("A development API Key has been generated");
            }
            var key = manager._context.APIKeys.FirstOrDefault(x => x.Description == "[WIKILIBS_SUPER_DEV_API_KEY]");
            manager._logger.LogWarning("Development API Key : " + key.Id);
            manager._logger.LogWarning("This development key is intended for development purposes, it has ALL flags enabled and will never expire");
#endif
            if (!manager._context.Groups.Any(x => x.Name == "Default"))
            {
                var g = new Data.Models.Group()
                {
                    Name = "Default"
                };
                manager.GroupManager.PostAsync(g).Wait();
            }
            if (!manager._context.Groups.Any(x => x.Name == "Admin"))
            {
                var g = new Data.Models.Group()
                {
                    Name = "Admin"
                };
                g.Permissions.Add(new Data.Models.Permission()
                {
                    Group = g,
                    Perm = "*"
                });
                manager.GroupManager.PostAsync(g).Wait();
            }
        }
    }
}
