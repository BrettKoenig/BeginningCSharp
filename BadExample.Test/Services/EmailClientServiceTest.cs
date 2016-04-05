using BadExample.Service.Interfaces;
using BadExample.Service.Services;
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
