using System;
using System.Net.Http;

namespace MovieService.Imdb
{
    public class CustomHttpClient : IDisposable
    {
        private HttpClient webClient = new HttpClient();

        public virtual string GetJson(string request)
        {
            return webClient.GetStringAsync(request).Result;
        }

        public void Dispose() { webClient.Dispose(); }
    }
}
