using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiLibs.Admin;
using WikiLibs.API.Admin;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Models.Input.Admin;
using WikiLibs.Shared.Attributes;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class AdminTests : DBTest<AdminManager>
    {
        public override AdminManager CreateManager()
        {
            return (new AdminManager(Context, LogUtils.FakeLogger<AdminManager>()));
        }

        private async Task<string> PostTestAPIKey()
        {
            var mdl = await Manager.ApiKeyManager.PostAsync(new Data.Models.ApiKey()
            {
                Description = "TEST_API_KEY",
                ExpirationDate = DateTime.MaxValue,
                Flags = AuthorizeApiKey.Authentication | AuthorizeApiKey.Registration | AuthorizeApiKey.Standard,
                UseNum = 2
            });
            return (mdl.Id);
        }

        [Test]
        public void CheckAdminModuleInitializer()
        {
            AdminManager.Initialize(Manager);

            Assert.AreEqual(2, Context.Groups.Count());
            Assert.AreEqual(1, Context.ApiKeys.Count());
            Assert.AreEqual("Default", Context.Groups.First().Name);
            Assert.AreEqual("Admin", Context.Groups.ToList().Last().Name);
            Assert.AreEqual("[WIKILIBS_SUPER_DEV_API_KEY]", Context.ApiKeys.First().Description);

            Context.RemoveRange(Context.Groups);
            Context.RemoveRange(Context.ApiKeys);
            Context.SaveChanges();
            AdminManager.Initialize(Manager);

            Assert.AreEqual(2, Context.Groups.Count());
            Assert.AreEqual(1, Context.ApiKeys.Count());
            Assert.AreEqual("Default", Context.Groups.First().Name);
            Assert.AreEqual("Admin", Context.Groups.ToList().Last().Name);
            Assert.AreEqual("[WIKILIBS_SUPER_DEV_API_KEY]", Context.ApiKeys.First().Description);
        }

        [Test]
        public async Task PostAPIKey()
        {
            var key = await PostTestAPIKey();

            Assert.NotNull(key);
            Assert.True(Guid.TryParse(key, out Guid test));
            Assert.AreEqual(1, Context.ApiKeys.Count());
            Assert.AreEqual("TEST_API_KEY", Context.ApiKeys.First().Description);
        }

        [Test]
        public async Task PatchAPIKey()
        {
            var key = await PostTestAPIKey();

            await Manager.ApiKeyManager.PatchAsync(key, new ApiKeyUpdate()
            {
                Description = "SUPER API KEY",
                ExpirationDate = DateTime.MaxValue,
                Flags = AuthorizeApiKey.Authentication,
                UseNum = 3
            }.CreatePatch(await Manager.ApiKeyManager.GetAsync(key)));
            Assert.AreEqual(1, Context.ApiKeys.Count());
            Assert.True(Guid.TryParse(Context.ApiKeys.First().Id, out Guid test));
            Assert.AreEqual("SUPER API KEY", Context.ApiKeys.First().Description);
            Assert.AreEqual(3, Context.ApiKeys.First().UseNum);
            Assert.AreEqual(DateTime.MaxValue, Context.ApiKeys.First().ExpirationDate);
            Assert.AreEqual(AuthorizeApiKey.Authentication, Context.ApiKeys.First().Flags);
        }

        [Test]
        public async Task DeleteAPIKey()
        {
            var key = await PostTestAPIKey();

            await Manager.ApiKeyManager.DeleteAsync(key);
            Assert.AreEqual(0, Context.ApiKeys.Count());
        }

        [Test]
        public async Task GetAPIKey()
        {
            var key = await PostTestAPIKey();

            var mdl = await Manager.ApiKeyManager.GetAsync(key);
            Assert.AreEqual("TEST_API_KEY", mdl.Description);
            Assert.AreEqual(2, mdl.UseNum);
            Assert.AreEqual(DateTime.MaxValue, mdl.ExpirationDate);
            Assert.AreEqual(key, mdl.Id);
            Assert.AreEqual(AuthorizeApiKey.Authentication | AuthorizeApiKey.Registration | AuthorizeApiKey.Standard, mdl.Flags);
        }

        [Test]
        public async Task APIKeyExists()
        {
            var key = await PostTestAPIKey();

            Assert.True(Manager.ApiKeyManager.Exists(key));
            Assert.False(Manager.ApiKeyManager.Exists("1231"));
            Assert.False(Manager.ApiKeyManager.Exists(null));
        }

        [Test]
        public async Task UseAPIKey()
        {
            var key = await PostTestAPIKey();

            var mdl = await Manager.ApiKeyManager.GetAsync(key);
            Assert.AreEqual(2, mdl.UseNum);
            await Manager.ApiKeyManager.UseAPIKey(key);
            mdl = await Manager.ApiKeyManager.GetAsync(key);
            Assert.AreEqual(1, mdl.UseNum);
            await Manager.ApiKeyManager.UseAPIKey(key);
            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => Manager.ApiKeyManager.GetAsync(key));
        }

        [Test]
        public async Task GetAllAPIKeys()
        {
            var key = await PostTestAPIKey();
            var res = Manager.ApiKeyManager.GetAll();

            Assert.AreEqual(1, res.Count());
            Assert.AreEqual("TEST_API_KEY", res.First().Description);
            Assert.AreEqual(2, res.First().UseNum);
            Assert.AreEqual(DateTime.MaxValue, res.First().ExpirationDate);
            Assert.AreEqual(key, res.First().Id);
            Assert.AreEqual(AuthorizeApiKey.Authentication | AuthorizeApiKey.Registration | AuthorizeApiKey.Standard, res.First().Flags);
        }

        [Test]
        public async Task GetAllAPIKeysOfDescription()
        {
            var key = await PostTestAPIKey();
            var res = Manager.ApiKeyManager.GetAllOfDescription("TEST_API_KEY");

            Assert.AreEqual(1, res.Count());
            Assert.AreEqual("TEST_API_KEY", res.First().Description);
            Assert.AreEqual(2, res.First().UseNum);
            Assert.AreEqual(DateTime.MaxValue, res.First().ExpirationDate);
            Assert.AreEqual(key, res.First().Id);
            Assert.AreEqual(AuthorizeApiKey.Authentication | AuthorizeApiKey.Registration | AuthorizeApiKey.Standard, res.First().Flags);
        }

        public async Task PostTestGroup()
        {
            await Manager.GroupManager.PostAsync(new GroupCreate()
            {
                Name = "TestGroup",
                Permissions = new string[]
                {
                    "perm1",
                    "perm2",
                    "perm3",
                    "perm4"
                }
            }.CreateModel());
        }

        [Test]
        public async Task PostGroup()
        {
            await PostTestGroup();

            Assert.AreEqual(3, Context.Groups.Count());
            Assert.AreEqual(5, Context.Permissions.Count());
        }

        [Test]
        public async Task PostGroup_Error_Dupe()
        {
            await PostTestGroup();

            Assert.ThrowsAsync<Shared.Exceptions.ResourceAlreadyExists>(() => PostTestGroup());
        }

        [Test]
        public async Task PatchGroup()
        {
            await PostTestGroup();
            var mdl = await Manager.GroupManager.GetAsync(3);

            await Manager.GroupManager.PatchAsync(3, new GroupUpdate()
            {
                Permissions = new string[]
                {
                    "perm1",
                    "perm3",
                }
            }.CreatePatch(mdl));
            Assert.AreEqual(3, Context.Groups.Count());
            Assert.AreEqual(3, Context.Permissions.Count());
            mdl = await Manager.GroupManager.GetAsync(3);
            Assert.AreEqual(2, mdl.Permissions.Count());
            Assert.AreEqual("TestGroup", mdl.Name);
            await Manager.GroupManager.PatchAsync(3, new GroupUpdate()
            {
                Name = "123"
            }.CreatePatch(mdl));
            mdl = await Manager.GroupManager.GetAsync(3);
            Assert.AreEqual("123", mdl.Name);
            Assert.AreEqual(2, mdl.Permissions.Count());
        }

        [Test]
        public async Task DeleteGroup()
        {
            await PostTestGroup();

            Assert.AreEqual(3, Context.Groups.Count());
            await Manager.GroupManager.DeleteAsync(3);
            Assert.AreEqual(2, Context.Groups.Count());
            Assert.AreEqual(1, Context.Permissions.Count());
        }

        [Test]
        public void DeleteInternalGroup()
        {
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => Manager.GroupManager.DeleteAsync(Manager.GroupManager.DefaultGroup));
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => Manager.GroupManager.DeleteAsync(Manager.GroupManager.AdminGroup));
        }

        [Test]
        public async Task DeleteUsedGroup()
        {
            await PostTestGroup();

            Assert.AreEqual(3, Context.Groups.Count());
            Context.Users.First().GroupId = Manager.GroupManager.Get("TestGroup").Id;
            await Context.SaveChangesAsync();
            Assert.AreEqual(Manager.GroupManager.Get("TestGroup").Id, Context.Users.First().GroupId);
            await Manager.GroupManager.DeleteAsync(Manager.GroupManager.Get("TestGroup").Id);
            Assert.AreEqual(2, Context.Groups.Count());
            Assert.AreEqual(Manager.GroupManager.DefaultGroup.Id, Context.Users.First().GroupId);
        }

        [Test]
        public void DeleteGroup_Error_NonExistant()
        {
            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => Manager.GroupManager.DeleteAsync(1000 /* In test no more than 999 groups may be created as we are using an InMemoryDB */));
        }

        [Test]
        public async Task GetGroup()
        {
            await PostTestGroup();

            Assert.Throws<Shared.Exceptions.ResourceNotFound>(() => Manager.GroupManager.Get("doesnotexist"));
            var mdl = Manager.GroupManager.Get("TestGroup");
            Assert.AreEqual("TestGroup", mdl.Name);
            Assert.AreEqual(new string[]
            {
                "perm1",
                "perm2",
                "perm3",
                "perm4"
            }, mdl.Permissions.Select(o => o.Perm).ToArray());
            Assert.AreEqual(3, mdl.Id);
        }

        [Test]
        public async Task GetAllGroups()
        {
            await PostTestGroup();

            Assert.AreEqual(3, Manager.GroupManager.GetAll().Count());
        }

        [Test]
        public void InternalGroups()
        {
            Assert.IsNotNull(Manager.GroupManager.DefaultGroup);
            Assert.IsNotNull(Manager.GroupManager.AdminGroup);
            Assert.AreEqual("Default", Manager.GroupManager.DefaultGroup.Name);
            Assert.AreEqual("Admin", Manager.GroupManager.AdminGroup.Name);
        }

        [Test]
        public async Task Controller_POST_APIKey()
        {
            var controller = new ApiKeyController(User, Manager);
            var res = await controller.PostAsync(new ApiKeyCreate()
            {
                Description = "TestKey",
                ExpirationDate = DateTime.MaxValue,
                Flags = AuthorizeApiKey.Standard,
                UseNum = -1
            }) as JsonResult;
            var obj = res.Value as Models.Output.Admin.ApiKey;

            Assert.AreEqual("TestKey", obj.Description);
            Assert.AreEqual(-1, obj.UseNum);
            Assert.AreEqual(DateTime.MaxValue, obj.ExpirationDate);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PostAsync(new ApiKeyCreate()));
        }

        [Test]
        public async Task Controller_PATCH_APIKey()
        {
            var controller = new ApiKeyController(User, Manager);

            var str = await PostTestAPIKey();
            var res = await controller.PatchAsync(str, new ApiKeyUpdate()
            {
                Description = "TestKey",
                UseNum = 0
            }) as JsonResult;
            var obj = res.Value as Models.Output.Admin.ApiKey;

            Assert.AreEqual("TestKey", obj.Description);
            Assert.AreEqual(AuthorizeApiKey.Authentication | AuthorizeApiKey.Registration | AuthorizeApiKey.Standard, obj.Flags);
            Assert.AreEqual(0, obj.UseNum);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchAsync(str, null));
        }

        [Test]
        public async Task Controller_DELETE_APIKey()
        {
            var controller = new ApiKeyController(User, Manager);

            var str = await PostTestAPIKey();
            Assert.AreEqual(1, Context.ApiKeys.Count());
            await controller.DeleteAsync(str);
            Assert.AreEqual(0, Context.ApiKeys.Count());
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteAsync(null));
        }

        [Test]
        public async Task Controller_GET_APIKey()
        {
            var controller = new ApiKeyController(User, Manager);

            await PostTestAPIKey();
            var res = controller.Get() as JsonResult;
            var obj = res.Value as IEnumerable<Models.Output.Admin.ApiKey>;

            Assert.AreEqual(1, obj.Count());
            Assert.AreEqual("TEST_API_KEY", obj.First().Description);
            Assert.AreEqual(DateTime.MaxValue, obj.First().ExpirationDate);
            Assert.AreEqual(AuthorizeApiKey.Authentication | AuthorizeApiKey.Registration | AuthorizeApiKey.Standard, obj.First().Flags);
            Assert.AreEqual(2, obj.First().UseNum);
            User.SetPermissions(new string[] { });
            Assert.Throws<Shared.Exceptions.InsuficientPermission>(() => controller.Get());
        }

        [Test]
        public async Task Controller_POST_Group()
        {
            var controller = new GroupController(User, Manager);
            var res = await controller.PostAsync(new GroupCreate()
            {
                Name = "TestGrp",
                Permissions = new string[]
                {
                    "permission.*"
                }
            }) as JsonResult;
            var obj = res.Value as Models.Output.Admin.Group;

            Assert.AreEqual("TestGrp", obj.Name);
            Assert.AreEqual(1, obj.Permissions.Length);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PostAsync(new GroupCreate()));
        }

        [Test]
        public async Task Controller_PATCH_Group()
        {
            var controller = new GroupController(User, Manager);

            await PostTestGroup();
            var res = await controller.PatchAsync(Context.Groups.ToList().Last().Id, new GroupUpdate()
            {
                Name = "MyGrp123"
            }) as JsonResult;
            var obj = res.Value as Models.Output.Admin.Group;

            Assert.AreEqual("MyGrp123", obj.Name);
            Assert.AreEqual(4, obj.Permissions.Length);
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.PatchAsync(Context.Groups.ToList().Last().Id, null));
        }

        [Test]
        public async Task Controller_DELETE_Group()
        {
            var controller = new GroupController(User, Manager);

            await PostTestGroup();
            Assert.AreEqual(3, Context.Groups.Count());
            await controller.DeleteAsync(Context.Groups.ToList().Last().Id);
            Assert.AreEqual(2, Context.Groups.Count());
            User.SetPermissions(new string[] { });
            Assert.ThrowsAsync<Shared.Exceptions.InsuficientPermission>(() => controller.DeleteAsync(0));
        }

        [Test]
        public async Task Controller_GET_Group()
        {
            var controller = new GroupController(User, Manager);

            await PostTestGroup();
            var res = controller.Get() as JsonResult;
            var obj = res.Value as IEnumerable<Models.Output.Admin.Group>;

            Assert.AreEqual(3, obj.Count());
            Assert.AreEqual("Default", obj.First().Name);
            User.SetPermissions(new string[] { });
            Assert.Throws<Shared.Exceptions.InsuficientPermission>(() => controller.Get());
        }
    }
}
