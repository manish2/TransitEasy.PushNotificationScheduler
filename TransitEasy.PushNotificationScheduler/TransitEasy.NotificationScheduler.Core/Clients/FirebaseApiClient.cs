using FirebaseAdmin.Messaging;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Models.Request;

namespace TransitEasy.NotificationScheduler.Core.Clients
{
    public class FirebaseApiClient : IFirebaseApiClient
    {
        public async Task SendNotificationToFCM(FCMRequest fcmRequest)
        {
            var message = new Message
            {
                Token = fcmRequest.RegistrationToken,
                Notification = new Notification
                {
                    Title = "TESTING FROM SERVER",
                    Body = "HELLO FROM SERVER"
                }
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
