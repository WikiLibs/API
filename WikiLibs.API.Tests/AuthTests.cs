using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Admin;
using WikiLibs.API.Auth;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Auth;
using WikiLibs.Shared.Modules.Auth;
using WikiLibs.Users;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class AuthTests : DBTest<AuthManager>
    {
        public override void Setup()
        {
            base.Setup();
            Manager = new AuthManager(new AdminManager(Context, null), new UserManager(Context), Smtp, new Config()
            {
                DefaultGroupName = "Default",
                Internal = new Config.CInternal()
                {
                    TokenAudiance = "localhost",
                    TokenIssuer = "localhost",
                    TokenLifeMinutes = 5,
                    TokenSecret = "TEST_DEVELOPMENT_SECRET"
                }
            });
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
        }

        [Test, Order(3)]
        public async Task Register()
        {
            var controller = new InternalController(Manager);

            //Creation
            await PostTestUser(controller);
            Assert.AreEqual(1, Smtp.SentEmailCount);
            Assert.AreEqual("WikiLibs API Server", Smtp.LastSendEmail.Subject);
            Assert.AreEqual("UserRegistration", Smtp.LastSendEmail.Template);
            Assert.AreEqual("test@test.com", Smtp.LastSendEmail.Recipients.First().Email);
            Assert.AreEqual(Context.Users.Last().FirstName + " " + Context.Users.Last().LastName, Smtp.LastSendEmail.Recipients.First().Name);
            var data = Smtp.LastSendEmail.Model as Shared.Modules.Smtp.Models.UserRegistration;
            Assert.AreEqual(Context.Users.Last().FirstName + " " + Context.Users.Last().LastName, data.UserName);
            Assert.AreEqual(Context.Users.Last().Confirmation, data.ConfirmCode);
            Assert.AreEqual(2, Context.Users.Count());
            Assert.IsNotNull(Context.Users.Last().Confirmation);

            //Check we can't login
            Assert.ThrowsAsync<InvalidCredentials>(() => controller.Login(new Models.Input.Auth.Login()
            {
                Email = "test@test.com",
                Password = "thisIsATest"
            }));

            //Confirmation
            await controller.Confirm(data.ConfirmCode);
            Assert.AreEqual(2, Context.Users.Count());
            Assert.IsNull(Context.Users.Last().Confirmation);

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
            await controller.Reset("dev@localhost");
            Assert.AreEqual(1, Smtp.SentEmailCount);
            Assert.AreEqual("WikiLibs API Server", Smtp.LastSendEmail.Subject);
            Assert.AreEqual("UserReset", Smtp.LastSendEmail.Template);
            Assert.AreEqual("dev@localhost", Smtp.LastSendEmail.Recipients.First().Email);
            var data = Smtp.LastSendEmail.Model as Shared.Modules.Smtp.Models.UserReset;
            Assert.AreEqual(Context.Users.Last().Pass, data.NewPassword);

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

            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => controller.Reset("doesnotexist@doesnotexist.com"));
        }

        [Test]
        public void Confirm_Error_Null()
        {
            var controller = new InternalController(Manager);

            Assert.ThrowsAsync<InvalidCredentials>(() => controller.Confirm(null));
        }

        [Test]
        public void Confirm_Error_Invalid()
        {
            var controller = new InternalController(Manager);

            Assert.ThrowsAsync<InvalidCredentials>(() => controller.Confirm("abcd"));
            Assert.ThrowsAsync<InvalidCredentials>(() => controller.Confirm("abcd.efgh"));
        }

        [Test]
        public void Confirm_Error_NonExistant()
        {
            var controller = new InternalController(Manager);

            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => controller.Confirm("abcd.efgh.123456789"));
            Assert.ThrowsAsync<InvalidCredentials>(() => controller.Confirm("abcd." + new Guid().ToString() + ".123456789"));
        }

        [Test]
        public void Refresh()
        {
            Assert.IsNotNull(Manager.Refresh(new Guid().ToString()));
        }
    }
}
