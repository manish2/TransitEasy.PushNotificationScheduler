using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Models.Request;
using Xunit;

namespace TransitEasy.Scheduler.Integration.Test
{
    public class NotificationControllerTests : TestsBase
    {
        private readonly HttpClient _client;
        public NotificationControllerTests()
        {
            _client = Server.CreateClient();
        }

        //[Fact]
        //public async Task GivenValidNotificationRequest_WhenSendApiCalled_ThenSendNotification()
        //{
        //    //Given
        //    var requestData = new SendNotificationRequest
        //    {
        //        ExpectedLeaveTime = DateTime.UtcNow, 
        //        DisplayExpectedLeaveTime = DateTime.Now.ToString(),
        //        FirebaseDeviceToken = "ABC",
        //        RouteNo = "335",
        //        Destination = "SURREY CTRL",
        //        NumberofNextBuses = 2,
        //        StopNo = "201245"
        //    };
        //    var httpContent = new StringContent(JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");

        //    //When
        //    var result = await _client.PostAsync("/sendnotification", httpContent);

        //    //Then
        //    result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        //}
    }
}
