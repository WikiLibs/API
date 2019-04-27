using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Shared.Modules;
using WikiLibs.Symbols;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class SymbolTests : DBTest<ISymbolManager>
    {
        public override void Setup()
        {
            base.Setup();
            Manager = new SymbolManager(Context, new Config()
            {
                MaxSymsPerPage = 15
            });
        }

        private async Task<IActionResult> PostTestSymbol(Symbols.SymbolController controller)
        {
            var res = await controller.PostSymbol(new Models.Input.SymbolCreate()
            {
                Lang = "C",
                Path = "C/TestLib/TestFunc",
                Type = "function",
                Prototypes = new Models.Input.SymbolCreate.Prototype[]
                {
                    new Models.Input.SymbolCreate.Prototype()
                    {
                        Description = "This is a test function",
                        Proto = "void TestFunc(int a, const int b, int c, int d, void *bad)",
                        Parameters = new Models.Input.SymbolCreate.Prototype.Parameter[]
                        {
                            new Models.Input.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "a",
                                Proto = "int a"
                            },
                            new Models.Input.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "b",
                                Proto = "const int b"
                            },
                            new Models.Input.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "c",
                                Proto = "int c"
                            },
                            new Models.Input.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "d",
                                Proto = "int d"
                            },
                            new Models.Input.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "bad raw pointer",
                                Proto = "void *bad"
                            }
                        }
                    }
                },
                Symbols = new string[] {}
            });
            return (res);
        }

        [Test, Order(1)]
        public async Task PostSymbol()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);
            var res = await PostTestSymbol(controller);

            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
        }

        [Test, Order(2)]
        public async Task PostSymbol_Error_Dupe()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            await PostTestSymbol(controller);
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.ThrowsAsync<Shared.Exceptions.ResourceAlreadyExists>(async () => await PostTestSymbol(controller));
        }

        [Test, Order(3)]
        public void PostSymbol_Error_Invalid()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(async () => await controller.PostSymbol(new Models.Input.SymbolCreate()
            {
                Lang = "",
                Path = "",
                Prototypes = new Models.Input.SymbolCreate.Prototype[] { },
                Symbols = new string[] { },
                Type = ""
            }));
        }

        [Test, Order(4)]
        public async Task PatchSymbol_Easy()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            await PostTestSymbol(controller);
            await controller.PatchSymbol("C/TestLib/TestFunc", new Models.Input.SymbolUpdate()
            {
                Type = "test"
            });
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type);
        }

        [Test, Order(5)]
        public async Task PatchSymbol_Complex()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            await PostTestSymbol(controller);
            await controller.PatchSymbol("C/TestLib/TestFunc", new Models.Input.SymbolUpdate()
            {
                Type = "test",
                Prototypes = new Models.Input.SymbolUpdate.Prototype[]
                {
                    new Models.Input.SymbolUpdate.Prototype()
                    {
                        Description = "This is a test function",
                        Proto = "void TestFunc()",
                        Parameters = new Models.Input.SymbolUpdate.Prototype.Parameter[]
                        {
                        }
                    }
                }
            });
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(0, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type);
            Assert.AreEqual("void TestFunc()", Context.Symbols.First().Prototypes.First().Data);
            Assert.AreEqual("This is a test function", Context.Symbols.First().Prototypes.First().Description);
        }

        [Test, Order(5)]
        public async Task PatchSymbol_Complex_1()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            await PostTestSymbol(controller);
            await controller.PatchSymbol("C/TestLib/TestFunc", new Models.Input.SymbolUpdate()
            {
                Type = "test",
                Prototypes = new Models.Input.SymbolUpdate.Prototype[]
                {
                    new Models.Input.SymbolUpdate.Prototype()
                    {
                        Description = "This is a test function 123456789"
                    }
                }
            });
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(0, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type);
            Assert.AreEqual("void TestFunc(int a, const int b, int c, int d, void *bad)", Context.Symbols.First().Prototypes.First().Data);
            Assert.AreEqual("This is a test function 123456789", Context.Symbols.First().Prototypes.First().Description);
        }

        [Test, Order(6)]
        public async Task PatchSymbol_Complex_2()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            await PostTestSymbol(controller);
            await controller.PatchSymbol("C/TestLib/TestFunc", new Models.Input.SymbolUpdate()
            {
                Type = "test",
                Prototypes = new Models.Input.SymbolUpdate.Prototype[]
                {
                    new Models.Input.SymbolUpdate.Prototype()
                    {
                        Description = "This is a test function 123456789",
                        Parameters = new Models.Input.SymbolUpdate.Prototype.Parameter[]
                        {
                            new Models.Input.SymbolUpdate.Prototype.Parameter()
                            {
                            },
                            new Models.Input.SymbolUpdate.Prototype.Parameter()
                            {
                            },
                            new Models.Input.SymbolUpdate.Prototype.Parameter()
                            {
                            },
                            new Models.Input.SymbolUpdate.Prototype.Parameter()
                            {
                            },
                            new Models.Input.SymbolUpdate.Prototype.Parameter()
                            {
                                Description = "raw pointer"
                            }
                        }
                    }
                }
            });
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(0, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type);
            Assert.AreEqual("void TestFunc(int a, const int b, int c, int d, void *bad)", Context.Symbols.First().Prototypes.First().Data);
            Assert.AreEqual("This is a test function 123456789", Context.Symbols.First().Prototypes.First().Description);
            Assert.AreEqual("raw pointer", Context.Symbols.First().Prototypes.First().Parameters.Last().Description);
            Assert.AreEqual("void *bad", Context.Symbols.First().Prototypes.First().Parameters.Last().Data);
        }
    }
}
