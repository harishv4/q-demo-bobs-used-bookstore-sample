using Microsoft.Extensions.Configuration;
using System.ServiceModel;

namespace Bookstore.Web.WcfClients
{
    public class WcfClientFactory
    {
        private readonly IConfiguration _configuration;

        public WcfClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IBookSearchServiceClient CreateBookSearchServiceClient()
        {
            var baseAddress = _configuration["WcfService:BaseAddress"];
            var endpoint = _configuration["WcfService:BookSearchServiceEndpoint"];
            var serviceAddress = $"{baseAddress}{endpoint}";

            var binding = new BasicHttpBinding();
            var endpointAddress = new EndpointAddress(serviceAddress);

            var channelFactory = new ChannelFactory<IBookSearchServiceClient>(binding, endpointAddress);
            return channelFactory.CreateChannel();
        }
    }
}