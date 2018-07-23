namespace BigcommerceInstaller.Models
{
    public class AuthRequest
    {
        public string Code { get; set; }
        public string Scope { get; set; }
        public string Context { get; set; }
    }

    public static class AuthRequestExtensions
    {
        public static string GetStoreHash(this AuthRequest authRequest)
        {
            return authRequest.Context.Replace("stores/", "");
        }

        public static string[] GetScopes(this AuthRequest authRequest)
        {
            return authRequest.Scope.Split('+');
        }
    }
}
