using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Kztek.Security;
using Kztek_Core.Models;
using Kztek_Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Kztek_Library.Helpers
{
    public class ApiHelper
    {
        public static HttpClient client;

        static ApiHelper()
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
        }

        public static string GenerateJSON_MobileToken(string userid)
        {
            //
            var now = DateTime.Now;
            var expire = now.AddDays(1);

            //
            var Issuer = AppSettingHelper.GetStringFromFileJson("appsettings", "Jwt:Issuer_Mobile").Result;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityModel.Mobile_Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                Issuer,
                Issuer,
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userid),
                },
                expires: expire,
                signingCredentials: credentials);

            var mo = new TokenModel()
            {
                Identifier = userid,
                Expires_In = (int)(expire - now).TotalMinutes,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return JsonConvert.SerializeObject(mo);
        }

        public static Task<T> ConvertResponse<T>(HttpResponseMessage response)
        {
            if (response != null && response.IsSuccessStatusCode)
            {
                var t = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                return Task.FromResult(t);
            }

            return null;
        }

        public static async Task<HttpResponseMessage> HttpGet(string uri, string token = "")
        {
            var url = AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

            try
            {
                if (!string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await client.GetAsync(url);
            }
            catch (System.Exception ex)
            {
                var re = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };

                return await Task.FromResult(re);
            }
        }

        public static async Task<HttpResponseMessage> HttpPost<T>(string uri, T obj, string token = "")
        {
            var url = AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

            try
            {
                if (!string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = JsonConvert.SerializeObject(obj);

                var data = new StringContent(content, Encoding.UTF8, "application/json");

                return await client.PostAsync(url, data);
            }
            catch (System.Exception ex)
            {
                var re = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };

                return await Task.FromResult(re);
            }
        }

        public static async Task<HttpResponseMessage> HttpPut<T>(string uri, T obj, string token = "")
        {
            var url = AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

            try
            {
                if (!string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var content = JsonConvert.SerializeObject(obj);

                var data = new StringContent(content, Encoding.UTF8, "application/json");

                return await client.PutAsync(url, data);
            }
            catch (System.Exception ex)
            {
                var re = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };

                return await Task.FromResult(re);
            }

        }

        public static async Task<HttpResponseMessage> HttpDelete(string uri, string token = "")
        {
            var url = AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

            try
            {
                if (!string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await client.DeleteAsync(url);
            }
            catch (System.Exception ex)
            {
                var re = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };

                return await Task.FromResult(re);
            }
        }
    }
}
