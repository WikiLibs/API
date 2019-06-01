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

        private async Task PostTestExampleRequest()
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
        }

        [Test]
        public void CheckExampleModule()
        {
            var module = new ExampleModule(Context);

            Assert.IsNotNull(module.Manager);
            Assert.IsNotNull(module.RequestManager);
        }

        [Test]
        public async Task Post()
        {
            await PostTestExampleRequest();

            Assert.AreEqual(1, Context.Examples.Count());
            Assert.AreEqual(3, Context.ExampleCodeLines.Count());
            Assert.AreEqual(1, Context.ExampleRequests.Count());
            Assert.AreEqual("This is a test example", Context.Examples.First().Description);
            Assert.IsNotNull(Context.ExampleRequests.First().DataId);
            Assert.IsNotNull(Context.ExampleRequests.First().Data);
            Assert.IsNotNull(Context.Examples.First().Request);
            Assert.IsNotNull(Context.Examples.First().RequestId);
            Assert.AreEqual(Context.ExampleRequests.First().Id, Context.Examples.First().RequestId.Value);
            Assert.AreEqual(Context.ExampleRequests.First(), Context.Examples.First().Request);
            Assert.AreEqual(Context.Examples.First().Id, Context.ExampleRequests.First().DataId);
            Assert.AreEqual(Context.Examples.First(), Context.ExampleRequests.First().Data);
        }

        [Test]
        public async Task Post_Error_Invalid()
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

            //Invalid POST
            var old = ex.Data;
            ex.Data = null;
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => Manager.PostAsync(ex));
            ex.Data = old;
            ex.ApplyToId = 1;
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => Manager.PostAsync(ex));
            ex.ApplyToId = null;
            ex.Data.Symbol = null;
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => Manager.PostAsync(ex));
            ex.Data.User = null;
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => Manager.PostAsync(ex));

            //Invalid PATCH
            ex.Type = Data.Models.Examples.ExampleRequestType.PATCH;
            old = ex.Data;
            ex.Data = null;
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => Manager.PostAsync(ex));
            ex.Data = old;
            ex.ApplyToId = null;
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => Manager.PostAsync(ex));

            //Invalid DELETE
            ex.Type = Data.Models.Examples.ExampleRequestType.DELETE;
            ex.ApplyToId = 1;
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => Manager.PostAsync(ex));
            ex.Data = null;
            ex.ApplyToId = null;
            Assert.ThrowsAsync<Shared.Exceptions.InvalidResource>(() => Manager.PostAsync(ex));
        }

        [Test]
        public async Task Patch()
        {
            await PostTestExampleRequest();

            await Manager.PatchAsync(1, new ExampleRequestUpdate()
            {
                Message = "An updated message"
            }.CreatePatch(Context.ExampleRequests.First()));
            Assert.AreEqual(1, Context.ExampleRequests.Count());
            Assert.AreEqual(1, Context.Examples.Count());
            Assert.AreEqual(3, Context.ExampleCodeLines.Count());
            Assert.AreEqual("An updated message", Context.ExampleRequests.First().Message);
        }

        [Test]
        public async Task Patch_Complex_1()
        {
            await PostTestExampleRequest();

            await Manager.PatchAsync(1, new ExampleRequestUpdate()
            {
                Message = "An updated message",
                Data = new ExampleUpdate()
                {
                    Description = "Updated"
                }
            }.CreatePatch(Context.ExampleRequests.First()));
            Assert.AreEqual(1, Context.ExampleRequests.Count());
            Assert.AreEqual(1, Context.Examples.Count());
            Assert.AreEqual(3, Context.ExampleCodeLines.Count());
            Assert.AreEqual("An updated message", Context.ExampleRequests.First().Message);
            Assert.AreEqual("Updated", Context.ExampleRequests.First().Data.Description);
        }

        [Test]
        public async Task Patch_Complex_2()
        {
            await PostTestExampleRequest();

            await Manager.PatchAsync(1, new ExampleRequestUpdate()
            {
                Message = "An updated message",
                Data = new ExampleUpdate()
                {
                    Description = "Updated",
                    Code = new ExampleUpdate.CodeLine[]
                    {
                        new ExampleUpdate.CodeLine()
                        {
                            Comment = "test",
                            Data = "int main() { }"
                        }
                    }
                }
            }.CreatePatch(Context.ExampleRequests.First()));
            Assert.AreEqual(1, Context.ExampleRequests.Count());
            Assert.AreEqual(1, Context.Examples.Count());
            Assert.AreEqual(1, Context.ExampleCodeLines.Count());
            Assert.AreEqual("An updated message", Context.ExampleRequests.First().Message);
            Assert.AreEqual("Updated", Context.ExampleRequests.First().Data.Description);
            Assert.AreEqual("test", Context.ExampleRequests.First().Data.Code.First().Comment);
            Assert.AreEqual("int main() { }", Context.ExampleRequests.First().Data.Code.First().Data);
        }

        [Test]
        public async Task GetAll()
        {
            await PostTestExampleRequest();

            var res = Manager.GetAll();
            Assert.AreEqual(1, res.Count());
            Assert.AreEqual("This is a test", res.First().Message);
            Assert.AreEqual("This is a test example", res.First().Data.Description);
        }

        [Test]
        public async Task GetForSymbol()
        {
            await PostTestExampleRequest();

            var res = Manager.GetAll(1);
            Assert.AreEqual(1, res.Count());
            Assert.AreEqual("This is a test", res.First().Message);
            Assert.AreEqual("This is a test example", res.First().Data.Description);
        }

        [Test]
        public async Task ApplyRequest_POST()
        {
            //POST request
            await PostTestExampleRequest();
            Assert.AreEqual(1, Context.Examples.Count());
            Assert.IsNotNull(Context.Examples.First().Request);
            await Manager.ApplyRequest(Context.ExampleRequests.First().Id);
            Assert.AreEqual(1, Context.Examples.Count());
            Assert.IsNull(Context.Examples.First().Request);
        }

        [Test]
        public async Task ApplyRequest_PATCH()
        {
            //POST request
            await PostTestExampleRequest();
            Assert.AreEqual(1, Context.ExampleRequests.Count());
            await Manager.ApplyRequest(Context.ExampleRequests.First().Id);
            Assert.AreEqual(0, Context.ExampleRequests.Count());
            Assert.AreEqual(1, Context.Examples.Count());
            Assert.IsNull(Context.Examples.First().Request);
            Assert.AreEqual(3, Context.ExampleCodeLines.Count());
            Assert.AreEqual("This is a test example", Context.Examples.First().Description);
            var ex = new ExampleRequestCreate()
            {
                Message = "Patching an example",
                Method = Data.Models.Examples.ExampleRequestType.PATCH,
                ApplyTo = 1,
                Data = new ExampleCreate()
                {
                    Description = "This has been updated",
                    Code = new ExampleCreate.CodeLine[]
                    {
                        new ExampleCreate.CodeLine()
                        {
                            Data = "void main() { }",
                            Comment = "inline main function"
                        }
                    }
                }
            }.CreateModel();
            ex.Data.Symbol = Context.Symbols.First();
            ex.Data.User = User.User;
            await Manager.PostAsync(ex);
            Assert.AreEqual(1, Context.ExampleRequests.Count());
            Assert.IsNull(Context.Examples.First().Request);
            Assert.IsNotNull(Context.Examples.Last().Request);
            await Manager.ApplyRequest(Context.ExampleRequests.First().Id);
            Assert.AreEqual(0, Context.ExampleRequests.Count());
            Assert.AreEqual("This has been updated", Context.Examples.First().Description);
            Assert.AreEqual(1, Context.ExampleCodeLines.Count());
            Assert.AreEqual("void main() { }", Context.Examples.First().Code.First().Data);
            Assert.AreEqual("inline main function", Context.Examples.First().Code.First().Comment);
        }

        [Test]
        public async Task ApplyRequest_DELETE()
        {
            //POST request
            await PostTestExampleRequest();
            Assert.AreEqual(1, Context.Examples.Count());
            Assert.IsNotNull(Context.Examples.First().Request);
            await Manager.ApplyRequest(Context.ExampleRequests.First().Id);
            Assert.AreEqual(1, Context.Examples.Count());
            Assert.IsNull(Context.Examples.First().Request);
            await Manager.PostAsync(new ExampleRequestCreate()
            {
                ApplyTo = 1,
                Method = Data.Models.Examples.ExampleRequestType.DELETE,
                Message = "Delete the useless example, it's only a test anyway"
            }.CreateModel());
            await Manager.ApplyRequest(Context.ExampleRequests.First().Id);
            Assert.AreEqual(0, Context.Examples.Count());
            Assert.AreEqual(0, Context.ExampleRequests.Count());
            Assert.AreEqual(0, Context.ExampleCodeLines.Count());
        }
    }
}
