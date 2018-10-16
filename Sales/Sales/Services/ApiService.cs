﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Sales.Common.Models;
using Sales.Helpers;

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
                    message = Languages.TurnOnInternet,
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    isSuccess = false,
                    message = Languages.NoInternet,
                };
            }

            return new Response
            {
                isSuccess = true,
            };
        }

        public async Task<TokenResponse> GetToken(string urlBase, string username, string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("Token",
                    new StringContent(string.Format(
                    "grant_type=password&username={0}&password={1}",
                    username, password),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(resultJSON);
                return result;
            }
            catch
            {
                return null;
            }
        }

        #region De forma no segura
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
            catch (Exception ex)
            {
                return new Response
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                var response1 = await client.PostAsync(url, content);
                var answer = await response1.Content.ReadAsStringAsync();
                if (!response1.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = false,
                        message = answer
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    isSuccess = true,
                    result = obj
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";
                var response1 = await client.PutAsync(url, content);
                var answer = await response1.Content.ReadAsStringAsync();
                if (!response1.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = false,
                        message = answer
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    isSuccess = true,
                    result = obj
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }

        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = false,
                        message = answer
                    };
                }
                return new Response
                {
                    isSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        } 
        #endregion


        #region De forma segura
        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
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
            catch (Exception ex)
            {
                return new Response
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model, string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response1 = await client.PostAsync(url, content);
                var answer = await response1.Content.ReadAsStringAsync();
                if (!response1.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = false,
                        message = answer
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    isSuccess = true,
                    result = obj
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id, string tokenType, string accessToken)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}/{id}";
                var response1 = await client.PutAsync(url, content);
                var answer = await response1.Content.ReadAsStringAsync();
                if (!response1.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = false,
                        message = answer
                    };
                }
                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    isSuccess = true,
                    result = obj
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }

        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id, string tokenType, string accessToken)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = false,
                        message = answer
                    };
                }
                return new Response
                {
                    isSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    isSuccess = false,
                    message = ex.Message
                };
            }
        }
        #endregion

        public async Task<Response> GetUser(string urlBase, string prefix, string controller, string email, string tokenType, string accessToken)
        {
            try
            {
                var getUserRequest = new GetUserRequest
                {
                    Email = email,
                };

                var request = JsonConvert.SerializeObject(getUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        isSuccess = false,
                        message = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<MyUserASP>(answer);
                return new Response
                {
                    isSuccess = true,
                    result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    isSuccess = false,
                    message = ex.Message,
                };
            }
        }


    }
}
