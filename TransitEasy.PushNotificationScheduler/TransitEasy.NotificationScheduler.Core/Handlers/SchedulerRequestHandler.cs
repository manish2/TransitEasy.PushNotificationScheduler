using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Clients;
using TransitEasy.NotificationScheduler.Core.Models.Request;
using TransitEasy.NotificationScheduler.Core.Models.Result;

namespace TransitEasy.NotificationScheduler.Core.Handlers
{
    public class SchedulerRequestHandler : IRequestHandler<CreateNotificationRequest, CreateNotificationResponse>
    {
        private readonly IGoogleCloudTaskApiClient _googleCloudTaskApiClient;
        //This is to take into the account the amount of time it takes to make an API call to translink and check the traffic updates
        private readonly TimeSpan NotificationProcessingTimeOffSet = TimeSpan.FromSeconds(30);
        //This is to take into the account the amount of time between sending the Notification to when it reaches the device
        private readonly TimeSpan NotificationSendTimeOffSet = TimeSpan.FromSeconds(5);
        private readonly ILogger<SchedulerRequestHandler> _logger;
        
        public SchedulerRequestHandler(IGoogleCloudTaskApiClient googleCloudTaskApiClient, ILogger<SchedulerRequestHandler> logger)
        {
            _googleCloudTaskApiClient = googleCloudTaskApiClient;
            _logger = logger; 
        }

        public async Task<CreateNotificationResponse> HandleRequest(CreateNotificationRequest request)
        {
            var sendNotificationRequest = new SendNotificationRequest
            {
                FirebaseDeviceToken = request.FirebaseDeviceToken,
                RouteNo = request.RouteNo,
                Destination = request.Destination,
                ExpectedLeaveTime = request.ExpectedDateTimeLocal,
                DisplayExpectedLeaveTime = request.DisplayExpectedLeaveTime,
                StopNo = request.StopNo,
                NumberofNextBuses = request.NumberofBuses
            }; 

            var payload = JsonConvert.SerializeObject(sendNotificationRequest);
            var triggerTimeUtc = CalculateNotificationTriggerTime(request.ExpectedLeaveTimeUTC, request.ScheduleReminderInMin);
            var result = await _googleCloudTaskApiClient.CreateScheduledNotificationAsync(payload, triggerTimeUtc);

            if (!string.IsNullOrEmpty(result))
                return new CreateNotificationResponse
                {
                    StatusCode = 201,
                    Message = "SUCCESS!"
                };

            return new CreateNotificationResponse
            {
                StatusCode = 500,
                Message = "Could not schedule task, something went wrong!"
            };
        }

        private DateTime CalculateNotificationTriggerTime(DateTime expectedLeaveTimeUtc, int scheduleReminderInMin)
        {
            TimeSpan scheduleReminderInMinOffset = TimeSpan.FromMinutes(scheduleReminderInMin);
            var totalOffSet = scheduleReminderInMinOffset.Add(NotificationProcessingTimeOffSet).Add(NotificationSendTimeOffSet); 
            return expectedLeaveTimeUtc.Subtract(totalOffSet);
        }
    }
}
