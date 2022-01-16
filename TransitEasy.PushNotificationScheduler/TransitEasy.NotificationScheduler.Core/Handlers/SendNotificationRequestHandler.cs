using System;
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
                    .Where(schedule => schedule.Destination.Equals(request.Destination, StringComparison.InvariantCultureIgnoreCase) && IsDateTimeInBounds(schedule.ExpectedLeaveTime, request.ExpectedLeaveTime))
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
                        var message = GetPushNotificationMessageFromStatus(schedule.ScheduleStatus, targetSchedule.RouteDescription, request.StopNo, schedule.CountdownInMin);
                        await _firebaseApiClient.SendNotificationToFCM(new FCMRequest { MessageTitle = "Bus Alert!", MessageBody = message, RegistrationToken = request.FirebaseDeviceToken });
                    }
                }
                else
                {
                    var message = $"Oh no! Could not find your schedule. Please check if it may have been severely delayed or cancelled.";
                    await _firebaseApiClient.SendNotificationToFCM(new FCMRequest { MessageTitle = "Bus Alert!", MessageBody = message, RegistrationToken = request.FirebaseDeviceToken });
                }
            }
            return new SendNotificationResponse
            {
                Message = "Notification was scheduled successfully"
            };
        }

        private bool IsDateTimeInBounds(DateTime first, DateTime second)
        {
            if (first == second)
                return true;
            else
            {
                var difference = first > second ? (first - second).TotalMinutes : (second - first).TotalMinutes;
                return difference <= 5;
            }
        }   

        private string GetPushNotificationMessageFromStatus(NextBusScheduleStatus status, string routeDesc, string stopNumber, int countDownInMin)
        => status switch
        {
            NextBusScheduleStatus.DELAYED => $"{routeDesc} will be delayed from departing {stopNumber}, please check local traffic. Arriving {Math.Abs(countDownInMin)} later",
            NextBusScheduleStatus.AHEAD => $"{routeDesc} will be departing early from stop {stopNumber}, leave earlier than usual. Arriving {Math.Abs(countDownInMin)} earlier",
            NextBusScheduleStatus.ONTIME => $"{routeDesc} will be on time for stop {stopNumber}. Arriving in {Math.Abs(countDownInMin)} min",
            _ => "ON TIME",
        };
    }
}
