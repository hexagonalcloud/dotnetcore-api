using System;
using Xunit;

namespace Specs
{
    //- Given a resource Products
    //- When a Client sends a GET request (or API receives a request, pbb makes more sense?)
    //- Then the API sends a Products responseB

    [Trait("Get Products Request", "")]
    public class GetProductsRequest
    {
        [Fact(DisplayName = "Send Products response")]
        public void Send_Products_Response()
        {
            throw new NotImplementedException("Implement me");
        }
    }
}
