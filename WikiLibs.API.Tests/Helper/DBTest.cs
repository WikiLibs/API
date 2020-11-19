using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;

namespace WikiLibs.API.Tests.Helper
{
    public abstract class DBTest<T>
        where T : class
    {
        private SqliteConnection _connection = new SqliteConnection("DataSource=:memory:");
        public Data.Context Context { get; private set; }
        public T Manager { get; set; }
        public FakeUser User { get; private set; }
        public TestingSmtp Smtp { get; private set; }

        public abstract T CreateManager();

        [SetUp]
        public virtual void Setup()
        {
            _connection.Open();
            Smtp = new TestingSmtp();
            DbUtils.CreateFakeDB(_connection);
            Context = new Data.Context(new DbContextOptionsBuilder().UseSqlite(_connection).UseLazyLoadingProxies().Options);
            User = new FakeUser(Context);
            Manager = CreateManager();
        }

        [TearDown]
        public void Teardown()
        {
            Context.Dispose();
            _connection.Close();
            Smtp = null;
        }

        public static void SetupUser(ControllerBase controller, string method, string role = "")
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.NameIdentifier, "userId"),
                new Claim(ClaimTypes.AuthenticationMethod, method),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(identity)
                }
            };
        }
    }
}
