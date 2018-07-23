using BigcommerceInstaller.Models;
using System.Threading.Tasks;

namespace BigcommerceInstaller.Servcices
{
    public interface IBigCommerceService
    {
        Task<AuthResponse> DoAuthAsync(AuthRequest authRequest);

        Task<string> DoLoadAsync(string signedPayload);
    }
}
