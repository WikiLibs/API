using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.API.Modules
{
    public interface IUserManager : IModule
    {
        Data.Models.User GetUser(string uuid);
        Data.Models.User GetUser(string email, string pass);

        /**
         * Attemts to create a user
         */
        void CreateUser(Data.Models.User usr);

        void PatchUser(Data.Models.User usr);

        /**
         * Attemts to delete a user
         */
        void DeleteUser(Data.Models.User usr);
    }
}
