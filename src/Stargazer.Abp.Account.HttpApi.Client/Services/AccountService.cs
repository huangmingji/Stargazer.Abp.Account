// using System;
// using System.Net.Http;
// using System.Text;
// using System.Threading.Tasks;
// using Stargazer.Abp.Account.Application;
// using Stargazer.Common.Extend;
// using Microsoft.Extensions.Configuration;
// using IdentityModel.Client;

// namespace Stargazer.Abp.Account.HttpApi.Client.Services
// {
//     public class AccountService : IAccountService
//     {
//         private readonly IConfiguration _configuration;
//         private readonly IHttpClientFactory _clientFactory;
//         public AccountService(IHttpClientFactory clientFactory,
//             IConfiguration configuration)
//         {
//             _clientFactory = clientFactory;
//             _configuration = configuration;
//         }

//         private async Task<String> getToken() {

//             var client = _clientFactory.CreateClient();
//             var disco = await client.GetDiscoveryDocumentAsync(_configuration.GetSection("RemoteServices:Authority").Value);
//             var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
//             {
//                 Address = disco.TokenEndpoint,

//                 ClientId = _configuration.GetSection("RemoteServices:Client").Value,
//                 ClientSecret = _configuration.GetSection("RemoteServices:Secret").Value,
//                 Scope = _configuration.GetSection("RemoteServices:Scope").Value
//             });
//             if (tokenResponse.IsError)
//             {
//                 return "";
//             }
//             return tokenResponse.AccessToken;
//         }

//         public async Task<ModelResponseDto<UserDto>> Get(Guid id)
//         {
//             var token = await getToken();
//             var client = _clientFactory.CreateClient("account");
//             client.SetBearerToken(token);
//             var responseMessage = await client.GetAsync("api/account/" + id);
//             var result = await responseMessage.Content.ReadAsStringAsync();
//             return result.DeserializeObject<ModelResponseDto<UserDto>>();
//         }

//         public async Task<ModelResponseDto<UserDto>> Register(AddUserDto data)
//         {
//             var client= _clientFactory.CreateClient("account");
//             StringContent stringContent = new StringContent(
//                 data.SerializeObject(),
//                 Encoding.UTF8
//             );
//             var responseMessage = await client.PostAsync("api/account/register", stringContent);
//             var result = await responseMessage.Content.ReadAsStringAsync();
//             return result.DeserializeObject<ModelResponseDto<UserDto>>();
//         }

//         public async Task<ModelResponseDto<UserDto>> VerifyPassword(VerifyPasswordDto data)
//         {
//             StringContent stringContent = new StringContent(
//                 data.SerializeObject(),
//                 Encoding.UTF8
//             );
//             var request = new HttpRequestMessage(HttpMethod.Post,"api/account/verify");
//             request.Content = stringContent;
//             var client= _clientFactory.CreateClient("account");
//             var responseMessage = await client.SendAsync(request);
//             var result = await responseMessage.Content.ReadAsStringAsync();
//             return result.DeserializeObject<ModelResponseDto<UserDto>>();
//         }
//     }
// }