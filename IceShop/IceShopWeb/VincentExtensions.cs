using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;

namespace IceShopWeb
{
    public static class VincentExtensions
    {

        public async static Task<T> GetDataAsync<T>(this Controller controller, string request)
        {
            using var client = MakeInsecureHttpClient();

            Task<HttpResponseMessage> response = client.GetAsync(request);

            //response.Wait();

            HttpResponseMessage result = await response;//.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<T>();
                //readTask.Wait();

                T resultData = await readTask;//.Result;

                return resultData;
            }
            else return default;
        }

        public static T GetData<T>(string request)
        {
            using var client = MakeInsecureHttpClient();

            Task<HttpResponseMessage> response = client.GetAsync(request);

            response.Wait();

            HttpResponseMessage result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<T>();
                readTask.Wait();

                T resultData = readTask.Result;

                return resultData;
            }
            else return default;
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
