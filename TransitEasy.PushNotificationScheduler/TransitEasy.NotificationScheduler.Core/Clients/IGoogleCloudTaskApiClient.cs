using System;
using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Clients
{
    public interface IGoogleCloudTaskApiClient
    {
        Task<string> CreateScheduledNotificationAsync(string payload, DateTime sendTimeInUtc); 
    }
}
