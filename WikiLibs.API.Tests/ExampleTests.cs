using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Examples;
using WikiLibs.Models.Input.Symbols;
using WikiLibs.Models.Input.Examples;
using WikiLibs.Shared.Modules.Examples;
using WikiLibs.Symbols;
using WikiLibs.API.Examples;
using Microsoft.AspNetCore.Mvc;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    class ExampleTests : DBTest<IExampleManager>
    {
        public override IExampleManager CreateManager()
        {
            return (new ExampleManager(Context));
        }

        private async Task<Symbol> PostTestSymbol(Symbols.SymbolController controller)
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
            await controller.PostSymbol(new SymbolCreate()
            {
                Path = "C/TestLib/TestFunc",
                Type = "function",
                Prototypes = new SymbolCreate.Prototype[]
                {
                    new SymbolCreate.Prototype()
                    {
                        Description = "This is a test function",
                        Proto = "void TestFunc(int a, const int b, int c, int d, void *bad)",
                        Parameters = new SymbolCreate.Prototype.Parameter[]
                        {
                            new SymbolCreate.Prototype.Parameter()
                            {
                                Description = "a",
                                Proto = "int a"
                            },
                            new SymbolCreate.Prototype.Parameter()
                            {
                                Description = "b",
                                Proto = "const int b"
                            },
                            new SymbolCreate.Prototype.Parameter()
                            {
                                Description = "c",
                                Proto = "int c"
                            },
                            new SymbolCreate.Prototype.Parameter()
                            {
                                Description = "d",
                                Proto = "int d"
                            },
                            new SymbolCreate.Prototype.Parameter()
                            {
                                Description = "bad raw pointer",
                                Proto = "void *bad"
                            }
                        }
                    }
                },
                Symbols = new string[] { }
            });

            return (Context.Symbols.First());
        }

        public async Task PostTestExample(Symbol sym)
        {
            var ex = new ExampleCreate()
            {
                Description = "This is a test example",
                Code = new ExampleCreate.CodeLine[]
                {
                    new ExampleCreate.CodeLine()
                    {
                        Data = "void main()",
                        Comment = ""
                    },
                    new ExampleCreate.CodeLine()
                    {
                        Data = "{",
                        Comment = ""
                    },
                    new ExampleCreate.CodeLine()
                    {
                        Data = "}",
                        Comment = ""
                    }
                }
            }.CreateModel();

            ex.Symbol = sym;
            ex.User = User.User;
            await Manager.PostAsync(ex);
        }

        [Test]
        public async Task Post()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            }), User));
            await PostTestExample(sym);

            Assert.AreEqual(1, Context.Examples.Count());
            Assert.AreEqual(3, Context.ExampleCodeLines.Count());
            Assert.AreEqual(3, Context.Examples.First().Code.Count());
            Assert.AreEqual("This is a test example", Context.Examples.First().Description);
        }

        [Test]
        public async Task Post_Multiple()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            }), User));
            await PostTestExample(sym);
            await PostTestExample(sym);

            Assert.AreEqual(2, Context.Examples.Count());
            Assert.AreEqual(6, Context.ExampleCodeLines.Count());
            Assert.AreEqual(3, Context.Examples.First().Code.Count());
            Assert.AreEqual(3, Context.Examples.ToList().Last().Code.Count());
            Assert.AreEqual("This is a test example", Context.Examples.First().Description);
        }

        [Test]
        public async Task Post_Error_Invalid()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            }), User));

            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => PostTestExample(null));
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(async () =>
            {
                var ex = new ExampleCreate()
                {
                    Description = "This is a test example",
                    Code = new ExampleCreate.CodeLine[]
                    {
                        new ExampleCreate.CodeLine()
                        {
                            Data = "void main()",
                            Comment = ""
                        },
                        new ExampleCreate.CodeLine()
                        {
                            Data = "{",
                            Comment = ""
                        },
                        new ExampleCreate.CodeLine()
                        {
                            Data = "}",
                            Comment = ""
                        }
                    }
                }.CreateModel();

                ex.Symbol = sym;
                await Manager.PostAsync(ex);
            });
        }

        [Test]
        public async Task Patch()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            }), User));
            await PostTestExample(sym);

            await Manager.PatchAsync(Context.Examples.First().Id, new ExampleUpdate()
            {
                Description = "test"
            }.CreatePatch(Context.Examples.First()));
            Assert.AreEqual("test", Context.Examples.First().Description);
        }

        [Test]
        public async Task Patch_Complex()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            }), User));
            await PostTestExample(sym);

            await Manager.PatchAsync(Context.Examples.First().Id, new ExampleUpdate()
            {
                Description = "test",
                Code = new ExampleUpdate.CodeLine[]
                {
                    new ExampleUpdate.CodeLine()
                    {
                        Comment = "test",
                        Data = "int main() {}"
                    }
                }
            }.CreatePatch(Context.Examples.First()));
            Assert.AreEqual(1, Context.ExampleCodeLines.Count());
            Assert.AreEqual(1, Context.Examples.First().Code.Count());
            Assert.AreEqual("test", Context.Examples.First().Description);
            Assert.AreEqual("test", Context.ExampleCodeLines.First().Comment);
            Assert.AreEqual("int main() {}", Context.ExampleCodeLines.First().Data);
        }

        [Test]
        public async Task Delete()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            }), User));
            await PostTestExample(sym);

            Assert.AreEqual(1, Context.Examples.Count());
            Assert.AreEqual(3, Context.ExampleCodeLines.Count());
            Assert.AreEqual("This is a test example", Context.Examples.First().Description);

            await Manager.DeleteAsync(Context.Examples.First());

            Assert.AreEqual(0, Context.Examples.Count());
            Assert.AreEqual(0, Context.ExampleCodeLines.Count());
        }

        [Test]
        public async Task Get()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            }), User));
            await PostTestExample(sym);

            var res = Manager.GetForSymbol(sym.Id);
            Assert.AreEqual(1, res.Count());
            Assert.AreEqual("This is a test example", res.First().Description);
        }

        [Test]
        public async Task Vote_1()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            }), User));
            await PostTestExample(sym);

            Assert.IsFalse(Manager.HasAlreadyVoted(User, 1));
            await Manager.UpVote(User, 1);
            Assert.IsTrue(Manager.HasAlreadyVoted(User, 1));
            Assert.AreEqual(1, Context.Examples.First().VoteCount);
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => Manager.UpVote(User, 1));
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => Manager.DownVote(User, 1));
        }

        [Test]
        public async Task Vote_2()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            }), User));
            await PostTestExample(sym);

            Assert.IsFalse(Manager.HasAlreadyVoted(User, 1));
            await Manager.DownVote(User, 1);
            Assert.IsTrue(Manager.HasAlreadyVoted(User, 1));
            Assert.AreEqual(-1, Context.Examples.First().VoteCount);
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => Manager.UpVote(User, 1));
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => Manager.DownVote(User, 1));
        }

        [Test]
        public async Task Controller_Post()
        {
            var smanager = new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            });
            var controller = new ExampleController(User, new ExampleModule(Context, new WikiLibs.Examples.Config()
            {
                MaxExampleRequestsPerPage = 100
            }), smanager);
            var sym = await PostTestSymbol(new Symbols.SymbolController(smanager, User));
            var res = await controller.PostAsync(new ExampleCreate()
            {
                SymbolId = 1,
                Description = "This is a test example",
                Code = new ExampleCreate.CodeLine[]
                {
                    new ExampleCreate.CodeLine()
                    {
                        Data = "void main()",
                        Comment = ""
                    },
                    new ExampleCreate.CodeLine()
                    {
                        Data = "{",
                        Comment = ""
                    },
                    new ExampleCreate.CodeLine()
                    {
                        Data = "}",
                        Comment = ""
                    }
                }
            }) as JsonResult;
            var obj = res.Value as Models.Output.Examples.Example;

            Assert.AreEqual(1, obj.SymbolId);
            Assert.AreEqual(3, obj.Code.Length);
            Assert.AreEqual("This is a test example", obj.Description);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PostAsync(new ExampleCreate()));
        }

        [Test]
        public async Task Controller_Patch()
        {
            var smanager = new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            });
            var controller = new ExampleController(User, new ExampleModule(Context, new WikiLibs.Examples.Config()
            {
                MaxExampleRequestsPerPage = 100
            }), smanager);
            var sym = await PostTestSymbol(new Symbols.SymbolController(smanager, User));
            await PostTestExample(sym);

            var res = await controller.PatchAsync(1, new ExampleUpdate()
            {
                Description = "test"
            }) as JsonResult;
            var obj = res.Value as Models.Output.Examples.Example;

            Assert.AreEqual(1, obj.SymbolId);
            Assert.AreEqual("test", obj.Description);
            Assert.AreEqual(3, obj.Code.Length);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchAsync(1, new ExampleUpdate()));
        }

        [Test]
        public async Task Controller_Delete()
        {
            var smanager = new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            });
            var controller = new ExampleController(User, new ExampleModule(Context, new WikiLibs.Examples.Config()
            {
                MaxExampleRequestsPerPage = 100
            }), smanager);
            var sym = await PostTestSymbol(new Symbols.SymbolController(smanager, User));
            await PostTestExample(sym);

            await controller.DeleteAsync(1);

            Assert.AreEqual(0, Context.Examples.Count());
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteAsync(1));
        }

        [Test]
        public async Task Controller_GetByQuery()
        {
            var smanager = new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            });
            var controller = new ExampleController(User, new ExampleModule(Context, new WikiLibs.Examples.Config()
            {
                MaxExampleRequestsPerPage = 100
            }), smanager);
            var sym = await PostTestSymbol(new Symbols.SymbolController(smanager, User));
            await PostTestExample(sym);

            var res = controller.Get(new ExampleController.ExampleQuery() { SymbolId = 1 }) as JsonResult;
            var obj = res.Value as IEnumerable<Models.Output.Examples.Example>;
            Assert.AreEqual(1, obj.Count());
            Assert.AreEqual("This is a test example", obj.First().Description);
            res = controller.Get(new ExampleController.ExampleQuery() { Token = "test" }) as JsonResult;
            obj = res.Value as IEnumerable<Models.Output.Examples.Example>;
            Assert.AreEqual(1, obj.Count());
            Assert.AreEqual("This is a test example", obj.First().Description);
        }

        [Test]
        public async Task Controller_GetById()
        {
            var smanager = new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            });
            var controller = new ExampleController(User, new ExampleModule(Context, new WikiLibs.Examples.Config()
            {
                MaxExampleRequestsPerPage = 100
            }), smanager);
            var sym = await PostTestSymbol(new Symbols.SymbolController(smanager, User));
            await PostTestExample(sym);

            var res = await controller.GetAsync(1) as JsonResult;
            var obj = res.Value as Models.Output.Examples.Example;
            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("This is a test example", obj.Description);
        }

        [Test]
        public async Task Controller_Get_Error_Invalid()
        {
            var smanager = new SymbolManager(Context, new WikiLibs.Symbols.Config()
            {
                MaxSymsPerPage = 15
            });
            var controller = new ExampleController(User, new ExampleModule(Context, new WikiLibs.Examples.Config()
            {
                MaxExampleRequestsPerPage = 100
            }), smanager);
            var sym = await PostTestSymbol(new Symbols.SymbolController(smanager, User));
            await PostTestExample(sym);

            Assert.Throws<Shared.Exceptions.InvalidResource>(() => controller.Get(new ExampleController.ExampleQuery()));
            Assert.Throws<Shared.Exceptions.InvalidResource>(() => controller.Get(null));
        }
    }
}
