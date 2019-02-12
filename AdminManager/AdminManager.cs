using System;
using System.Collections.Generic;
using System.Linq;
using API.Entities;
using Microsoft.Extensions.Configuration;

namespace AdminManager
{
    public class AdminManager : API.Modules.IAdminManager
    {
        private WikiLibs.DB.Context _db;

        public AdminManager(WikiLibs.DB.Context db)
        {
            _db = db;
        }

        public bool APIKeyExists(string key)
        {
            return (_db.APIKeys.Find(new object[] { key }) != null);
        }

        public APIKey CreateAPIKey(string desc)
        {
            APIKey res = new APIKey();
            Guid uuid = System.Guid.NewGuid();

            res.Key = uuid.ToString();
            res.Description = desc;
            WikiLibs.DB.APIKey k = new WikiLibs.DB.APIKey();
            k.Description = desc;
            k.Key = uuid.ToString();
            _db.APIKeys.Add(k);
            _db.SaveChanges();
            return (res);
        }

        public void DeleteGroup(string name)
        {
            var g = _db.Groups.Find(new object[] { name });

            if (g != null)
            {
                _db.Permissions.RemoveRange(_db.Permissions.Where(p => p.Group == name));
                _db.Groups.Remove(g);
                _db.SaveChanges();
            }
        }

        public void DeleteMessage(uint id)
        {
            throw new NotImplementedException();
        }

        private APIKey GenAPIKey(WikiLibs.DB.APIKey k)
        {
            APIKey k1 = new APIKey();
            k1.Key = k.Key;
            k1.Description = k.Description;
            return (k1);
        }

        public APIKey[] GetAllAPIKeys()
        {
            IEnumerable<APIKey> keys = _db.APIKeys.Select(k => GenAPIKey(k));

            return (keys.ToArray());
        }

        public AdminMsg[] GetAllMessages()
        {
            throw new NotImplementedException();
        }

        public Group GetGroup(string name)
        {
            var grp = _db.Groups.Find(new object[] { name });
            if (grp == null)
                return (null);
            Group g = new Group();
            IEnumerable<string> perms = _db.Permissions.Where(p => p.Group == g.Name).Select(p => p.Perm);

            g.Name = grp.Name;
            g.Permissions = perms.ToArray();
            return (g);
        }

        public string[] GetGroups()
        {
            return (_db.Groups.Select(g => g.Name).ToArray());
        }

        public void LoadConfig(IConfiguration cfg)
        {
            throw new NotImplementedException();
        }

        public void PostMessage(string msg)
        {
            throw new NotImplementedException();
        }

        public void RevokeAPIKey(string key)
        {
            var k = _db.APIKeys.Find(new object[] { key });

            if (k == null)
                return;
            _db.APIKeys.Remove(k);
            _db.SaveChanges();
        }

        private void AddGroup(Group group)
        {
            var g = new WikiLibs.DB.Group();

            g.Name = group.Name;
            _db.Groups.Add(g);
            foreach (string s in group.Permissions)
            {
                var p = new WikiLibs.DB.Permission();
                p.Group = group.Name;
                p.Perm = s;
                _db.Permissions.Add(p);
            }
            _db.SaveChanges();
        }

        public void SetGroup(Group group)
        {
            var g = _db.Groups.Find(new object[] { group.Name });

            if (g == null)
            {
                AddGroup(group);
                return;
            }
            g.Name = group.Name;
            _db.Permissions.RemoveRange(_db.Permissions.Where(p => p.Group == group.Name));
            foreach (string s in group.Permissions)
            {
                var p = new WikiLibs.DB.Permission();
                p.Group = group.Name;
                p.Perm = s;
                _db.Permissions.Add(p);
            }
            _db.SaveChanges();
        }
    }
}