using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;

namespace BusinessLayer.Utils
{
    public static class PayPalConfig
    {
        private static PayPalHttpClient? _client;

        public static PayPalHttpClient GetClient()
        {
            if (_client == null)
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var clientId = configuration["PayPal:ClientId"];
                var clientSecret = configuration["PayPal:ClientSecret"];

                var environment = new SandboxEnvironment(clientId, clientSecret);
                _client = new PayPalHttpClient(environment);
            }

            return _client;
        }
    }
}
