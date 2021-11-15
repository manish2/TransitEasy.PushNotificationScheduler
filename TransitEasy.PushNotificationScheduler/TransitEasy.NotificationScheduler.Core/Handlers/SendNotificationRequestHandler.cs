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
            var targetSchedule = apiResponse.NextBusStopInfo.Where(stopInfo => stopInfo.RouteDescription.Contains(request.RouteNo)).FirstOrDefault();
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
                        var message = $"Oh no! Looks like your trip was cancelled for stop {request.StopNo}";
                        await _firebaseApiClient.SendNotificationToFCM(new FCMRequest { MessageBody = message, RegistrationToken = request.FirebaseDeviceToken });
                    }
                    else
                    {
                        var message = GetPushNotificationMessageFromStatus(schedule.ScheduleStatus);
                        await _firebaseApiClient.SendNotificationToFCM(new FCMRequest { MessageBody = message, RegistrationToken = request.FirebaseDeviceToken });
                    }
                }
            }
            return new SendNotificationResponse
            {

            };
        }

        private string GetPushNotificationMessageFromStatus(NextBusScheduleStatus status)
        => status switch
        {
            NextBusScheduleStatus.DELAYED => "DELAYED",
            NextBusScheduleStatus.AHEAD => "EARLY",
            NextBusScheduleStatus.ONTIME => "ON TIME",
            _ => "ON TIME",
        };
    }
}
