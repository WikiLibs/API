using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.API.Symbols;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Models.Input.Symbols;
using WikiLibs.Shared.Modules.Symbols;
using WikiLibs.Symbols;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class TypeTests : DBTest<ISymbolManager>
    {
        public override ISymbolManager CreateManager()
        {
            return (new SymbolManager(Context, new Config()
            {
                MaxSymsPerPage = 15
            }));
        }

        [Test]
        public async Task Post()
        {
            await Manager.TypeManager.PostAsync(new TypeCreate()
            {
                Name = "test"
            }.CreateModel());

            Assert.AreEqual(1, Context.SymbolTypes.Count());
            Assert.AreEqual("test", Context.SymbolTypes.First().Name);
        }

        [Test]
        public async Task Patch()
        {
            await Manager.TypeManager.PostAsync(new TypeCreate()
            {
                Name = "test"
            }.CreateModel());

            Assert.AreEqual(1, Context.SymbolTypes.Count());
            Assert.AreEqual("test", Context.SymbolTypes.First().Name);

            await Manager.TypeManager.PatchAsync(1, new TypeUpdate()
            {
                Name = "test123"
            }.CreatePatch(Context.SymbolTypes.First()));

            Assert.AreEqual(1, Context.SymbolTypes.Count());
            Assert.AreEqual("test123", Context.SymbolTypes.First().Name);
        }

        [Test]
        public async Task Delete()
        {
            await Manager.TypeManager.PostAsync(new TypeCreate()
            {
                Name = "test"
            }.CreateModel());

            Assert.AreEqual(1, Context.SymbolTypes.Count());
            Assert.AreEqual("test", Context.SymbolTypes.First().Name);

            await Manager.TypeManager.DeleteAsync(1);

            Assert.AreEqual(0, Context.SymbolTypes.Count());
        }

        [Test]
        public async Task Controller_POST()
        {
            var controller = new TypeController(Manager, User);

            var mdl = await controller.PostAsync(new TypeCreate()
            {
                Name = "test"
            }) as JsonResult;
            var obj = mdl.Value as Models.Output.Symbols.Type;

            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("test", obj.Name);

            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PostAsync(new TypeCreate()));
        }

        [Test]
        public async Task Controller_GET()
        {
            var controller = new TypeController(Manager, User);

            var mdl = await controller.PostAsync(new TypeCreate()
            {
                Name = "test"
            }) as JsonResult;
            var obj = mdl.Value as Models.Output.Symbols.Type;

            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("test", obj.Name);

            var res = controller.AllTypes() as JsonResult;
            var types = res.Value as IEnumerable<Models.Output.Symbols.Type>;
            Assert.AreEqual(1, types.Count());
            Assert.AreEqual("test", types.First().Name);
        }

        [Test]
        public async Task Controller_PATCH()
        {
            var controller = new TypeController(Manager, User);

            var mdl = await controller.PostAsync(new TypeCreate()
            {
                Name = "test"
            }) as JsonResult;
            var obj = mdl.Value as Models.Output.Symbols.Type;

            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("test", obj.Name);

            mdl = await controller.PatchAsync(1, new TypeUpdate()
            {
                Name = "function"
            }) as JsonResult;
            obj = mdl.Value as Models.Output.Symbols.Type;

            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("function", obj.Name);

            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchAsync(1, null));
        }

        [Test]
        public async Task Controller_DELETE()
        {
            var controller = new TypeController(Manager, User);

            var mdl = await controller.PostAsync(new TypeCreate()
            {
                Name = "test"
            }) as JsonResult;
            var obj = mdl.Value as Models.Output.Symbols.Type;

            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("test", obj.Name);
            Assert.AreEqual(1, Context.SymbolTypes.Count());

            await controller.DeleteAsync(1);

            Assert.AreEqual(0, Context.SymbolTypes.Count());

            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteAsync(1));
        }
    }
}
