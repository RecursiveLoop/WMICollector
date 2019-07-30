using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using ARAMS.API;

namespace ARAMS.API.Tests
{
    public class ARAMSAPITests
    {
        public ARAMSAPITests()
        {
        }

        [Fact]
        public void TestFunctionHandlerMethod()
        {
            var function = new Function();
            var request = new APIGatewayProxyRequest();
            var context = new TestLambdaContext();
            
            var response = function.FunctionHandler(request, context);
            
            Assert.Equal(200, response.StatusCode);
            Assert.StartsWith("{\"message\":\"hello world\",\"location\":", response.Body);
        }
    }
}
