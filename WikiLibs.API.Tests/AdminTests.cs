using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Admin;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Models.Input.Admin;
using WikiLibs.Shared.Attributes;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class AdminTests : DBTest<AdminManager>
    {
        public override void Setup()
        {
            base.Setup();
            Manager = new AdminManager(Context, null);
        }

        private async Task<string> PostTestAPIKey()
        {
            var mdl = await Manager.APIKeyManager.PostAsync(new Data.Models.APIKey()
            {
                Description = "TEST_API_KEY",
                ExpirationDate = DateTime.MaxValue,
                Flags = AuthorizeApiKey.Authentication | AuthorizeApiKey.Registration | AuthorizeApiKey.Standard,
                UseNum = 2
            });
            return (mdl.Id);
        }

        [Test]
        public async Task PostAPIKey()
        {
            var key = await PostTestAPIKey();

            Assert.NotNull(key);
            Assert.True(Guid.TryParse(key, out Guid test));
            Assert.AreEqual(1, Context.APIKeys.Count());
            Assert.AreEqual("TEST_API_KEY", Context.APIKeys.First().Description);
        }

        [Test]
        public async Task PatchAPIKey()
        {
            var key = await PostTestAPIKey();

            await Manager.APIKeyManager.PatchAsync(key, new APIKeyUpdate()
            {
                Description = "SUPER API KEY",
                UseNum = 3
            }.CreatePatch(await Manager.APIKeyManager.GetAsync(key)));
            Assert.AreEqual(1, Context.APIKeys.Count());
            Assert.True(Guid.TryParse(Context.APIKeys.First().Id, out Guid test));
            Assert.AreEqual("SUPER API KEY", Context.APIKeys.First().Description);
            Assert.AreEqual(3, Context.APIKeys.First().UseNum);
        }

        [Test]
        public async Task DeleteAPIKey()
        {
            var key = await PostTestAPIKey();

            await Manager.APIKeyManager.DeleteAsync(key);
            Assert.AreEqual(0, Context.APIKeys.Count());
        }

        [Test]
        public async Task GetAPIKey()
        {
            var key = await PostTestAPIKey();

            var mdl = await Manager.APIKeyManager.GetAsync(key);
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

            Assert.True(Manager.APIKeyManager.Exists(key));
            Assert.False(Manager.APIKeyManager.Exists("1231"));
            Assert.False(Manager.APIKeyManager.Exists(null));
        }

        [Test]
        public async Task UseAPIKey()
        {
            var key = await PostTestAPIKey();

            var mdl = await Manager.APIKeyManager.GetAsync(key);
            Assert.AreEqual(2, mdl.UseNum);
            await Manager.APIKeyManager.UseAPIKey(key);
            mdl = await Manager.APIKeyManager.GetAsync(key);
            Assert.AreEqual(1, mdl.UseNum);
            await Manager.APIKeyManager.UseAPIKey(key);
            Assert.ThrowsAsync<Shared.Exceptions.ResourceNotFound>(() => Manager.APIKeyManager.GetAsync(key));
        }

        [Test]
        public async Task GetAllAPIKeys()
        {
            var key = await PostTestAPIKey();
            var res = Manager.APIKeyManager.GetAll();

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
            var res = Manager.APIKeyManager.GetAllOfDescription("TEST_API_KEY");

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
    }
}
