using Newtonsoft.Json;

namespace BigcommerceInstaller.Models
{
    public class AuthResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        public string Scope { get; set; }
        public string Context { get; set; }
        public AuthResponseUser User { get; set; }
    }

    public class AuthResponseUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }

    public static class AuthResponseExtensions
    {
        public static string GetStoreHash(this AuthResponse authResponse)
        {
            return authResponse.Context.Replace("stores/", "");
        }

        public static string[] GetScopes(this AuthResponse authResponse)
        {
            return authResponse.Scope.Split('+');
        }
    }
}
