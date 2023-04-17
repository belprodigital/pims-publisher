using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PimsPublisher.Infrastructure.HttpClients
{
    public class Oauth2ClientCredential
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }

    }

    internal class Token
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }

    internal static class Oauth2Client
    {
        internal static async Task<string> AppAccessToken(Oauth2ClientCredential clientCredential, CancellationToken cancellationToken)
        {
            RestClient client = RestClient.For(clientCredential.Instance, $"{clientCredential.TenantId}/oauth2");

            var requestMessage = client.MakeRequest(HttpMethod.Post, "v2.0", "token")
                .WithApplicationFormContent(new Dictionary<string, string>
                {
                    {"client_id", clientCredential.ClientId },
                    {"client_secret", clientCredential.ClientSecret },
                    {"grant_type", "client_credentials" },
                    {"response_type", "token" },
                    {"scope", clientCredential.Scopes }
                });

            var response = await client.SendAsync(cancellationToken);

            Token? token = await response.Content.ReadFromJsonAsync<Token>(new JsonSerializerOptions
            {
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true
            }, cancellationToken);

            if (token != null)
            {
                return token.AccessToken;
            }
            else
            {
                throw new Exception($"Faild in request token to {requestMessage.RequestUri} for ClientId: {clientCredential.ClientId}");

            }
        }
    }
}
