using System;
using System.Collections.Generic;
using System.Text;

namespace API.Modules
{
    public interface IAdminManager : IModule
    {
        void PostMessage(string msg);
        Entities.AdminMsg[] GetAllMessages();
        void DeleteMessage(uint id);
        void DeleteGroup(string name);
        void SetGroup(Entities.Group group);
        Entities.Group GetGroup(string name);
        string[] GetGroups();
        Entities.APIKey CreateAPIKey(string desc);
        Entities.APIKey[] GetAllAPIKeys();
        void RevokeAPIKey(string key);
        bool APIKeyExists(string key);
    }
}
