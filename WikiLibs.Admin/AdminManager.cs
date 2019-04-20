using System;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Admin;

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
    }
}
