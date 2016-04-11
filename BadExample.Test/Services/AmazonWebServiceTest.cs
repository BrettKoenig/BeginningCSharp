using BadExample.Service.Interfaces;
using BadExample.Service.Services;
using Xunit;

namespace BadExample.Test.Services
{
    public class AmazonWebServiceTest
    {
        private IAmazonWebService _target;

        [Fact]
        public void UploadFileToBucket_ReturnsError()
        {
            _target = new AmazonWebService();
            var response = _target.UploadFileToBucket("", "sampleFileTest", "dev.faxmanager");
            Assert.True(response.HasError);
        }
    }
}
