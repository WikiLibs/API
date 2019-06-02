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
            var res = await controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/TestFunc",
                Type = "function",
                Prototypes = new Models.Input.Symbols.SymbolCreate.Prototype[]
                {
                    new Models.Input.Symbols.SymbolCreate.Prototype()
                    {
                        Description = "This is a test function",
                        Proto = "void TestFunc(int a, const int b, int c, int d, void *bad)",
                        Parameters = new Models.Input.Symbols.SymbolCreate.Prototype.Parameter[]
                        {
                            new Models.Input.Symbols.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "a",
                                Proto = "int a"
                            },
                            new Models.Input.Symbols.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "b",
                                Proto = "const int b"
                            },
                            new Models.Input.Symbols.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "c",
                                Proto = "int c"
                            },
                            new Models.Input.Symbols.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "d",
                                Proto = "int d"
                            },
                            new Models.Input.Symbols.SymbolCreate.Prototype.Parameter()
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

        private async Task<IActionResult> PostTestSymbol_Complex_1(Symbols.SymbolController controller)
        {
            await controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/fint",
                Type = "typedef",
                Prototypes = new Models.Input.Symbols.SymbolCreate.Prototype[]
                {
                    new Models.Input.Symbols.SymbolCreate.Prototype()
                    {
                        Description = "An int typedef",
                        Proto = "typedef int fint",
                        Parameters = new Models.Input.Symbols.SymbolCreate.Prototype.Parameter[] { }
                    }
                },
                Symbols = new string[] { }
            });

            var res = await controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/TestFunc",
                Type = "function",
                Prototypes = new Models.Input.Symbols.SymbolCreate.Prototype[]
                {
                    new Models.Input.Symbols.SymbolCreate.Prototype()
                    {
                        Description = "This is a test function",
                        Proto = "void TestFunc(fint a, const int b, int c, int d, void *bad)",
                        Parameters = new Models.Input.Symbols.SymbolCreate.Prototype.Parameter[]
                        {
                            new Models.Input.Symbols.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "a",
                                Proto = "fint a",
                                Path = "C/TestLib/fint"
                            },
                            new Models.Input.Symbols.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "b",
                                Proto = "const int b"
                            },
                            new Models.Input.Symbols.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "c",
                                Proto = "int c"
                            },
                            new Models.Input.Symbols.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "d",
                                Proto = "int d"
                            },
                            new Models.Input.Symbols.SymbolCreate.Prototype.Parameter()
                            {
                                Description = "bad raw pointer",
                                Proto = "void *bad"
                            }
                        }
                    }
                },
                Symbols = new string[] { }
            });
            return (res);
        }

        private async Task<IActionResult> PostTestSymbol_Complex(Symbols.SymbolController controller)
        {
            await PostTestSymbol(controller);

            var res = await controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/Test",
                Type = "class",
                Prototypes = new Models.Input.Symbols.SymbolCreate.Prototype[]
                {
                    new Models.Input.Symbols.SymbolCreate.Prototype()
                    {
                        Description = "Yeah I know a class in C...",
                        Proto = "class Test",
                        Parameters = new Models.Input.Symbols.SymbolCreate.Prototype.Parameter[] { }
                    }
                },
                Symbols = new string[]
                {
                    "C/TestLib/TestFunc"
                }
            });
            return (res);
        }

        [Test]
        public async Task Post()
        {
            var controller = new Symbols.SymbolController(Manager, User);
            var res = await PostTestSymbol(controller);

            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual("C", Context.Symbols.First().Lang);
            Assert.AreEqual("TestLib", Context.Symbols.First().Lib);
        }

        [Test]
        public async Task Post_Complex()
        {
            var controller = new Symbols.SymbolController(Manager, User);
            var res = await PostTestSymbol_Complex(controller);

            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual(1, Context.SymbolRefs.Count());
        }

        [Test]
        public async Task Post_Complex_1()
        {
            var controller = new Symbols.SymbolController(Manager, User);
            var res = await PostTestSymbol_Complex_1(controller);

            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual(1, Context.PrototypeParamSymbolRefs.Count());
        }

        [Test]
        public async Task Optimize_1()
        {
            var controller = new Symbols.SymbolController(Manager, User);
            var res = await PostTestSymbol_Complex(controller);

            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual(1, Context.SymbolRefs.Count());
            Assert.IsNull(Context.SymbolRefs.First().RefId);
            Assert.IsNull(Context.SymbolRefs.First().Ref);
            Assert.IsNotNull(Context.SymbolRefs.First().RefPath);
            await controller.OptimizeAsync();
            Assert.IsNotNull(Context.SymbolRefs.First().RefId);
            Assert.IsNotNull(Context.SymbolRefs.First().Ref);
            Assert.IsNotNull(Context.SymbolRefs.First().RefPath);
        }

        [Test]
        public async Task Optimize_2()
        {
            var controller = new Symbols.SymbolController(Manager, User);
            var res = await PostTestSymbol_Complex_1(controller);

            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual(1, Context.PrototypeParamSymbolRefs.Count());
            Assert.IsNull(Context.PrototypeParamSymbolRefs.First().RefId);
            Assert.IsNull(Context.PrototypeParamSymbolRefs.First().Ref);
            Assert.IsNotNull(Context.PrototypeParamSymbolRefs.First().RefPath);
            await controller.OptimizeAsync();
            Assert.IsNotNull(Context.PrototypeParamSymbolRefs.First().RefId);
            Assert.IsNotNull(Context.PrototypeParamSymbolRefs.First().Ref);
            Assert.IsNotNull(Context.PrototypeParamSymbolRefs.First().RefPath);
        }

        [Test]
        public async Task Post_Error_Dupe()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol(controller);
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.ThrowsAsync<Shared.Exceptions.ResourceAlreadyExists>(() => PostTestSymbol(controller));
        }

        [Test]
        public void Post_Error_Invalid()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "",
                Prototypes = new Models.Input.Symbols.SymbolCreate.Prototype[] { },
                Symbols = new string[] { },
                Type = ""
            }));
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => Manager.PostAsync(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/NonValidFunc",
                Prototypes = new Models.Input.Symbols.SymbolCreate.Prototype[] { },
                Symbols = new string[] { },
                Type = ""
            }.CreateModel()));
        }

        [Test]
        public async Task Patch_Easy()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol(controller);
            await controller.PatchSymbol(1, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "test"
            });
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type);
        }

        [Test]
        public async Task Patch_Complex()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol(controller);
            await controller.PatchSymbol(1, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "test",
                Prototypes = new Models.Input.Symbols.SymbolUpdate.Prototype[]
                {
                    new Models.Input.Symbols.SymbolUpdate.Prototype()
                    {
                        Description = "This is a test function",
                        Proto = "void TestFunc()",
                        Parameters = new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter[]
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

        [Test]
        public async Task Patch_Complex_1()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol(controller);
            await controller.PatchSymbol(1, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "test",
                Prototypes = new Models.Input.Symbols.SymbolUpdate.Prototype[]
                {
                    new Models.Input.Symbols.SymbolUpdate.Prototype()
                    {
                        Description = "This is a test function 123456789"
                    }
                }
            });
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type);
            Assert.AreEqual("void TestFunc(int a, const int b, int c, int d, void *bad)", Context.Symbols.First().Prototypes.First().Data);
            Assert.AreEqual("This is a test function 123456789", Context.Symbols.First().Prototypes.First().Description);
        }

        [Test]
        public async Task Patch_Complex_2()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol(controller);
            await controller.PatchSymbol(1, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "test",
                Prototypes = new Models.Input.Symbols.SymbolUpdate.Prototype[]
                {
                    new Models.Input.Symbols.SymbolUpdate.Prototype()
                    {
                        Description = "This is a test function 123456789",
                        Parameters = new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter[]
                        {
                            new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter()
                            {
                            },
                            new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter()
                            {
                            },
                            new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter()
                            {
                            },
                            new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter()
                            {
                            },
                            new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter()
                            {
                                Description = "raw pointer"
                            }
                        }
                    }
                }
            });
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type);
            Assert.AreEqual("void TestFunc(int a, const int b, int c, int d, void *bad)", Context.Symbols.First().Prototypes.First().Data);
            Assert.AreEqual("This is a test function 123456789", Context.Symbols.First().Prototypes.First().Description);
            Assert.AreEqual("raw pointer", Context.Symbols.First().Prototypes.First().Parameters.Last().Description);
            Assert.AreEqual("void *bad", Context.Symbols.First().Prototypes.First().Parameters.Last().Data);
        }

        [Test]
        public async Task Patch_Complex_3()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol_Complex(controller);
            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual(1, Context.SymbolRefs.Count());
            await controller.PatchSymbol(2, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "test",
                Symbols = new string[] { }
            });
            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual(0, Context.SymbolRefs.Count());
            await controller.PatchSymbol(2, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "test",
                Symbols = new string[]
                {
                    "C/TestLib/TestFunc"
                }
            });
            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual(1, Context.SymbolRefs.Count());
        }

        [Test]
        public async Task Patch_Complex_4()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol_Complex_1(controller);
            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual(1, Context.PrototypeParamSymbolRefs.Count());
            Assert.IsNotNull(Context.PrototypeParams.First().SymbolRef);
            await controller.PatchSymbol(2, new Models.Input.Symbols.SymbolUpdate()
            {
                Prototypes = new Models.Input.Symbols.SymbolUpdate.Prototype[]
                {
                    new Models.Input.Symbols.SymbolUpdate.Prototype()
                    {
                        Description = "This is a test function",
                        Proto = "void TestFunc(int a, const int b, int c, int d, void *bad)",
                        Parameters = new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter[]
                        {
                            new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter()
                            {
                                Description = "a",
                                Proto = "int a"
                            },
                            new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter()
                            {
                                Description = "b",
                                Proto = "const int b"
                            },
                            new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter()
                            {
                                Description = "c",
                                Proto = "int c"
                            },
                            new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter()
                            {
                                Description = "d",
                                Proto = "int d"
                            },
                            new Models.Input.Symbols.SymbolUpdate.Prototype.Parameter()
                            {
                                Description = "bad raw pointer",
                                Proto = "void *bad"
                            }
                        }
                    }
                }
            });
            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(2, Context.InfoTable.Count());
            Assert.AreEqual(0, Context.PrototypeParamSymbolRefs.Count());
            Assert.IsNull(Context.PrototypeParams.First().SymbolRef);
        }

        [Test]
        public async Task Get_Path()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol(controller);
            var res = await controller.GetSymbol(new Symbols.SymbolController.SymbolQuery() { Path = "C/TestLib/TestFunc" }) as JsonResult;
            var obj = res.Value as Models.Output.Symbol;
            Assert.AreEqual("C", obj.Lang);
            Assert.AreEqual("C/TestLib/TestFunc", obj.Path);
            Assert.AreEqual("function", obj.Type);
            Assert.AreEqual(1, obj.Prototypes.Length);
            Assert.AreEqual(5, obj.Prototypes[0].Parameters.Length);
        }

        [Test]
        public async Task Get_Id()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol(controller);
            var res = await controller.GetSymbol(new Symbols.SymbolController.SymbolQuery() { Id = 1 }) as JsonResult;
            var obj = res.Value as Models.Output.Symbol;
            Assert.AreEqual("C", obj.Lang);
            Assert.AreEqual("C/TestLib/TestFunc", obj.Path);
            Assert.AreEqual("function", obj.Type);
            Assert.AreEqual(1, obj.Prototypes.Length);
            Assert.AreEqual(5, obj.Prototypes[0].Parameters.Length);
        }

        [Test]
        public void Get_Error_NonExistant()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => controller.GetSymbol(new Symbols.SymbolController.SymbolQuery() { Path = "crap" }));
        }

        [Test]
        public void Delete_Error_NonExistant()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => controller.DeleteSymbol(-1));
        }

        [Test]
        public async Task Delete()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol(controller);
            Assert.AreEqual(1, Context.Symbols.Count());
            await controller.DeleteSymbol(1);
            Assert.AreEqual(0, Context.Symbols.Count());
            Assert.AreEqual(0, Context.Prototypes.Count());
            Assert.AreEqual(0, Context.PrototypeParams.Count());
            Assert.AreEqual(0, Context.InfoTable.Count());
        }

        [Test]
        public async Task SearchLangs()
        {
            var symController = new Symbols.SymbolController(Manager, User);
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol(symController);
            var res = controller.AllLangs() as JsonResult;
            var obj = res.Value as string[];
            Assert.AreEqual(1, obj.Length);
            Assert.AreEqual("C", obj[0]);
        }

        [Test]
        public async Task SearchLibs()
        {
            var symController = new Symbols.SymbolController(Manager, User);
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol(symController);
            var res = controller.AllLibs("C") as JsonResult;
            var obj = res.Value as string[];
            Assert.AreEqual(1, obj.Length);
            Assert.AreEqual("C/TestLib/", obj[0]);
        }

        [Test]
        public async Task SearchString_Error_Invalid()
        {
            var symController = new Symbols.SymbolController(Manager, User);
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol(symController);
            Assert.Throws<Shared.Exceptions.InvalidResource>(() => controller.SearchSymbols("C/TestLib", new PageOptions()
            {
                Page = 0
            }));
        }

        [Test]
        public async Task SearchString()
        {
            var symController = new Symbols.SymbolController(Manager, User);
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

        [Test]
        public async Task Search_2()
        {
            var symController = new Symbols.SymbolController(Manager, User);
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

        [Test]
        public async Task Search_3()
        {
            var symController = new Symbols.SymbolController(Manager, User);
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

        [Test]
        public async Task Search_4()
        {
            var symController = new Symbols.SymbolController(Manager, User);
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

        [Test]
        public void Permissions()
        {
            User.SetPermissions(new string[] { });
            var controller = new Symbols.SymbolController(Manager, User);

            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => PostTestSymbol(controller));
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchSymbol(1, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "enum"
            }));
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteSymbol(1));
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.OptimizeAsync());
        }
    }
}
