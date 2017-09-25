using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Api.Middleware
{
    /// <summary>
    /// https://gist.github.com/madskristensen/36357b1df9ddbfd123162cd4201124c4
    /// </summary>
    public class ETag
    {
        private readonly RequestDelegate _next;

        public ETag(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var response = context.Response;
            var originalStream = response.Body;

            using (var ms = new MemoryStream())
            {
                response.Body = ms;

                await _next(context);

                // TODO: verify other status codes to exclude (204 does not allow writing to response stream)
                if (context.Response.StatusCode == 204)
                {
                    return;
                }

                if (IsEtagSupported(response))
                {
                    string checksum = CalculateChecksum(ms);

                    response.Headers[HeaderNames.ETag] = checksum;

                    if (context.Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etag) && checksum == etag)
                    {
                        response.StatusCode = StatusCodes.Status304NotModified;
                        return;
                    }
                }

                ms.Position = 0;
                await ms.CopyToAsync(originalStream);
            }
        }

        private static bool IsEtagSupported(HttpResponse response)
        {
            if (response.StatusCode != StatusCodes.Status200OK)
            {
                return false;
            }

            // TODO: review > The 20kb length limit is not based in science. Feel free to change
            if (response.Body.Length > 20 * 1024)
            {
                return false;
            }

            if (response.Headers.ContainsKey(HeaderNames.ETag))
            {
                return false;
            }

            return true;
        }

        private static string CalculateChecksum(MemoryStream ms)
        {
            string checksum;

            using (var algo = SHA1.Create())
            {
                ms.Position = 0;
                byte[] bytes = algo.ComputeHash(ms);
                checksum = $"\"{WebEncoders.Base64UrlEncode(bytes)}\"";
            }

            return checksum;
        }
    }
}

