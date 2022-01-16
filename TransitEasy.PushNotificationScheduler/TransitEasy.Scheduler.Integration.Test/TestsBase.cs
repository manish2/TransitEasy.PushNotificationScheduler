using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransitEasy.PushNotificationScheduler;

namespace TransitEasy.Scheduler.Integration.Test
{
    public class TestsBase
    {
        protected TestServer Server { get; private set; }
        public TestsBase()
        {
            Server = new TestServer(new WebHostBuilder()
                .UseConfiguration(
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                )
                .UseStartup<Startup>());
        }
    }
}
