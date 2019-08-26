using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
