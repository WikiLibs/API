using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.API.Symbols;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Models.Input.Symbols;
using WikiLibs.Shared.Modules.Symbols;
using WikiLibs.Symbols;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class LangTests : DBTest<ISymbolManager>
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
            await Manager.LangManager.PostAsync(new LangCreate()
            {
                Name = "test"
            }.CreateModel());

            Assert.AreEqual(1, Context.SymbolLangs.Count());
            Assert.AreEqual("test", Context.SymbolLangs.First().Name);
        }

        [Test]
        public async Task Patch()
        {
            await Manager.LangManager.PostAsync(new LangCreate()
            {
                Name = "test"
            }.CreateModel());

            Assert.AreEqual(1, Context.SymbolLangs.Count());
            Assert.AreEqual("test", Context.SymbolLangs.First().Name);

            await Manager.LangManager.PatchAsync(1, new LangUpdate()
            {
                Name = "test123"
            }.CreatePatch(Context.SymbolLangs.First()));

            Assert.AreEqual(1, Context.SymbolLangs.Count());
            Assert.AreEqual("test123", Context.SymbolLangs.First().Name);
        }

        [Test]
        public async Task Delete()
        {
            await Manager.LangManager.PostAsync(new LangCreate()
            {
                Name = "test"
            }.CreateModel());

            Assert.AreEqual(1, Context.SymbolLangs.Count());
            Assert.AreEqual("test", Context.SymbolLangs.First().Name);

            await Manager.LangManager.DeleteAsync(1);

            Assert.AreEqual(0, Context.SymbolLangs.Count());
        }

        [Test]
        public async Task Controller_POST()
        {
            var controller = new LangController(Manager, User);

            var mdl = await controller.PostLang(new LangCreate()
            {
                Name = "C"
            }) as JsonResult;
            var obj = mdl.Value as Models.Output.Symbols.Lang;

            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("C", obj.Name);

            Assert.ThrowsAsync<Shared.Exceptions.ResourceAlreadyExists>(() => controller.PostLang(new LangCreate() { Name = "C" }));

            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PostLang(new LangCreate()));
        }

        [Test]
        public async Task Controller_PATCH()
        {
            var controller = new LangController(Manager, User);

            var mdl = await controller.PostLang(new LangCreate()
            {
                Name = "C"
            }) as JsonResult;
            var obj = mdl.Value as Models.Output.Symbols.Lang;

            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("C", obj.Name);

            await controller.PostLang(new LangCreate()
            {
                Name = "C1"
            });

            mdl = await controller.PatchLang(1, new LangUpdate()
            {
                Name = "Test"
            }) as JsonResult;
            obj = mdl.Value as Models.Output.Symbols.Lang;

            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("Test", obj.Name);

            Assert.ThrowsAsync<Shared.Exceptions.ResourceAlreadyExists>(() => controller.PatchLang(1, new LangUpdate() { Name = "C1" }));

            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchLang(1, null));
        }

        [Test]
        public async Task Controller_DELETE()
        {
            var controller = new LangController(Manager, User);

            var mdl = await controller.PostLang(new LangCreate()
            {
                Name = "C"
            }) as JsonResult;
            var obj = mdl.Value as Models.Output.Symbols.Lang;

            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("C", obj.Name);
            Assert.AreEqual(1, Context.SymbolLangs.Count());

            await controller.DeleteLang(1);

            Assert.AreEqual(0, Context.SymbolLangs.Count());

            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteLang(1));
        }

        [Test]
        public async Task Controller_Icon()
        {
            var controller = new LangController(Manager, User);
            await controller.PostLang(new LangCreate()
            {
                Name = "C"
            });

            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => controller.GetIcon(1));
			
			var str = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==";
            var stream = new MemoryStream(Convert.FromBase64String(str));
            stream.Position = 0;

            await controller.PutIcon(1, new FileController.FormFile()
            {
                File = new FormFile(stream, 0, stream.Length, null, "Image.png")
            });

            Assert.IsNotNull(Context.SymbolLangs.First().Icon);
            Assert.IsNotEmpty(Context.SymbolLangs.First().Icon);

            var mdl = await controller.GetIcon(1) as JsonResult;
            var iconb64 = mdl.Value as string;

            Assert.IsNotNull(iconb64);
            Assert.IsNotEmpty(iconb64);
            Assert.IsTrue(iconb64.StartsWith("image/png,"));

            var data = Manager.LangManager.GetFile(Context.SymbolLangs.First());
            Assert.AreEqual("image/png", data.ContentType);
            Assert.Greater(data.Length, 0);
            Assert.AreEqual("Image", data.Name);

            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PutIcon(1, null));
        }
    }
}
