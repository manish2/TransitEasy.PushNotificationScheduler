using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Clients;
using TransitEasy.NotificationScheduler.Core.Handlers;
using TransitEasy.NotificationScheduler.Core.Models.Request;
using TransitEasy.NotificationScheduler.Core.Models.Result;
using Xunit;

namespace TransitEasy.Scheduler.Core.Tests
{
    public class SchedulerRequestHandlerTests
    {
        private readonly Mock<IGoogleCloudTaskApiClient> _googleCloudTaskApiClientMock;
        private readonly Mock<ILogger<SchedulerRequestHandler>> _schedulerRequestHandlerLoggerMock;
        private readonly IRequestHandler<CreateNotificationRequest, CreateNotificationResponse> _sut;

        public SchedulerRequestHandlerTests()
        {
            _googleCloudTaskApiClientMock = new Mock<IGoogleCloudTaskApiClient>();
            _schedulerRequestHandlerLoggerMock = new Mock<ILogger<SchedulerRequestHandler>>();
            _sut = new SchedulerRequestHandler(_googleCloudTaskApiClientMock.Object, _schedulerRequestHandlerLoggerMock.Object);
        }

        [Fact]
        public async Task GivenValidNotificationRequest_WhenHandle_ThenScheduleNotification()
        {
            //Given
            var mockRequest = new CreateNotificationRequest
            {
                ExpectedDateTimeLocal = DateTime.Now,
                ExpectedLeaveTimeUTC = DateTime.UtcNow,
                FirebaseDeviceToken = "abc",
                RouteNo = "335",
                ScheduleReminderInMin = 5,
                NumberofBuses = 2
            };
            var expectedResponseCode = 201;
            var expectedMessage = "SUCCESS!";
            _googleCloudTaskApiClientMock
                .Setup(tc => tc.CreateScheduledNotificationAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync("Success!");

            //When
            var result = await _sut.HandleRequest(mockRequest);

            //Then
            result.StatusCode.Should().Be(expectedResponseCode);
            result.Message.Should().Be(expectedMessage);
        }

        [Fact]
        public async Task GivenValidNotificationRequest_WhenHandleFails_ThenReturnStatusFailed()
        {
            //Given
            var mockRequest = new CreateNotificationRequest
            {
                ExpectedDateTimeLocal = DateTime.Now,
                ExpectedLeaveTimeUTC = DateTime.UtcNow,
                FirebaseDeviceToken = "abc",
                RouteNo = "335",
                ScheduleReminderInMin = 5,
                NumberofBuses = 2
            };
            var expectedResponseCode = 500;
            var expectedMessage = "Could not schedule task, something went wrong!";
            _googleCloudTaskApiClientMock
                .Setup(tc => tc.CreateScheduledNotificationAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync("");

            //When
            var result = await _sut.HandleRequest(mockRequest);

            //Then
            result.StatusCode.Should().Be(expectedResponseCode);
            result.Message.Should().Be(expectedMessage);
        }
    }
}
