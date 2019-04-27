using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Data.Models;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Tests.Helper
{
    class FakeUser : IUser
    {
        public FakeUser(Data.Context ctx)
        {
            if (ctx.Users.FirstOrDefault() != null)
                User = ctx.Users.FirstOrDefault();
            else
            {
                User = new User()
                {
                    Icon = null,
                    Confirmation = null,
                    Group = ctx.Groups.Find(new object[] { (long)2 }),
                    FirstName = "Dev",
                    LastName = "DEV",
                    EMail = "dev@localhost",
                    Pass = "dev",
                    Id = new Guid().ToString(),
                    Points = 0,
                    Private = false,
                    ProfileMsg = "Development user",
                    Pseudo = "dev",
                    RegistrationDate = DateTime.UtcNow
                };
                ctx.Add(User);
                ctx.SaveChanges();
            }
        }

        public bool IsExternal => false;

        public User User { get; }

        public string UserId => User.Id;

        public int Points { get => User.Points; set => User.Points = value; }

        public bool HasPermission(string name)
        {
            return (true);
        }

        public Task Save()
        {
            return (Task.CompletedTask);
        }
    }
}
