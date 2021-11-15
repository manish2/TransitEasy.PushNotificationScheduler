using FirebaseAdmin;
using Microsoft.Extensions.Hosting;

namespace TransitEasy.PushNotificationScheduler.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder WithFirebaseSDK(this IHostBuilder builder)
        {
            FirebaseApp.Create();
            return builder; 
        }
    }
}
