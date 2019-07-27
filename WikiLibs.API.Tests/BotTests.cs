using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Admin;
using WikiLibs.API.Admin;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Shared.Modules;
using WikiLibs.Users;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class BotTests : DBTest<IUserManager>
    {
        public override void Setup()
        {
            base.Setup();
            Manager = new UserManager(Context);
        }

        public async Task<Models.Output.Bot> PostTestBot(BotController controller)
        {
            var res = await controller.PostAsync(new Models.Input.Admin.BotCreate()
            {
                Email = "mybot@wikilibs",
                Name = "MyBot",
                Private = false,
                ProfileMsg = "Test message",
                Pseudo = "mybot"
            }) as JsonResult;
            return (res.Value as Models.Output.Bot);
        }

        [Test]
        public async Task PostBot()
        {
            var controller = new BotController(Manager, new AdminManager(Context, LogUtils.FakeLogger<AdminManager>()), User);
            var bot = await PostTestBot(controller);

            Assert.AreEqual("MyBot", bot.FirstName);
            Assert.AreEqual("mybot@wikilibs", bot.Email);
            Assert.AreEqual("mybot", bot.Pseudo);
            Assert.IsFalse(bot.Private);
            Assert.AreEqual("Test message", bot.ProfileMsg);
            Assert.IsEmpty(bot.LastName);
            Assert.AreEqual(0, bot.Points);
            Assert.IsNotNull(bot.Secret);
            Assert.IsNotEmpty(bot.Secret);
            Assert.AreEqual(24, bot.Secret.Length);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => PostTestBot(controller));
        }

        [Test]
        public async Task PatchBot()
        {
            var controller = new BotController(Manager, new AdminManager(Context, LogUtils.FakeLogger<AdminManager>()), User);
            var bot = await PostTestBot(controller);
            var res = await controller.PatchAsync(bot.Id, new Models.Input.Admin.BotUpdate()
            {
                ProfileMsg = "Updated message"
            }) as JsonResult;
            var obj = res.Value as Models.Output.Bot;

            Assert.AreEqual("MyBot", obj.FirstName);
            Assert.AreEqual("mybot@wikilibs", obj.Email);
            Assert.AreEqual("mybot", obj.Pseudo);
            Assert.IsFalse(obj.Private);
            Assert.AreEqual("Updated message", obj.ProfileMsg);
            Assert.IsTrue(obj.LastName == null || obj.LastName.Length <= 0);
            Assert.AreEqual(0, obj.Points);
            Assert.IsNotNull(obj.Secret);
            Assert.IsNotEmpty(obj.Secret);
            Assert.AreEqual(24, obj.Secret.Length);
            Assert.AreNotEqual(obj.Secret, bot.Secret);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchAsync(null, null));
        }

        [Test]
        public async Task DeleteBot()
        {
            var controller = new BotController(Manager, new AdminManager(Context, LogUtils.FakeLogger<AdminManager>()), User);
            var bot = await PostTestBot(controller);

            Assert.AreEqual(2, Context.Users.Count());
            await controller.DeleteAsync(bot.Id);
            Assert.AreEqual(1, Context.Users.Count());
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteAsync(null));
        }

        [Test]
        public async Task GetBots()
        {
            var controller = new BotController(Manager, new AdminManager(Context, LogUtils.FakeLogger<AdminManager>()), User);
            await PostTestBot(controller);
            var res = controller.Get() as JsonResult;
            var elems = res.Value as IEnumerable<Models.Output.User>;

            Assert.AreEqual(1, elems.Count());
            User.SetPermissions(new string[] { });
            Assert.Throws<Shared.Exceptions.InsuficientPermission>(() => controller.Get());
        }
    }
}