using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Shared.Helpers;
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
            Assert.ThrowsAsync<Shared.Exceptions.ResourceAlreadyExists>(() => PostTestSymbol(controller));
        }

        [Test, Order(3)]
        public void PostSymbol_Error_Invalid()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => controller.PostSymbol(new Models.Input.SymbolCreate()
            {
                Lang = "",
                Path = "",
                Prototypes = new Models.Input.SymbolCreate.Prototype[] { },
                Symbols = new string[] { },
                Type = ""
            }));
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => Manager.PostAsync(new Models.Input.SymbolCreate()
            {
                Lang = "",
                Path = "",
                Prototypes = new Models.Input.SymbolCreate.Prototype[] { },
                Symbols = new string[] { },
                Type = ""
            }.CreateModel()));
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

        [Test, Order(7)]
        public async Task GetSymbol()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            await PostTestSymbol(controller);
            var res = controller.GetSymbol("C/TestLib/TestFunc") as JsonResult;
            var obj = res.Value as Models.Output.Symbol;
            Assert.AreEqual("C", obj.Lang);
            Assert.AreEqual("C/TestLib/TestFunc", obj.Path);
            Assert.AreEqual("function", obj.Type);
            Assert.AreEqual(1, obj.Prototypes.Length);
            Assert.AreEqual(5, obj.Prototypes[0].Parameters.Length);
        }

        [Test, Order(8)]
        public void GetSymbol_Error_NonExistant()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            Assert.Throws<Shared.Exceptions.ResourceNotFound>(() => 
            controller.GetSymbol("crap"));
        }

        [Test, Order(9)]
        public void DeleteSymbol_Error_NonExistant()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => controller.DeleteSymbol("crap"));
        }

        [Test, Order(10)]
        public async Task DeleteSymbol()
        {
            var controller = new Symbols.SymbolController(Manager, FakeUser);

            await PostTestSymbol(controller);
            Assert.AreEqual(1, Context.Symbols.Count());
            await controller.DeleteSymbol("C/TestLib/TestFunc");
            Assert.AreEqual(0, Context.Symbols.Count());
            Assert.AreEqual(0, Context.Prototypes.Count());
            Assert.AreEqual(0, Context.PrototypeParams.Count());
            Assert.AreEqual(0, Context.InfoTable.Count());
        }

        [Test, Order(11)]
        public async Task SearchLangs()
        {
            var symController = new Symbols.SymbolController(Manager, FakeUser);
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol(symController);
            var res = controller.AllLangs() as JsonResult;
            var obj = res.Value as string[];
            Assert.AreEqual(1, obj.Length);
            Assert.AreEqual("C", obj[0]);
        }

        [Test, Order(12)]
        public async Task SearchLibs()
        {
            var symController = new Symbols.SymbolController(Manager, FakeUser);
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol(symController);
            var res = controller.AllLibs("C") as JsonResult;
            var obj = res.Value as string[];
            Assert.AreEqual(1, obj.Length);
            Assert.AreEqual("C/TestLib/", obj[0]);
        }

        [Test, Order(13)]
        public async Task SearchSymbols_Error_Invalid()
        {
            var symController = new Symbols.SymbolController(Manager, FakeUser);
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol(symController);
            Assert.Throws<Shared.Exceptions.InvalidResource>(() => controller.SearchSymbols("C/TestLib", new PageOptions()
            {
                Page = 0
            }));
        }

        [Test, Order(14)]
        public async Task SearchSymbols()
        {
            var symController = new Symbols.SymbolController(Manager, FakeUser);
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol(symController);
            var res = controller.SearchSymbols("C/TestLib/", new PageOptions()
            {
                Page = 1
            }) as JsonResult;
            var obj = res.Value as PageResult<string>;
            Assert.AreEqual(1, obj.Data.Count());
            Assert.AreEqual("C/TestLib/TestFunc", obj.Data.First());
            Assert.IsFalse(obj.HasMorePages);
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(15, obj.Count);
        }

        [Test, Order(15)]
        public async Task SearchSymbols_2()
        {
            var symController = new Symbols.SymbolController(Manager, FakeUser);
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol(symController);
            var res = controller.SearchSymbols("C/TestLib/", new PageOptions()
            {
                Page = 1,
                Count = 0
            }) as JsonResult;
            var obj = res.Value as PageResult<string>;
            Assert.AreEqual(1, obj.Data.Count());
            Assert.AreEqual("C/TestLib/TestFunc", obj.Data.First());
            Assert.IsFalse(obj.HasMorePages);
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(15, obj.Count);
        }

        [Test, Order(16)]
        public async Task SearchSymbols_3()
        {
            var symController = new Symbols.SymbolController(Manager, FakeUser);
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol(symController);
            var res = controller.SearchSymbols("C/TestLib/", new PageOptions()
            {
                Page = 1,
                Count = 1
            }) as JsonResult;
            var obj = res.Value as PageResult<string>;
            Assert.AreEqual(1, obj.Data.Count());
            Assert.AreEqual("C/TestLib/TestFunc", obj.Data.First());
            Assert.IsFalse(obj.HasMorePages);
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(1, obj.Count);
        }

        [Test, Order(17)]
        public async Task SearchSymbols_4()
        {
            var symController = new Symbols.SymbolController(Manager, FakeUser);
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol(symController);
            var res = controller.SearchSymbols("C/TestLib/", new PageOptions() { }) as JsonResult;
            var obj = res.Value as PageResult<string>;
            Assert.AreEqual(1, obj.Data.Count());
            Assert.AreEqual("C/TestLib/TestFunc", obj.Data.First());
            Assert.IsFalse(obj.HasMorePages);
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(15, obj.Count);
        }
    }
}
