using Newtonsoft.Json;
using Plugin.Connectivity;
using Sales.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Services
{
    public class ApiService
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    isSuccess = false,
                    message = "Please turn on your intenet settings."
                    //message = Languages.TurnOnInternet,
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    isSuccess = false,
                    message = "No internet connection"
                    //message = Languages.NoInternet,
                };
            }

            return new Response
            {
                isSuccess = true,
            };
        }
        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response1 = await client.GetAsync(url);
                var answer = await response1.Content.ReadAsStringAsync();
                if (!response1.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = false,
                        message = answer
                    };
                }
                var lst = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    isSuccess = true,
                    result = lst
                };
            }
            catch(Exception ex)
            {
                return new Response
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }
    }
}
