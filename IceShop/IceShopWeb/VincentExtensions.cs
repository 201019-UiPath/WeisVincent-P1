using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IceShopWeb
{
    public static class VincentExtensions
    {
        private static HttpClient httpClient;

        public static HttpClient HttpClient
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = MakeInsecureHttpClient();
                    return httpClient;
                }
                else return httpClient;
            }
        }

        public async static Task<T> GetDataAsync<T>(this Controller controller, string request)
        {
            Task<HttpResponseMessage> response = HttpClient.GetAsync(request);

            HttpResponseMessage result = await response;//.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<T>();
                //readTask.Wait();

                T resultData = await readTask;//.Result;

                return resultData;
            }
            else throw new HttpRequestException(result.StatusCode.ToString());
        }

        public static T GetData<T>(string request)
        {
            Task<HttpResponseMessage> response = HttpClient.GetAsync(request);

            response.Wait();

            HttpResponseMessage result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<T>();
                readTask.Wait();

                T resultData = readTask.Result;

                return resultData;
            }
            else throw new HttpRequestException(result.StatusCode.ToString());
        }

        public async static Task PostDataAsync<T>(this Controller controller, string request, T data)
        {
            Task<HttpResponseMessage> response = HttpClient.PostAsJsonAsync(request, data);

            HttpResponseMessage result = await response;
            if (result.IsSuccessStatusCode) return; else throw new HttpRequestException(result.StatusCode.ToString());
        }

        public async static Task PutDataAsync<T>(this Controller controller, string request, T data)
        {
            Task<HttpResponseMessage> response = HttpClient.PutAsJsonAsync(request, data);

            HttpResponseMessage result = await response;
            if (result.IsSuccessStatusCode) return; else throw new HttpRequestException(result.StatusCode.ToString());
        }

        

        public static HttpClient MakeInsecureHttpClient()
        {
            string url = "http://localhost:5000/api/";

            HttpClientHandler handler = new HttpClientHandler();
            //X509Certificate2 certificate = new X509Certificate2();
            //handler.ClientCertificates.Add(certificate);
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            var client = new HttpClient(handler);
            client.BaseAddress = new Uri(url);

            return client;
        }
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static void SetBoolean(this ISession session, string key, bool value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static bool? GetBoolean(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return null;
            }
            return BitConverter.ToBoolean(data, 0);
        }

        public static void SetDouble(this ISession session, string key, double value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static double? GetDouble(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return null;
            }
            return BitConverter.ToDouble(data, 0);
        }



    }
}
