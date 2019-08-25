using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Models.Input.Symbols;
using WikiLibs.Shared.Modules;
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
    }
}
