using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.API.Tests.Helper;
using WikiLibs.Auth;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class AuthTests : DBTest<AuthManager>
    {
        public override void Setup()
        {
            base.Setup();
            //Manager = new AuthManager();
        }

        [Test, Order(1)]
        public void BasicAuth()
        {
            //var controller
        }
    }
}
