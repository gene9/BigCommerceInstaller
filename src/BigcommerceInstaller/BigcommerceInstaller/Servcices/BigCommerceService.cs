using BigcommerceInstaller.Helpers;
using BigcommerceInstaller.Models;
using BigcommerceInstaller.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BigcommerceInstaller.Servcices
{
    public class BigCommerceService : IBigCommerceService
    {
        private readonly BigCommerceConfig _bigCommerceConfig;
        private readonly ChatigyConfig _chatigyConfig;
        private readonly ILogger<BigCommerceService> _logger;

        public BigCommerceService(IOptions<BigCommerceConfig> bigCommerceConfigAccessor, IOptions<ChatigyConfig> chatigyConfigAccessor, ILogger<BigCommerceService> logger)
        {
            _bigCommerceConfig = bigCommerceConfigAccessor?.Value ?? throw new ArgumentNullException(nameof(bigCommerceConfigAccessor));
            _chatigyConfig = chatigyConfigAccessor?.Value ?? throw new ArgumentNullException(nameof(chatigyConfigAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AuthResponse> DoAuthAsync(AuthRequest authRequest)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_bigCommerceConfig.TokenUrl);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", _bigCommerceConfig.ClientId),
                    new KeyValuePair<string, string>("client_secret", _bigCommerceConfig.ClientSecret),
                    new KeyValuePair<string, string>("redirect_uri", _bigCommerceConfig.RedirectUri),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("code", authRequest.Code),
                    new KeyValuePair<string, string>("scope", authRequest.Scope),
                    new KeyValuePair<string, string>("context", authRequest.Context),
                });

                var result = await client.PostAsync(_bigCommerceConfig.TokenUrl, content);

                var resultContent = await result.Content.ReadAsStringAsync();

                _logger.LogInformation(resultContent);

                var authResponse = JsonConvert.DeserializeObject<AuthResponse>(resultContent);

                await CreateScriptAsync(authResponse);

                return authResponse;
            }
        }

        private async Task CreateScriptAsync(AuthResponse authResponse)
        {
            string url = $"https://api.bigcommerce.com/stores/{authResponse.GetStoreHash()}/v3/content/scripts";

            var html = $@"<script src=\""{_chatigyConfig.Src}\""></script>";

            var jsonString = JsonConvert.SerializeObject(new
            {
                name = _chatigyConfig.Name,
                description = _chatigyConfig.Description,
                html = html,
                src = _chatigyConfig.Src,
                auto_uninstall = _chatigyConfig.AutoUninstall,
                load_method = _chatigyConfig.LoadMethod,
                location = _chatigyConfig.Location,
                visibility = _chatigyConfig.visibility,
                kind = _chatigyConfig.Kind
            });

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("X-Auth-Client", _bigCommerceConfig.ClientId);
                client.DefaultRequestHeaders.Add("X-Auth-Token", authResponse.AccessToken);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var result = await client.PostAsync(url, content);

                var resultContent = await result.Content.ReadAsStringAsync();

                _logger.LogInformation(resultContent);
            }
        }

        public async Task<string> DoLoadAsync(string signedPayload)
        {
            var jsonPart = signedPayload.Split('.')[0];
            var hmacSignaturePart = signedPayload.Split('.')[1];

            var decodedJsonPart = Base64Helper.Base64ForUrlDecode(jsonPart);
            var decodedHmacSignaturePart = Base64Helper.Base64ForUrlDecode(hmacSignaturePart);

            var html = @"<iframe id=""chatigy-iframe"" scrolling=""no"" src=""https://chatigy.justinchasez.space/chatbox?tenantDomain=genk.vn""></iframe>";

            return html;
        }

        private bool VerifySignedRequest()
        {
            // TODO
            return true;
        }
    }
}
