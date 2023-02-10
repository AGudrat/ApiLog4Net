using System.Net.Sockets;
using System.Net;

namespace Log4net.API.Configurations
{
    public class CustomMiddleware
    {
        private readonly ILogger<CustomMiddleware> _logger;
        private readonly RequestDelegate _next;

        public CustomMiddleware(ILogger<CustomMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    log4net.LogicalThreadContext.Properties["Address"] = ip.ToString();
                    await _next(context);
                    break;
                }
            }
            await _next(context);
        }
    }
}
