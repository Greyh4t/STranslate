using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace STranslate.Util
{
    public class HttpUtil
    {
        /// <summary>
        /// �첽Get����(����Token)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url, int timeout = 10) => await GetAsync(url, CancellationToken.None, timeout);

        /// <summary>
        /// �첽Get����
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url, CancellationToken token, int timeout = 10)
        {
            using var client = new HttpClient(new SocketsHttpHandler()) { Timeout = TimeSpan.FromSeconds(timeout) };

            var respContent = await client.GetAsync(url, token);

            string respStr = await respContent.Content.ReadAsStringAsync(token);

            return respStr;
        }

        /// <summary>
        /// �첽Get���󣬴���ѯ����
        /// </summary>
        /// <param name="url">�����URL</param>
        /// <param name="queryParams">��ѯ�����ֵ�</param>
        /// <param name="token">ȡ������</param>
        /// <param name="timeout">��ʱʱ�䣨�룩</param>
        /// <returns></returns>
        public static async Task<string> GetAsync(
            string url,
            Dictionary<string, string> queryParams,
            CancellationToken token,
            int timeout = 10
        )
        {
            using var client = new HttpClient(new SocketsHttpHandler()) { Timeout = TimeSpan.FromSeconds(timeout) };
            // ��������ѯ������URL
            if (queryParams != null && queryParams.Count > 0)
            {
                var uriBuilder = new UriBuilder(url);
                var query = queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}");
                uriBuilder.Query = string.Join("&", query);
                url = uriBuilder.ToString();
            }

            var respContent = await client.GetAsync(url, token);

            string respStr = await respContent.Content.ReadAsStringAsync(token);

            return respStr;
        }

        /// <summary>
        /// �첽Post����(����Token)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, string req, int timeout = 10) =>
            await PostAsync(url, req, CancellationToken.None, timeout);

        /// <summary>
        /// �첽Post����(Body)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, string req, CancellationToken token, int timeout = 10)
        {
            using var client = new HttpClient(new SocketsHttpHandler()) { Timeout = TimeSpan.FromSeconds(timeout) };

            var content = new StringContent(req, Encoding.UTF8, "application/json");

            var respContent = await client.PostAsync(url, content, token);

            string respStr = await respContent.Content.ReadAsStringAsync(token);

            return respStr;
        }

        /// <summary>
        /// �첽Post����(QueryParams��Header��Body)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="req"></param>
        /// <param name="queryParams"></param>
        /// <param name="headers"></param>
        /// <param name="token"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, string req, Dictionary<string, string> queryParams, Dictionary<string, string> headers, CancellationToken token, int timeout = 10)
        {
            using var client = new HttpClient(new SocketsHttpHandler()) { Timeout = TimeSpan.FromSeconds(timeout) };
            // ��������ѯ������URL
            if (queryParams != null && queryParams.Count > 0)
            {
                var uriBuilder = new UriBuilder(url);
                var query = queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}");
                uriBuilder.Query = string.Join("&", query);
                url = uriBuilder.ToString();
            }
            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
            request.Content = new StringContent(req, Encoding.UTF8, "application/json");
            headers.ToList().ForEach(header => request.Headers.Add(header.Key, header.Value));

            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(request, token);
            // Read response as a string.
            string result = await response.Content.ReadAsStringAsync(token);

            return result;
        }
    }
}