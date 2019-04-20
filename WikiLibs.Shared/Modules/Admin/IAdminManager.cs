using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Shared.Modules.Admin
{
    public interface IAdminManager : IModule
    {
        //void PostMessage(string msg);
        //ICollection<Data.Models.AdminMsg> GetAllMessages(); Needs to be added to Database scheme first
        //void DeleteMessage(uint id);
        IAPIKeyManager APIKeyManager { get; }
        IGroupManager GroupManager { get; }
    }
}
