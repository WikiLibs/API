using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Shared.Helpers;
using WikiLibs.Shared.Modules.Symbols;
using WikiLibs.Symbols;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class SymbolTests : DBTest<ISymbolManager>
    {
        public override ISymbolManager CreateManager()
        {
            return (new SymbolManager(Context, new Config()
            {
                MaxSymsPerPage = 15
            }));
        }

        private async Task<IActionResult> PostTestSymbol()
        {
            if (!Context.SymbolLangs.Any(e => e.Name == "C"))
                Context.SymbolLangs.Add(new Data.Models.Symbols.Lang()
                {
                    Name = "C",
                });
            if (!Context.SymbolTypes.Any(e => e.Name == "function"))
                Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
                {
                    Name = "function"
                });
            await Context.SaveChangesAsync();
            var controller = new Symbols.SymbolController(Manager, User);
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
                Symbols = new string[] { }
            });
            return (res);
        }

        private async Task<IActionResult> PostTestSymbol_Complex_1(bool broken = false)
        {
            Context.SymbolLangs.Add(new Data.Models.Symbols.Lang()
            {
                Name = "C",
            });
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "function"
            });
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "typedef"
            });
            await Context.SaveChangesAsync();
            var controller = new Symbols.SymbolController(Manager, User);
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
                                Ref = broken ? "NON_EXISTANT" : "C/TestLib/fint"
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

        private async Task<IActionResult> PostTestSymbol_Complex(bool broken = false)
        {
            await PostTestSymbol();

            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "class"
            });
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "typedef"
            });
            await Context.SaveChangesAsync();
            var controller = new Symbols.SymbolController(Manager, User);
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
                    broken ? "NON_EXISTANT" : "C/TestLib/TestFunc"
                }
            });
            return (res);
        }

        [Test]
        public async Task Post()
        {
            var res = await PostTestSymbol();

            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual("C", Context.Symbols.First().Lang.Name);
            Assert.AreEqual("C/TestLib", Context.Symbols.First().Lib.Name);
        }

        [Test]
        public async Task Post_Complex()
        {
            var res = await PostTestSymbol_Complex();

            Assert.AreEqual(3, Context.Symbols.Count());
            Assert.AreEqual(3, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual(1, Context.SymbolRefs.Count());
        }

        [Test]
        public async Task Post_Complex_1()
        {
            var res = await PostTestSymbol_Complex_1();

            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual(1, Context.PrototypeParamSymbolRefs.Count());
            Assert.IsNotNull(Context.PrototypeParamSymbolRefs.First().PrototypeParam);
            Assert.IsNotNull(Context.PrototypeParamSymbolRefs.First().PrototypeParamId);
        }

        [Test]
        public async Task Optimize_1()
        {
            var controller = new Symbols.SymbolController(Manager, User);
            await PostTestSymbol_Complex();

            Assert.AreEqual(3, Context.Symbols.Count());
            Assert.AreEqual(3, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
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
            await PostTestSymbol_Complex_1();

            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
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
        public async Task Optimize_3()
        {
            var controller = new Symbols.SymbolController(Manager, User);
            await PostTestSymbol_Complex_1(true);

            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual(1, Context.PrototypeParamSymbolRefs.Count());
            Assert.IsNull(Context.PrototypeParamSymbolRefs.First().RefId);
            Assert.IsNull(Context.PrototypeParamSymbolRefs.First().Ref);
            Assert.IsNotNull(Context.PrototypeParamSymbolRefs.First().RefPath);
            await controller.OptimizeAsync();
            Assert.IsNull(Context.PrototypeParamSymbolRefs.First().RefId);
            Assert.IsNull(Context.PrototypeParamSymbolRefs.First().Ref);
            Assert.IsNotNull(Context.PrototypeParamSymbolRefs.First().RefPath);
        }

        [Test]
        public async Task Optimize_4()
        {
            var controller = new Symbols.SymbolController(Manager, User);
            await PostTestSymbol_Complex(true);

            Assert.AreEqual(3, Context.Symbols.Count());
            Assert.AreEqual(3, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual(1, Context.SymbolRefs.Count());
            Assert.IsNull(Context.SymbolRefs.First().RefId);
            Assert.IsNull(Context.SymbolRefs.First().Ref);
            Assert.IsNotNull(Context.SymbolRefs.First().RefPath);
            await controller.OptimizeAsync();
            Assert.IsNull(Context.SymbolRefs.First().RefId);
            Assert.IsNull(Context.SymbolRefs.First().Ref);
            Assert.IsNotNull(Context.SymbolRefs.First().RefPath);
        }

        [Test]
        public async Task CheckInit_Optimize()
        {
            await PostTestSymbol_Complex();

            Assert.AreEqual(3, Context.Symbols.Count());
            Assert.AreEqual(3, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual(1, Context.SymbolRefs.Count());
            Assert.IsNull(Context.SymbolRefs.First().RefId);
            Assert.IsNull(Context.SymbolRefs.First().Ref);
            Assert.IsNotNull(Context.SymbolRefs.First().RefPath);
            SymbolManager.InitSymbols(Manager);
            Assert.IsNotNull(Context.SymbolRefs.First().RefId);
            Assert.IsNotNull(Context.SymbolRefs.First().Ref);
            Assert.IsNotNull(Context.SymbolRefs.First().RefPath);
        }

        [Test]
        public async Task Post_Error_Dupe()
        {
            await PostTestSymbol();
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.ThrowsAsync<Shared.Exceptions.ResourceAlreadyExists>(() => PostTestSymbol());
        }

        [Test]
        public async Task Post_Error_Invalid()
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

            // Invalid lang (lang C has not been added)
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/TestFunc",
                Prototypes = new Models.Input.Symbols.SymbolCreate.Prototype[] { },
                Symbols = new string[] { },
                Type = "function"
            }));

            // Invalid type (type function has not been added)
            Context.SymbolLangs.Add(new Data.Models.Symbols.Lang()
            {
                Name = "C"
            });
            await Context.SaveChangesAsync();
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/TestFunc",
                Prototypes = new Models.Input.Symbols.SymbolCreate.Prototype[] { },
                Symbols = new string[] { },
                Type = "function"
            }));
        }

        [Test]
        public async Task PostPatchDelete_Import_Easy()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            Context.SymbolLangs.Add(new Data.Models.Symbols.Lang()
            {
                Name = "C",
            });
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "function"
            });
            await Context.SaveChangesAsync();
            var res = await controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/TestFunc",
                Type = "function",
                Import = "#include \"file.h\"",
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
                Symbols = new string[] { }
            });
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual("#include \"file.h\"", Context.Symbols.First().Import.Name);
            Assert.AreEqual(1, Context.SymbolImports.Count());
            await controller.PatchSymbol(Context.Symbols.First().Id, new Models.Input.Symbols.SymbolUpdate()
            {
                Import = "#include <test>"
            });
            Assert.AreEqual(1, Context.SymbolImports.Count());
            Assert.AreEqual("#include <test>", Context.Symbols.First().Import.Name);
            await controller.DeleteSymbol(Context.Symbols.First().Id);
            Assert.AreEqual(0, Context.SymbolImports.Count());
        }

        [Test]
        public async Task PostPatchDelete_Import_Complex()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            Context.SymbolLangs.Add(new Data.Models.Symbols.Lang()
            {
                Name = "C",
            });
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "function"
            });
            await Context.SaveChangesAsync();
            var res = await controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/TestFunc",
                Type = "function",
                Import = "#include \"file.h\"",
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
                Symbols = new string[] { }
            });
            await controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/TestFunc1",
                Type = "function",
                Import = "#include \"file.h\"",
                Prototypes = new Models.Input.Symbols.SymbolCreate.Prototype[]
                {
                    new Models.Input.Symbols.SymbolCreate.Prototype()
                    {
                        Description = "This is a test function",
                        Proto = "void TestFunc1()",
                        Parameters = new Models.Input.Symbols.SymbolCreate.Prototype.Parameter[]
                        {
                        }
                    }
                },
                Symbols = new string[] { }
            });
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual("#include \"file.h\"", Context.Symbols.First().Import.Name);
            Assert.AreEqual(1, Context.SymbolImports.Count());
            await controller.PatchSymbol(Context.Symbols.First().Id, new Models.Input.Symbols.SymbolUpdate()
            {
                Import = "#include <test>"
            });
            Assert.AreEqual(2, Context.SymbolImports.Count());
            Assert.AreEqual("#include <test>", Context.Symbols.First().Import.Name);
            await controller.DeleteSymbol(Context.Symbols.First().Id);
            Assert.AreEqual(1, Context.SymbolImports.Count());
            await controller.DeleteSymbol(Context.Symbols.First().Id);
            Assert.AreEqual(0, Context.SymbolImports.Count());
        }

        [Test]
        public async Task PostPatchDelete_Import_Complex_1()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            Context.SymbolLangs.Add(new Data.Models.Symbols.Lang()
            {
                Name = "C",
            });
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "function"
            });
            await Context.SaveChangesAsync();
            var res = await controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/TestFunc",
                Type = "function",
                Import = "#include \"file.h\"",
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
                Symbols = new string[] { }
            });
            await controller.PostSymbol(new Models.Input.Symbols.SymbolCreate()
            {
                Path = "C/TestLib/TestFunc1",
                Type = "function",
                Import = "#include \"file.h\"",
                Prototypes = new Models.Input.Symbols.SymbolCreate.Prototype[]
                {
                    new Models.Input.Symbols.SymbolCreate.Prototype()
                    {
                        Description = "This is a test function",
                        Proto = "void TestFunc1()",
                        Parameters = new Models.Input.Symbols.SymbolCreate.Prototype.Parameter[]
                        {
                        }
                    }
                },
                Symbols = new string[] { }
            });
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual("#include \"file.h\"", Context.Symbols.First().Import.Name);
            Assert.AreEqual(1, Context.SymbolImports.Count());
            await controller.PatchSymbol(Context.Symbols.First().Id, new Models.Input.Symbols.SymbolUpdate()
            {
                Import = "#include <test>"
            });
            Assert.AreEqual(2, Context.SymbolImports.Count());
            Assert.AreEqual("#include <test>", Context.Symbols.First().Import.Name);
            await controller.PatchSymbol(Context.Symbols.First().Id, new Models.Input.Symbols.SymbolUpdate()
            {
                Import = "#include \"file.h\""
            });
            Assert.AreEqual(1, Context.SymbolImports.Count());
            await controller.DeleteSymbol(Context.Symbols.First().Id);
            Assert.AreEqual(1, Context.SymbolImports.Count());
            await controller.DeleteSymbol(Context.Symbols.First().Id);
            Assert.AreEqual(0, Context.SymbolImports.Count());
        }

        [Test]
        public async Task Patch_Easy()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol();
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "test"
            });
            await Context.SaveChangesAsync();
            await controller.PatchSymbol(1, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "test"
            });
            Assert.AreEqual(1, Context.Symbols.Count());
            Assert.AreEqual(1, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type.Name);

            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => controller.PatchSymbol(1, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "test123456789"
            }));

        }

        [Test]
        public async Task Patch_Complex()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol();
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "test"
            });
            await Context.SaveChangesAsync();
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
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type.Name);
            Assert.AreEqual("void TestFunc()", Context.Symbols.First().Prototypes.First().Data);
            Assert.AreEqual("This is a test function", Context.Symbols.First().Prototypes.First().Description);
        }

        [Test]
        public async Task Patch_Complex_1()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol();
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "test"
            });
            await Context.SaveChangesAsync();
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
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type.Name);
            Assert.AreEqual("void TestFunc(int a, const int b, int c, int d, void *bad)", Context.Symbols.First().Prototypes.First().Data);
            Assert.AreEqual("This is a test function 123456789", Context.Symbols.First().Prototypes.First().Description);
        }

        [Test]
        public async Task Patch_Complex_2()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol();
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "test"
            });
            await Context.SaveChangesAsync();
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
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual("test", Context.Symbols.First().Type.Name);
            Assert.AreEqual("void TestFunc(int a, const int b, int c, int d, void *bad)", Context.Symbols.First().Prototypes.First().Data);
            Assert.AreEqual("This is a test function 123456789", Context.Symbols.First().Prototypes.First().Description);
            Assert.AreEqual("raw pointer", Context.Symbols.First().Prototypes.First().Parameters.Last().Description);
            Assert.AreEqual("void *bad", Context.Symbols.First().Prototypes.First().Parameters.Last().Data);
        }

        [Test]
        public async Task Patch_Complex_3()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol_Complex();
            Assert.AreEqual(3, Context.Symbols.Count());
            Assert.AreEqual(3, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual(1, Context.SymbolRefs.Count());
            Context.SymbolTypes.Add(new Data.Models.Symbols.Type()
            {
                Name = "test"
            });
            await Context.SaveChangesAsync();
            await controller.PatchSymbol(3, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "test",
                Symbols = new string[] { }
            });
            Assert.AreEqual(3, Context.Symbols.Count());
            Assert.AreEqual(3, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual(0, Context.SymbolRefs.Count());
            await controller.PatchSymbol(3, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "test",
                Symbols = new string[]
                {
                    "C/TestLib/TestFunc"
                }
            });
            Assert.AreEqual(3, Context.Symbols.Count());
            Assert.AreEqual(3, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual(1, Context.SymbolRefs.Count());
        }

        [Test]
        public async Task Patch_Complex_4()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol_Complex_1();
            Assert.AreEqual(2, Context.Symbols.Count());
            Assert.AreEqual(2, Context.Prototypes.Count());
            Assert.AreEqual(5, Context.PrototypeParams.Count());
            Assert.AreEqual(1, Context.SymbolLibs.Count());
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
            Assert.AreEqual(1, Context.SymbolLibs.Count());
            Assert.AreEqual(0, Context.PrototypeParamSymbolRefs.Count());
            Assert.IsNull(Context.PrototypeParams.First().SymbolRef);
        }

        [Test]
        public async Task Get_Path()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol();
            var res = await controller.GetSymbol(new Symbols.SymbolController.SymbolQuery() { Path = "C/TestLib/TestFunc" }) as JsonResult;
            var obj = res.Value as Models.Output.Symbols.Symbol;
            Assert.AreEqual("C", obj.Lang.Name);
            Assert.AreEqual("C/TestLib/TestFunc", obj.Path);
            Assert.AreEqual("function", obj.Type.Name);
            Assert.AreEqual(1, obj.Prototypes.Length);
            Assert.AreEqual(5, obj.Prototypes[0].Parameters.Length);
            Assert.AreEqual(1, obj.Views);
            res = await controller.GetSymbol(new Symbols.SymbolController.SymbolQuery() { Id = 1 }) as JsonResult;
            obj = res.Value as Models.Output.Symbols.Symbol;
            Assert.AreEqual(2, obj.Views);
        }

        [Test]
        public async Task Get_Path_Complex()
        {
            var controller = new Symbols.SymbolController(Manager, User);
            await PostTestSymbol_Complex();
            await controller.OptimizeAsync();
            var res = await controller.GetSymbol(new Symbols.SymbolController.SymbolQuery() { Path = "C/TestLib/Test" }) as JsonResult;
            var obj = res.Value as Models.Output.Symbols.Symbol;

            Assert.Greater(obj.Symbols.Count, 0);
        }

        [Test]
        public async Task Get_Id()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            await PostTestSymbol();
            var res = await controller.GetSymbol(new Symbols.SymbolController.SymbolQuery() { Id = 1 }) as JsonResult;
            var obj = res.Value as Models.Output.Symbols.Symbol;
            Assert.AreEqual("C", obj.Lang.Name);
            Assert.AreEqual("C/TestLib/TestFunc", obj.Path);
            Assert.AreEqual("function", obj.Type.Name);
            Assert.AreEqual(1, obj.Prototypes.Length);
            Assert.AreEqual(5, obj.Prototypes[0].Parameters.Length);
            Assert.AreEqual(1, obj.Views);
            res = await controller.GetSymbol(new Symbols.SymbolController.SymbolQuery() { Id = 1 }) as JsonResult;
            obj = res.Value as Models.Output.Symbols.Symbol;
            Assert.AreEqual(2, obj.Views);
        }

        [Test]
        public void Get_Error_NonExistant()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => controller.GetSymbol(new Symbols.SymbolController.SymbolQuery() { Path = "crap" }));
        }

        [Test]
        public void Get_Error_Invalid()
        {
            var controller = new Symbols.SymbolController(Manager, User);

            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => controller.GetSymbol(new Symbols.SymbolController.SymbolQuery() { }));
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => controller.GetSymbol(null));
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

            await PostTestSymbol();
            Assert.AreEqual(1, Context.Symbols.Count());
            await controller.DeleteSymbol(1);
            Assert.AreEqual(0, Context.Symbols.Count());
            Assert.AreEqual(0, Context.Prototypes.Count());
            Assert.AreEqual(0, Context.PrototypeParams.Count());
            Assert.AreEqual(0, Context.SymbolLibs.Count());
        }

        [Test]
        public async Task SearchString_Error_Invalid()
        {
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol();
            Assert.Throws<Shared.Exceptions.InvalidResource>(() => controller.SearchSymbols("C/TestLib", new PageOptions()
            {
                Page = 0
            }));
        }

        [Test]
        public async Task ListSymbols()
        {
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol();
            var res = controller.GetSymbolsForLib(1, new PageOptions()
            {
                Page = 1
            }) as JsonResult;
            var obj = res.Value as PageResult<SymbolListItem>;
            Assert.AreEqual(1, obj.Data.Count());
            Assert.AreEqual("C/TestLib/TestFunc", obj.Data.First().Path);
            Assert.AreEqual(1, obj.Data.First().Id);
            Assert.AreEqual("function", obj.Data.First().Type);
            Assert.IsFalse(obj.HasMorePages);
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(10, obj.Count);
        }

        [Test]
        public async Task ListSymbols_Complex()
        {
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol_Complex();
            await Manager.OptimizeAsync();
            var res = controller.GetSymbolsForLib(1, new PageOptions()
            {
                Page = 1
            }) as JsonResult;
            var obj = res.Value as PageResult<SymbolListItem>;
            Assert.AreEqual(3, obj.Data.Count());
        }

        [Test]
        public async Task SearchString()
        {
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol();
            var res = controller.SearchSymbols("C/TestLib/", new PageOptions()
            {
                Page = 1
            }) as JsonResult;
            var obj = res.Value as PageResult<SymbolListItem>;
            Assert.AreEqual(1, obj.Data.Count());
            Assert.AreEqual("C/TestLib/TestFunc", obj.Data.First().Path);
            Assert.AreEqual(1, obj.Data.First().Id);
            Assert.AreEqual("function", obj.Data.First().Type);
            Assert.IsFalse(obj.HasMorePages);
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(10, obj.Count);
        }

        [Test]
        public async Task SearchString_2()
        {
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol();
            var res = controller.SearchSymbols("C/TestLib/", new PageOptions()
            {
                Page = 1,
                Count = 0
            }) as JsonResult;
            var obj = res.Value as PageResult<SymbolListItem>;
            Assert.AreEqual(1, obj.Data.Count());
            Assert.AreEqual("C/TestLib/TestFunc", obj.Data.First().Path);
            Assert.AreEqual(1, obj.Data.First().Id);
            Assert.AreEqual("function", obj.Data.First().Type);
            Assert.IsFalse(obj.HasMorePages);
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(10, obj.Count);
        }

        [Test]
        public async Task SearchString_3()
        {
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol();
            var res = controller.SearchSymbols("C/TestLib/", new PageOptions()
            {
                Page = 1,
                Count = 1
            }) as JsonResult;
            var obj = res.Value as PageResult<SymbolListItem>;
            Assert.AreEqual(1, obj.Data.Count());
            Assert.AreEqual("C/TestLib/TestFunc", obj.Data.First().Path);
            Assert.AreEqual(1, obj.Data.First().Id);
            Assert.AreEqual("function", obj.Data.First().Type);
            Assert.IsFalse(obj.HasMorePages);
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(1, obj.Count);
        }

        [Test]
        public async Task SearchString_4()
        {
            var controller = new Symbols.SearchController(Manager);

            await PostTestSymbol();
            var res = controller.SearchSymbols("C/TestLib/", new PageOptions() { }) as JsonResult;
            var obj = res.Value as PageResult<SymbolListItem>;
            Assert.AreEqual(1, obj.Data.Count());
            Assert.AreEqual("C/TestLib/TestFunc", obj.Data.First().Path);
            Assert.AreEqual(1, obj.Data.First().Id);
            Assert.AreEqual("function", obj.Data.First().Type);
            Assert.IsFalse(obj.HasMorePages);
            Assert.AreEqual(1, obj.Page);
            Assert.AreEqual(10, obj.Count);
        }

        [Test]
        public void Permissions()
        {
            User.SetPermissions(new string[] { });
            var controller = new Symbols.SymbolController(Manager, User);

            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => PostTestSymbol());
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchSymbol(1, new Models.Input.Symbols.SymbolUpdate()
            {
                Type = "enum"
            }));
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteSymbol(1));
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.OptimizeAsync());
        }

        [Test]
        public async Task SearchLangs()
        {
            var controller = new Symbols.LangController(Manager, User);

            await PostTestSymbol();
            var res = controller.AllLangs() as JsonResult;
            var obj = res.Value as IEnumerable<Models.Output.Symbols.Lang>;
            Assert.AreEqual(1, obj.Count());
            Assert.AreEqual("C", obj.ElementAt(0).Name);
        }

        [Test]
        public async Task SearchLibs()
        {
            var controller = new Symbols.LangController(Manager, User);

            await PostTestSymbol();
            var res = controller.AllLibs(1, new PageOptions() { Page = 1 }) as JsonResult;
            var obj = res.Value as PageResult<LibListItem>;
            Assert.AreEqual(1, obj.Data.Count());
            Assert.AreEqual("C/TestLib", obj.Data.ElementAt(0).Name);
        }
    }
}
