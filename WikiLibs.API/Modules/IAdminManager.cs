using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.API.Modules
{
    public interface IAdminManager : IModule
    {
        //void PostMessage(string msg);
        //ICollection<Data.Models.AdminMsg> GetAllMessages(); Needs to be added to Database scheme first
        //void DeleteMessage(uint id);
        void DeleteGroup(Data.Models.Group group);
        void CreateGroup(Data.Models.Group group);
        void PatchGroup(Data.Models.Group group);
        Data.Models.Group GetGroup(string name);
        Data.Models.Group GetGroup(long id);
        ICollection<Data.Models.Group> GetGroups();
        Data.Models.APIKey CreateAPIKey(string desc);
        ICollection<Data.Models.APIKey> GetAllAPIKeys();
        void RevokeAPIKey(string key);
        bool APIKeyExists(string key);
    }
}
