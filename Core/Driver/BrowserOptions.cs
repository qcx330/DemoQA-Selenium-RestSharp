using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace Core.Driver
{
    public class BrowserConfig
    {
        public bool Headless { get; set; }
        public List<string> Arguments { get; set; }
    }

    public static class BrowserOptions
    {
        private static IConfigurationRoot _configuration {get;}

        static BrowserOptions()
        {

            Console.WriteLine("current path: "+Directory.GetCurrentDirectory());

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("browser_settings.json");
                

            _configuration = builder.Build();
        }

        public static BrowserConfig GetBrowserConfig(string browser)
        {
            Console.WriteLine("Create browser config for: "+browser);

            return _configuration.GetSection($"Browsers:{browser}").Get<BrowserConfig>();
        }

    }
}