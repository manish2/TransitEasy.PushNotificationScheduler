using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Models.Request;

namespace TransitEasy.NotificationScheduler.Core.Clients
{
    public interface IFirebaseApiClient
    {
        Task SendNotificationToFCM(FCMRequest fcmRequest); 
    }
}
