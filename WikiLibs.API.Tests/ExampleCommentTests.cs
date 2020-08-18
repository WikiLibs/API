using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.API.Examples;
using WikiLibs.API.Symbols;
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
    class ExampleCommentTests : DBTest<IExampleCommentsManager>
    {
        public override IExampleCommentsManager CreateManager()
        {
            return (new ExampleCommentManager(Context, new WikiLibs.Examples.Config()
            {
                MaxExampleRequestsPerPage = 100
            }));
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

        public async Task PostTestExample()
        {
            var controller = new SymbolController(new SymbolManager(Context, new WikiLibs.Symbols.Config() { }), User);
            var sym = await PostTestSymbol(controller);
            ExampleManager mgr = new ExampleManager(Context);
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
            await mgr.PostAsync(ex);
        }

        private ExampleCommentController NewController()
        {
            return (new ExampleCommentController(User, new ExampleModule(Context, new WikiLibs.Examples.Config()
            {
                MaxExampleRequestsPerPage = 100
            })));
        }

        [Test]
        public void CheckExampleModule()
        {
            var module = new ExampleModule(Context, new WikiLibs.Examples.Config()
            {
                MaxExampleRequestsPerPage = 100
            });

            Assert.IsNotNull(module.Manager);
            Assert.IsNotNull(module.RequestManager);
            Assert.IsNotNull(module.CommentsManager);
        }

        [Test]
        public async Task Post()
        {
            await PostTestExample();
            var controller = NewController();

            await controller.PostAsync(new ExampleCommentCreate()
            {
                ExampleId = 1,
                Data = "This is a test comment"
            });
            Assert.AreEqual(Context.ExampleComments.Count(), 1);
            Assert.AreEqual(Context.ExampleComments.First().Data, "This is a test comment");
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PostAsync(new ExampleCommentCreate() { }));
        }

        [Test]
        public async Task Patch()
        {
            await PostTestExample();
            var controller = NewController();

            await controller.PostAsync(new ExampleCommentCreate()
            {
                ExampleId = 1,
                Data = "This is a test comment"
            });
            Assert.AreEqual(Context.ExampleComments.Count(), 1);
            Assert.AreEqual(Context.ExampleComments.First().Data, "This is a test comment");
            await controller.PatchAsync(1, new ExampleCommentUpdate()
            {
                Data = "Updated comment by admin"
            });
            Assert.AreEqual(Context.ExampleComments.Count(), 1);
            Assert.AreEqual(Context.ExampleComments.First().Data, "Updated comment by admin");
            User.SetPermissions(new string[] { });
            await controller.PatchAsync(1, new ExampleCommentUpdate()
            {
                Data = "Updated comment by owner"
            });
            Assert.AreEqual(Context.ExampleComments.Count(), 1);
            Assert.AreEqual(Context.ExampleComments.First().Data, "Updated comment by owner");
            User.User.Id = "asjdiasjdiaj";
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchAsync(1, new ExampleCommentUpdate() { }));
        }
    }
}
