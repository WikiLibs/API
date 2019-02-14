using System;
using System.Linq;
using API.Entities;
using Microsoft.Extensions.Configuration;

namespace UserManager
{
    [API.Module(typeof(API.Modules.IUserManager))]
    public class UserManager : API.Modules.IUserManager
    {
        public const string DEFAULT_USER_GROUP = "Default";
        private WikiLibs.DB.Context _db;

        public UserManager(WikiLibs.DB.Context db)
        {
            _db = db;
        }

        public int DeleteUser(User usr)
        {
            var u = _db.Users.Find(new object[] { usr.UUID });

            if (u == null)
                return (404);
            _db.Users.Remove(u);
            return (200);
        }

        public User GetUser(string uuid)
        {
            var usr = _db.Users.Find(new object[] { uuid });
            User u = new User();

            if (usr == null)
                return (null);
            u.EMail = usr.EMail;
            u.FirstName = usr.FirstName;
            u.LastName = usr.LastName;
            u.Pass = usr.Pass;
            u.Points = usr.Points;
            u.Icon = usr.Icon;
            u.ProfileMsg = usr.ProfileMsg;
            u.Pseudo = usr.Pseudo;
            u.Group = usr.Group;
            u.ShowEmail = usr.ShowEmail;
            u.UUID = usr.UUID;
            u.Date = usr.Date;
            var perms = _db.Permissions.Where(o => o.Group == u.Group);
            foreach (var p in perms)
                u.AddPermission(p.Perm);
            return (u);
        }

        public User GetUser(string email, string pass)
        {
            var users = _db.Users.Where(uu => uu.EMail == email && uu.Pass == pass);
            User u = new User();

            if (users == null || users.Count() <= 0)
                return (null);
            var usr = users.First();
            u.EMail = usr.EMail;
            u.FirstName = usr.FirstName;
            u.LastName = usr.LastName;
            u.Pass = usr.Pass;
            u.Points = usr.Points;
            u.Icon = usr.Icon;
            u.ProfileMsg = usr.ProfileMsg;
            u.Pseudo = usr.Pseudo;
            u.Group = usr.Group;
            u.ShowEmail = usr.ShowEmail;
            u.UUID = usr.UUID;
            u.Date = usr.Date;
            var perms = _db.Permissions.Where(o => o.Group == u.Group);
            foreach (var p in perms)
                u.AddPermission(p.Perm);
            return (u);
        }

        #region EEXP
        User GenRootUser()
        {
            User u = new User
            {
                EMail = "root@root.com",
                FirstName = "ANONYMOUS",
                LastName = "ANONYMOUS",
                Icon = "",
                Pass = "12Poissons2hOt4U",
                ShowEmail = false,
                Pseudo = "root",
                ProfileMsg = "Temporary user for Epitech Experiance"
            };

            return (u);
        }

        private void ApplyUserGroup(User u)
        {
            User root = GetUser(u.EMail, u.Pass);

            root.Group = "Root";
            SetUser(root);
        }
        #endregion

        public void LoadConfig(IConfiguration cfg)
        {
            User u = GenRootUser();
            if (GetUser(u.EMail, u.Pass) == null)
            {
                SetUser(u);
                ApplyUserGroup(u);
            }
        }

        private int AddUser(User usr)
        {
            var u = new WikiLibs.DB.User();

            var usrs = _db.Users.Where(uu => uu.EMail == usr.EMail || uu.Pseudo == usr.Pseudo);
            if (usrs.Count() > 0)
                return (409); //User already exists
            usr.Group = DEFAULT_USER_GROUP; //Force the group to the default to avoid hack of Root group injection
            u.EMail = usr.EMail;
            u.FirstName = usr.FirstName;
            u.LastName = usr.LastName;
            u.Pass = usr.Pass;
            u.Points = 0;
            u.Icon = usr.Icon;
            u.ProfileMsg = usr.ProfileMsg;
            u.Pseudo = usr.Pseudo;
            u.Group = usr.Group;
            u.ShowEmail = usr.ShowEmail;
            u.UUID = System.Guid.NewGuid().ToString();
            u.Date = DateTime.UtcNow;
            _db.Users.Add(u);
            _db.SaveChanges();
            return (200);
        }

        public int SetUser(User usr)
        {
            var u = _db.Users.Find(new object[] { usr.UUID });

            if (usr.UUID == "" || u == null)
                return (AddUser(usr));
            u.EMail = usr.EMail;
            u.FirstName = usr.FirstName;
            u.LastName = usr.LastName;
            u.Pass = usr.Pass;
            u.Points = usr.Points;
            u.Icon = usr.Icon;
            u.ProfileMsg = usr.ProfileMsg;
            u.Pseudo = usr.Pseudo;
            u.Group = usr.Group;
            u.ShowEmail = usr.ShowEmail;
            u.UUID = usr.UUID;
            u.Date = usr.Date;
            _db.SaveChanges();
            return (200);
        }
    }
}
