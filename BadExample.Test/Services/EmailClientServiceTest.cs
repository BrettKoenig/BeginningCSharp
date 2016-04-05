using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadExample.Service.Interfaces;
using BadExample.Service.Services;
using NSubstitute;
using Xunit;

namespace BadExample.Test.Services
{
    public class EmailClientServiceTest
    {
        private IEmailClientService _target;

        [Fact]
        public void LoginFails()
        {
            _target = new EmailClientService();

            var result = _target.Login("BadUser", "BadPassword");

            Assert.False(result);
        }
    }
}
