using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Data.Models.Symbols;
using WikiLibs.Examples;
using WikiLibs.Models.Input.Examples;
using WikiLibs.Models.Input.Symbols;
using WikiLibs.Shared.Modules.Examples;
using WikiLibs.Symbols;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    class ExampleRequestTests : DBTest<IExampleRequestManager>
    {
        public override void Setup()
        {
            base.Setup();
            Manager = new ExampleRequestManager(Context);
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

        [Test]
        public async Task Post()
        {
            var sym = await PostTestSymbol(new Symbols.SymbolController(new SymbolManager(Context, new Config()
            {
                MaxSymsPerPage = 15
            }), User));
            var ex = new ExampleRequestCreate()
            {
                Message = "This is a test",
                Method = Data.Models.Examples.ExampleRequestType.POST,
                Data = new ExampleCreate()
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
                }
            }.CreateModel();
            ex.Data.Symbol = sym;
            ex.Data.User = User.User;
            await Manager.PostAsync(ex);

            Assert.AreEqual(1, Context.Examples.Count());
            Assert.AreEqual(3, Context.ExampleCodeLines.Count());
            Assert.AreEqual(1, Context.ExampleRequests.Count());
            Assert.AreEqual("This is a test example", Context.Examples.First().Description);
            Assert.IsNotNull(Context.ExampleRequests.First().DataId);
            Assert.IsNotNull(Context.ExampleRequests.First().Data);
            Assert.IsNotNull(Context.Examples.First().Request);
            Assert.IsNotNull(Context.Examples.First().RequestId);
        }
    }
}
