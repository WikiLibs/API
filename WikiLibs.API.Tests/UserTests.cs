using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Shared.Modules;
using WikiLibs.Users;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class UserTests : DBTest<IUserManager>
    {
        public override IUserManager CreateManager()
        {
            return (new UserManager(Context));
        }

        [Test]
        public async Task Controller_GET_ME()
        {
            User.User.Private = true;
            await Context.SaveChangesAsync();
            var controller = new UserController(User, Manager);
            var res = await controller.GetMe() as JsonResult;
            var usr = res.Value as Models.Output.User;
            Assert.AreEqual("Dev", usr.FirstName);
            Assert.AreEqual("DEV", usr.LastName);
            Assert.AreEqual("dev@localhost", usr.Email);
            Assert.AreEqual("dev", usr.Pseudo);
        }

        [Test]
        public async Task Controller_GET_GLOBAL()
        {
            User.User.Private = true;
            await Context.SaveChangesAsync();
            var controller = new UserController(User, Manager);
            var res = await controller.GetUser(User.UserId) as JsonResult;
            var usr = res.Value as Models.Output.User;
            Assert.IsNull(usr.FirstName);
            Assert.IsNull(usr.LastName);
            Assert.IsNull(usr.Email);
            Assert.AreEqual("Development user", usr.ProfileMsg);
            Assert.AreEqual("dev", usr.Pseudo);
        }

        [Test]
        public async Task Controller_PATCH_ME()
        {
            User.User.Private = true;
            await Context.SaveChangesAsync();
            var controller = new UserController(User, Manager);
            await controller.PatchMe(new Models.Input.Users.UserUpdate()
            {
                CurPassword = "dev",
                Pseudo = "dev2"
            });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchMe(new Models.Input.Users.UserUpdate()
            {
                CurPassword = "dev33",
                Pseudo = "dev3"
            }));
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchMe(new Models.Input.Users.UserUpdate()
            {
                CurPassword = "dev",
                Pseudo = "dev4"
            }));
            var res = await controller.GetMe() as JsonResult;
            var usr = res.Value as Models.Output.User;
            Assert.AreEqual("Dev", usr.FirstName);
            Assert.AreEqual("DEV", usr.LastName);
            Assert.AreEqual("dev@localhost", usr.Email);
            Assert.AreEqual("dev2", usr.Pseudo);
        }

        [Test]
        public async Task Controller_PATCH_GLOBAL()
        {
            User.User.Private = true;
            await Context.SaveChangesAsync();
            var controller = new UserController(User, Manager);
            await controller.PatchUser(User.UserId, new Models.Input.Users.UserUpdateGlobal()
            {
                Pseudo = "dev2"
            });
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchUser(User.UserId, new Models.Input.Users.UserUpdateGlobal()
            {
                Pseudo = "dev4"
            }));
            var res = await controller.GetMe() as JsonResult;
            var usr = res.Value as Models.Output.User;
            Assert.AreEqual("Dev", usr.FirstName);
            Assert.AreEqual("DEV", usr.LastName);
            Assert.AreEqual("dev@localhost", usr.Email);
            Assert.AreEqual("dev2", usr.Pseudo);
        }

        [Test]
        public async Task Controller_DELETE_ME_OK()
        {
            User.User.Private = true;
            await Context.SaveChangesAsync();
            var controller = new UserController(User, Manager);
            await controller.DeleteUser();
            Assert.AreEqual(0, Context.Users.Count());
        }

        [Test]
        public async Task Controller_DELETE_ME_PERM()
        {
            User.User.Private = true;
            await Context.SaveChangesAsync();
            var controller = new UserController(User, Manager);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteUser());
        }

        [Test]
        public async Task Controller_DELETE_GLOBAL_OK()
        {
            User.User.Private = true;
            await Context.SaveChangesAsync();
            var controller = new UserController(User, Manager);
            await controller.DeleteUser(User.UserId);
            Assert.AreEqual(0, Context.Users.Count());
        }

        [Test]
        public async Task Controller_DELETE_GLOBAL_PERM()
        {
            User.User.Private = true;
            await Context.SaveChangesAsync();
            var controller = new UserController(User, Manager);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteUser(User.UserId));
        }
    }
}
