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
        public TestingSmtp Smtp { get; private set; }
        public Data.Context Context { get; private set; }
        public T Manager { get; set; }
        public FakeUser User { get; private set; }

        [SetUp]
        public virtual void Setup()
        {
            Context = DbUtils.CreateFakeDB();
            User = new FakeUser(Context);
            Smtp = new TestingSmtp();
        }

        [TearDown]
        public virtual void Teardown()
        {
            Context = null;
            User = null;
            Manager = null;
            Smtp = null;
        }
    }
}
