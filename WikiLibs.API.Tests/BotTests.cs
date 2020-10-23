using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
        public override IUserManager CreateManager()
        {
            return (new UserManager(Context));
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

            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchAsync(User.UserId, null));

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
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchAsync(bot.Id, null));
        }

        [Test]
        public async Task DeleteBot()
        {
            var controller = new BotController(Manager, new AdminManager(Context, LogUtils.FakeLogger<AdminManager>()), User);
            var bot = await PostTestBot(controller);

            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteAsync(User.UserId));
            Assert.AreEqual(2, Context.Users.Count());
            await controller.DeleteAsync(bot.Id);
            Assert.AreEqual(1, Context.Users.Count());
            bot = await PostTestBot(controller);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteAsync(bot.Id));
        }

        [Test]
        public async Task GetBots()
        {
            var controller = new BotController(Manager, new AdminManager(Context, LogUtils.FakeLogger<AdminManager>()), User);
            await PostTestBot(controller);
            var res = controller.Get() as JsonResult;
            var elems = res.Value as IEnumerable<Models.Output.UserGlobal>;

            Assert.AreEqual(1, elems.Count());
            User.SetPermissions(new string[] { });
            Assert.Throws<Shared.Exceptions.InsuficientPermission>(() => controller.Get());
        }

        [Test]
        public async Task Controller_Icon()
        {
            var controller = new BotController(Manager, new AdminManager(Context, LogUtils.FakeLogger<AdminManager>()), User);
            var bot = await PostTestBot(controller);

            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchAsync(User.UserId, null));

            var str = "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";
            var stream = new MemoryStream(Convert.FromBase64String(str));
            stream.Position = 0;

            await controller.PutIcon(bot.Id, new FileController.FormFile()
            {
                File = new FormFile(stream, 0, stream.Length, null, "Image.jpeg")
            });

            Assert.IsNotNull(Context.Users.ToList().Last().Icon);
            Assert.IsNotEmpty(Context.Users.ToList().Last().Icon);

            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PutIcon(bot.Id, null));
        }
    }
}