using FirebaseAdmin.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Models.Request;

namespace TransitEasy.NotificationScheduler.Core.Clients
{
    public class FirebaseApiClient : IFirebaseApiClient
    {
        Dictionary<string, string> apnsHeaders = new Dictionary<string, string>()
        {
            { "apns-priority", "10"}
        };
        public async Task SendNotificationToFCM(FCMRequest fcmRequest)
        {
            var message = new Message
            {
                Token = fcmRequest.RegistrationToken,
                Notification = new Notification
                {
                    Title = fcmRequest.MessageTitle,
                    Body = fcmRequest.MessageBody
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                }
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
