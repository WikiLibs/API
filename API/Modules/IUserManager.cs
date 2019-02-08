using System;
using System.Collections.Generic;
using System.Text;

namespace API.Modules
{
    public interface IUserManager : IModule
    {
        Entities.User GetUser(string uuid);
        Entities.User GetUser(string email, string pass);

        /**
         * Attemts to update/create a user
         * Returns 0 when succeeded, error code otherwise
         */
        int SetUser(Entities.User usr);
        
        /**
         * Attemts to delete a user
         * Returns 0 when succeeded, error code otherwise
         */
        int DeleteUser(Entities.User usr);
    }
}
