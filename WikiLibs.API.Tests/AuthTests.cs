using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WikiLibs.Admin;
using WikiLibs.API.Auth;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Auth;
using WikiLibs.Shared.Modules.Auth;
using WikiLibs.Users;
using static WikiLibs.API.Auth.InternalController;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class AuthTests : DBTest<AuthManager>
    {
        public override AuthManager CreateManager()
        {
            return (new AuthManager(new AdminManager(Context, null), new UserManager(Context), Smtp, new Config()
            {
                DefaultGroupName = "Default",
                AdminGroupName = "Admin",
                Internal = new Config.CInternal()
                {
                    TokenAudiance = "localhost",
                    TokenIssuer = "localhost",
                    TokenLifeMinutes = 5,
                    TokenSecret = "TEST_DEVELOPMENT_SECRET"
                }
            }));
        }

        [Test]
        public async Task Basics()
        {
            Assert.IsNull(Manager.GetAuthenticator("doesnotexist"));
            Assert.IsNull(Manager.GetAuthenticator("internal").GetConnectionString());
            Assert.IsNull(await Manager.GetAuthenticator("internal").Login(null));
        }

        public async Task PostTestUser(InternalController controller)
        {
            await controller.Register(new Models.Input.Users.UserCreate()
            {
                Email = "test@test.com",
                Password = "thisIsATest",
                FirstName = "Test",
                LastName = "Test",
                Private = false,
                Pseudo = "pseudo",
                ProfileMsg = "This is a testing account"
            });
        }

        [Test, Order(1)]
        public async Task Auth()
        {
            var controller = new InternalController(Manager);

            var res = await controller.Login(new Models.Input.Auth.Login()
            {
                Email = "dev@localhost",
                Password = "dev"
            }) as JsonResult;
            var token = res.Value as string;
            Assert.IsNotNull(token);
        }

        [Test, Order(2)]
        public void Auth_Error_Invalid()
        {
            var controller = new InternalController(Manager);

            Assert.ThrowsAsync<InvalidCredentials>(() => controller.Login(new Models.Input.Auth.Login()
            {
                Email = "dev@localhost123456789",
                Password = "dev123456789"
            }));

            //Check that bot login is not allowed using standard user systems
            Context.Users.ToList().Last().IsBot = true;
            Assert.ThrowsAsync<InvalidCredentials>(() => controller.Login(new Models.Input.Auth.Login()
            {
                Email = "dev@localhost",
                Password = "dev"
            }));
        }

        [Test, Order(3)]
        public async Task Register()
        {
            var controller = new InternalController(Manager);

            //Creation
            await PostTestUser(controller);
            Assert.AreEqual(1, Smtp.SentEmailCount);
            Assert.AreEqual("WikiLibs API Server", Smtp.LastSendEmail.Subject);
            Assert.AreEqual(Shared.Modules.Smtp.Models.UserRegistration.Template, Smtp.LastSendEmail.Template);
            Assert.AreEqual("test@test.com", Smtp.LastSendEmail.Recipients.First().Email);
            Assert.AreEqual(Context.Users.ToList().Last().FirstName + " " + Context.Users.ToList().Last().LastName, Smtp.LastSendEmail.Recipients.First().Name);
            var data = Smtp.LastSendEmail.Model as Shared.Modules.Smtp.Models.UserRegistration;
            Assert.AreEqual(Context.Users.ToList().Last().FirstName + " " + Context.Users.ToList().Last().LastName, data.UserName);
            Assert.AreEqual(Context.Users.ToList().Last().Confirmation, data.ConfirmCode);
            Assert.AreEqual(2, Context.Users.Count());
            Assert.IsNotNull(Context.Users.ToList().Last().Confirmation);

            //Check we can't login
            Assert.ThrowsAsync<InvalidCredentials>(() => controller.Login(new Models.Input.Auth.Login()
            {
                Email = "test@test.com",
                Password = "thisIsATest"
            }));

            //Confirmation
            await controller.Confirm(new ConfirmQuery() { Code = data.ConfirmCode });
            Assert.AreEqual(2, Context.Users.Count());
            Assert.IsNull(Context.Users.ToList().Last().Confirmation);

            //Check we can login
            var res = await controller.Login(new Models.Input.Auth.Login()
            {
                Email = "test@test.com",
                Password = "thisIsATest"
            }) as JsonResult;
            var token = res.Value as string;
            Assert.IsNotNull(token);
        }

        [Test, Order(4)]
        public async Task Register_Error_Dupe()
        {
            var controller = new InternalController(Manager);

            await PostTestUser(controller);
            Assert.ThrowsAsync<Shared.Exceptions.ResourceAlreadyExists>(() => PostTestUser(controller));
        }

        [Test, Order(5)]
        public async Task Reset()
        {
            var controller = new InternalController(Manager);

            //Reset development account password
            await controller.Reset(mew InternalController.ResetObject{ Email = "dev@localhost" });
            Assert.AreEqual(1, Smtp.SentEmailCount);
            Assert.AreEqual("WikiLibs API Server", Smtp.LastSendEmail.Subject);
            Assert.AreEqual(Shared.Modules.Smtp.Models.UserReset.Template, Smtp.LastSendEmail.Template);
            Assert.AreEqual("dev@localhost", Smtp.LastSendEmail.Recipients.First().Email);
            var data = Smtp.LastSendEmail.Model as Shared.Modules.Smtp.Models.UserReset;
            Assert.AreEqual(Context.Users.ToList().Last().Pass, data.NewPassword);

            //Check we can login
            var res = await controller.Login(new Models.Input.Auth.Login()
            {
                Email = "dev@localhost",
                Password = data.NewPassword
            }) as JsonResult;
            var token = res.Value as string;
            Assert.IsNotNull(token);
        }

        [Test, Order(6)]
        public void Reset_Error_NonExistant()
        {
            var controller = new InternalController(Manager);

            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => controller.Reset(mew InternalController.ResetObject{ Email = "doesnotexist@doesnotexist.com" }));
            Context.Users.First().IsBot = true;
            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => controller.Reset(mew InternalController.ResetObject{ Email = "dev@localhost" }));
        }

        [Test]
        public async Task Confirm_Error_Null()
        {
            var controller = new InternalController(Manager);

            var res = await controller.Confirm(new ConfirmQuery() { Code = null });
            Assert.AreEqual(res.GetType(), typeof(UnauthorizedResult));
            var redirect = await controller.Confirm(new ConfirmQuery() { Code = null, RedirectKO = "" }) as RedirectResult;
            Assert.AreEqual(redirect.Url, "?error=" + HttpUtility.UrlEncode("Code is null"));
            var redirect1 = await controller.Confirm(new ConfirmQuery() { Code = null, RedirectKO = "?test=test" }) as RedirectResult;
            Assert.AreEqual(redirect1.Url, "?test=test&error=" + HttpUtility.UrlEncode("Code is null"));
        }

        [Test]
        public async Task Confirm_Error_Invalid()
        {
            var controller = new InternalController(Manager);

            var res = await controller.Confirm(new ConfirmQuery() { Code = "abcd" });
            var res1 = await controller.Confirm(new ConfirmQuery() { Code = "abcd.efgh" });
            Assert.AreEqual(res.GetType(), typeof(UnauthorizedResult));
            Assert.AreEqual(res1.GetType(), typeof(UnauthorizedResult));
            var redirect = await controller.Confirm(new ConfirmQuery() { Code = "abcd", RedirectKO = "" }) as RedirectResult;
            var redirect1 = await controller.Confirm(new ConfirmQuery() { Code = "abcd.efgh", RedirectKO = "" }) as RedirectResult;
            Assert.AreEqual(redirect.Url, "?error=" + HttpUtility.UrlEncode("Invalid code format"));
            Assert.AreEqual(redirect1.Url, "?error=" + HttpUtility.UrlEncode("Invalid code format"));
        }

        [Test]
        public async Task Confirm_Error_NonExistant()
        {
            var controller = new InternalController(Manager);

            var res = await controller.Confirm(new ConfirmQuery() { Code = "abcd.efgh.123456789" });
            var res1 = await controller.Confirm(new ConfirmQuery() { Code = "abcd." + new Guid().ToString() + ".123456789" });
            Assert.AreEqual(res.GetType(), typeof(NotFoundResult));
            Assert.AreEqual(res1.GetType(), typeof(UnauthorizedResult));
            var redirect = await controller.Confirm(new ConfirmQuery() { Code = "abcd.efgh.123456789", RedirectKO = "" }) as RedirectResult;
            var redirect1 = await controller.Confirm(new ConfirmQuery() { Code = "abcd." + new Guid().ToString() + ".123456789", RedirectKO = "" }) as RedirectResult;
            Assert.AreEqual(redirect.Url, "?error=" + HttpUtility.UrlEncode("Resource not found: efgh"));
            Assert.AreEqual(redirect1.Url, "?error=" + HttpUtility.UrlEncode("Bad code"));
        }

        [Test]
        public void Refresh()
        {
            Assert.IsNotNull(Manager.Refresh(new Guid().ToString()));

            var controller = new BaseController(new UserManager(Context), Manager, User);
            Assert.IsNotNull(controller.Refresh());
        }

        [Test]
        public async Task BotLogin()
        {
            var controller = new BaseController(new UserManager(Context), Manager, User);

            Assert.ThrowsAsync<InvalidCredentials>(() => controller.BotLogin(new Models.Input.Auth.Bot()
            {
                AppId = Context.Users.First().Id,
                AppSecret = Context.Users.First().Pass
            }));
            Context.Users.First().IsBot = true;
            var res = await controller.BotLogin(new Models.Input.Auth.Bot()
            {
                AppId = Context.Users.First().Id,
                AppSecret = Context.Users.First().Pass
            }) as JsonResult;
            var tok = res.Value as string;
            Assert.IsNotNull(tok);
        }
    }
}
