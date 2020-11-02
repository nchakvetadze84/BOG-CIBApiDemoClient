using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CIBApiDemoClient
{
   // public class OAuth2Client
   // {
   //     protected HttpClient Client;
   //     protected ClientAuthenticationStyle AuthenticationStyle;
   //     protected Uri Address;
   //     protected string ClientId;
   //     protected string ClientSecret;

   //     public enum ClientAuthenticationStyle
   //     {
   //         BasicAuthentication,
   //         PostValues,
   //         None
   //     };

   //     public OAuth2Client(Uri address)
   //         : this(address, new HttpClientHandler())
   //     { }

   //     public OAuth2Client(Uri address, HttpMessageHandler innerHttpClientHandler)
   //     {
   //         if (innerHttpClientHandler == null)
   //         {
   //             throw new ArgumentNullException("innerHttpClientHandler");
   //         }

   //         Client = new HttpClient(innerHttpClientHandler)
   //         {
   //             BaseAddress = address
   //         };

   //         Address = address;
   //         AuthenticationStyle = ClientAuthenticationStyle.None;
   //     }

   //     public string CreateAuthorizeUrl(string clientId, string responseType, string scope = null, string redirectUri = null, string state = null, Dictionary<string, string> additionalValues = null)
   //     {
   //         var values = new Dictionary<string, string>
			//{
			//	{ OAuth2Constants.ClientId, clientId },
			//	{ OAuth2Constants.ResponseType, responseType }
			//};

   //         if (!string.IsNullOrWhiteSpace(scope))
   //         {
   //             values.Add(OAuth2Constants.Scope, scope);
   //         }

   //         if (!string.IsNullOrWhiteSpace(redirectUri))
   //         {
   //             values.Add(OAuth2Constants.RedirectUri, redirectUri);
   //         }

   //         if (!string.IsNullOrWhiteSpace(state))
   //         {
   //             values.Add(OAuth2Constants.State, state);
   //         }

   //         return CreateAuthorizeUrl(Address, Merge(values, additionalValues));
   //     }

   //     public static string CreateAuthorizeUrl(Uri endpoint, Dictionary<string, string> values)
   //     {
   //         var qs = string.Join("&", values.Select(kvp => String.Format("{0}={1}", WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(kvp.Value))).ToArray());
   //         return string.Format("{0}?{1}", endpoint.AbsoluteUri, qs);
   //     }

   //     private Dictionary<string, string> Merge(Dictionary<string, string> explicitValues, Dictionary<string, string> additionalValues = null)
   //     {
   //         var merged = explicitValues;

   //         if (AuthenticationStyle == ClientAuthenticationStyle.PostValues)
   //         {
   //             merged.Add(OAuth2Constants.ClientId, ClientId);
   //             merged.Add(OAuth2Constants.ClientSecret, ClientSecret);
   //         }

   //         if (additionalValues != null)
   //         {
   //             merged =
   //                 explicitValues.Concat(additionalValues.Where(add => !explicitValues.ContainsKey(add.Key)))
   //                                      .ToDictionary(final => final.Key, final => final.Value);
   //         }

   //         return merged;
   //     }
   // }
}