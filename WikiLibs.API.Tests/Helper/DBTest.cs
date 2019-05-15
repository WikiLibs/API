using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Shared.Service;

namespace WikiLibs.API.Tests.Helper
{
    public class DBTest<T>
        where T : class
    {
        public TestingSmtp Smtp { get; set; }
        public Data.Context Context { get; set; }
        public T Manager { get; set; }
        public IUser FakeUser { get; set; }

        [SetUp]
        public virtual void Setup()
        {
            Context = DbUtils.CreateFakeDB();
            FakeUser = new FakeUser(Context);
            Smtp = new TestingSmtp();
        }

        [TearDown]
        public virtual void Teardown()
        {
            Context = null;
            FakeUser = null;
            Manager = null;
            Smtp = null;
        }
    }
}
