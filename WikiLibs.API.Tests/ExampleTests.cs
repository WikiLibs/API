using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Examples;
using WikiLibs.Models.Input;
using WikiLibs.Shared.Modules.Examples;
using WikiLibs.Symbols;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    class ExampleTests : DBTest<IExampleManager>
    {
        public override void Setup()
        {
            base.Setup();
            Manager = new ExampleManager(Context);
        }

        private async Task<Symbol> PostTestSymbol(Symbols.SymbolController controller)
        {
            await controller.PostSymbol(new SymbolCreate()
            {
                Lang = "C",
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
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new Config()
            {
                MaxSymsPerPage = 15
            }), User));
            await PostTestExample(sym);

            Assert.AreEqual(1, Context.Examples.Count());
            Assert.AreEqual(3, Context.ExampleCodeLines.Count());
            Assert.AreEqual("This is a test example", Context.Examples.First().Description);
        }

        [Test]
        public async Task Post_Multiple()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new Config()
            {
                MaxSymsPerPage = 15
            }), User));
            await PostTestExample(sym);
            await PostTestExample(sym);

            Assert.AreEqual(2, Context.Examples.Count());
            Assert.AreEqual(6, Context.ExampleCodeLines.Count());
            Assert.AreEqual("This is a test example", Context.Examples.First().Description);
        }

        [Test]
        public async Task Post_Error_Invalid()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new Config()
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
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new Config()
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
    }
}
