using Google.Cloud.Tasks.V2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Options;
using GoogleTask = Google.Cloud.Tasks.V2.Task; 

namespace TransitEasy.NotificationScheduler.Core.Clients
{
    public class GoogleCloudTaskApiClient : IGoogleCloudTaskApiClient
    {
        private readonly IOptions<ApplicationOptions> _options; 
        public GoogleCloudTaskApiClient(IOptions<ApplicationOptions> options)
        {
            _options = options; 
        }

        public async Task<string> CreateScheduledNotificationAsync(string payload, DateTime sendTimeInUtc)
        {
            CloudTasksClient cloudTasksClient = await CloudTasksClient.CreateAsync();
            QueueName queueName = new(_options.Value.GoogleCloudSettings.ProjectId, _options.Value.GoogleCloudSettings.QueueLocationId, _options.Value.GoogleCloudSettings.QueueId);
            var httpRequest = new HttpRequest
            {
                HttpMethod = HttpMethod.Post,
                Url = _options.Value.SendNotificationCallbackUrl,
                Body = ByteString.CopyFromUtf8(payload)
            };
            httpRequest.Headers.Add("Content-Type", "application/json");

            var response = await cloudTasksClient.CreateTaskAsync(new CreateTaskRequest
            {
                Parent = queueName.ToString(),
                Task = new GoogleTask
                {
                    HttpRequest = httpRequest,
                    ScheduleTime = Timestamp.FromDateTime(sendTimeInUtc)
                }
            });
            return response.TaskName.TaskId;
        }
    }
}
