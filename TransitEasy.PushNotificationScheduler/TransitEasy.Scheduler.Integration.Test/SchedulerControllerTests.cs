using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Models.Request;
using TransitEasy.PushNotificationScheduler.Models;
using Xunit;

namespace TransitEasy.Scheduler.Integration.Test
{
    public class SchedulerControllerTests : TestsBase
    {
        private readonly HttpClient _client;
        public SchedulerControllerTests()
        {
            _client = Server.CreateClient();
        }

        [Fact]
        public async Task GivenValidScheduleRequest_WhenCreateScheduleNotificationCalled_ThenScheduleNotification()
        {
            //Given
            var requestData = new CreateScheduledNotificationRequest
            {
                ExpectedLeaveTime = DateTime.UtcNow,
                DisplayExpectedLeaveTime = DateTime.Now.ToString(),
                FirebaseDeviceToken = "ABC",
                RouteNo = "335",
                Destination = "SURREY CTRL",
                StopNo = "201245",
                NumberOfNextBuses = 2
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");

            //When
            var result = await _client.PostAsync("/createschedulednotification", httpContent);

            //Then
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }
    }
}
