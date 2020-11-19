using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Admin;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Error;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class ErrorTests : DBTest<ErrorManager>
    {
        public override ErrorManager CreateManager()
        {
            return new ErrorManager(Context);
        }

        public async Task PostTestError()
        {
            await Manager.PostAsync(new Data.Models.Error
            {
                Description = "test",
                ErrorData = "test1",
                ErrorMessage = "test message"
            });
        }

        [Test]
        public async Task Post()
        {
            await PostTestError();
            Assert.AreEqual(1, Context.Errors.Count());
            Assert.AreEqual("test", Context.Errors.First().Description);
        }

        [Test]
        public async Task Controller_DELETE()
        {
            await PostTestError();
            var controller = new ErrorController(User, Manager);

            Assert.AreEqual(1, Context.Errors.Count());
            await controller.DeleteAsync(1);
            Assert.AreEqual(0, Context.Errors.Count());
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteAsync(1));
        }

        [Test]
        public async Task Controller_CLEANUP()
        {
            await PostTestError();
            var controller = new ErrorController(User, Manager);

            Assert.AreEqual(1, Context.Errors.Count());
            await controller.CleanupAsync();
            Assert.AreEqual(0, Context.Errors.Count());
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.CleanupAsync());
        }

        [Test]
        public async Task Controller_GET()
        {
            await PostTestError();
            var controller = new ErrorController(User, Manager);
            var res = controller.GetErrors() as JsonResult;
            var obj = res.Value as IEnumerable<Models.Output.Error>;

            Assert.AreEqual("test", obj.First().Description);
            Assert.AreEqual("test1", obj.First().ErrorData);
            Assert.AreEqual("test message", obj.First().ErrorMessage);
            User.SetPermissions(new string[] { });
            Assert.Throws<Shared.Exceptions.InsuficientPermission>(() => controller.GetErrors());
        }
    }
}
