using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThirdPartyService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "3rd Party Book Seller";
            var identityClient = new HttpClient();
            var discovery = await identityClient.GetDiscoveryDocumentAsync("https://localhost:44380");

            var response = await identityClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discovery.TokenEndpoint,
                    ClientId = "bookseller",
                    ClientSecret = "easypassword",
                    Scope = "bookstore_apis bookstore_viewbook"
                });

            if (response.IsError)
            {
                Console.WriteLine(response.Error);
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Access Token");
            Console.WriteLine(response.AccessToken);    
            Console.WriteLine("\n");
            Console.WriteLine("-------------------------------------------------------------------------------------------");
            Console.WriteLine("\n");
            // call to book api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(response.AccessToken);

            var apiResponse = await apiClient.GetAsync("https://localhost:44308/api/book", HttpCompletionOption.ResponseContentRead);

            Console.WriteLine("Result");
            Console.WriteLine("\n");
            if (!apiResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(apiResponse.StatusCode);
            }
            else
            {
                var json = await apiResponse.Content.ReadAsStringAsync();
                Console.WriteLine(json);
            }
            Console.ReadLine();
            return;
        }
    }
}
