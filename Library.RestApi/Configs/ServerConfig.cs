using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Library.RestApi.Configs
{
    class ServerConfig : Configuration
    {
        private const string UrlConfigKey = "url";

        public override void ConfigureServer(IWebHostBuilder host)
        {
            if (AppSettings.GetSection(UrlConfigKey).Exists())
            {
                host.UseUrls(AppSettings.GetValue<String>(UrlConfigKey));
            }
            
            host.UseKestrel();
            
            host.UseIISIntegration();
        }
    }
}
