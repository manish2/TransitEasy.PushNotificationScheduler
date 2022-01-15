using System.Linq;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Clients;
using TransitEasy.NotificationScheduler.Core.Models.Request;
using TransitEasy.NotificationScheduler.Core.Models.Result;

namespace TransitEasy.NotificationScheduler.Core.Handlers
{
    public class SendNotificationRequestHandler : IRequestHandler<SendNotificationRequest, SendNotificationResponse>
    {
        private readonly ITransitEasyApiClient _transitEasyApiClient;
        private readonly IFirebaseApiClient _firebaseApiClient; 
        public SendNotificationRequestHandler(ITransitEasyApiClient transitEasyApiClient, IFirebaseApiClient firebaseApiClient)
        {
            _transitEasyApiClient = transitEasyApiClient;
            _firebaseApiClient = firebaseApiClient; 
        }

        public async Task<SendNotificationResponse> HandleRequest(SendNotificationRequest request)
        {
            var apiResponse = await _transitEasyApiClient.GetNextBusSchedules(request.StopNo, request.NumberofNextBuses);
            var targetSchedule = apiResponse.NextBusStopInfo.Where(stopInfo => stopInfo.RouteDescription == request.RouteNo).FirstOrDefault();
            if (targetSchedule != null)
            {
                var schedule = targetSchedule
                    .Schedules
                    .Where(schedule => schedule.Destination.Equals(request.Destination, System.StringComparison.InvariantCultureIgnoreCase) && schedule.ExpectedLeaveTime == request.ExpectedLeaveTime)
                    .FirstOrDefault();

                if(schedule != null)
                {
                    if(schedule.IsTripCancelled)
                    {
                        var message = $"Oh no! Looks like your trip was cancelled for stop {request.StopNo}, you can check the service alerts page for more info";
                        await _firebaseApiClient.SendNotificationToFCM(new FCMRequest { MessageTitle = "Bus Alert!", MessageBody = message, RegistrationToken = request.FirebaseDeviceToken });
                    }
                    else
                    {
                        var message = GetPushNotificationMessageFromStatus(schedule.ScheduleStatus, targetSchedule.RouteDescription, request.StopNo);
                        await _firebaseApiClient.SendNotificationToFCM(new FCMRequest { MessageTitle = "Bus Alert!", MessageBody = message, RegistrationToken = request.FirebaseDeviceToken });
                    }
                }
            }
            return new SendNotificationResponse
            {
                Message = "Notification was scheduled successfully"
            };
        }

        private string GetPushNotificationMessageFromStatus(NextBusScheduleStatus status, string routeDesc, string stopNumber)
        => status switch
        {
            NextBusScheduleStatus.DELAYED => $"{routeDesc} will be delayed from departing {stopNumber}, please check local traffic",
            NextBusScheduleStatus.AHEAD => $"{routeDesc} will be departing early from stop {stopNumber}, leave earlier than usual",
            NextBusScheduleStatus.ONTIME => $"{routeDesc} will be on time for stop {stopNumber}",
            _ => "ON TIME",
        };
    }
}
