using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Handlers;
using TransitEasy.NotificationScheduler.Core.Models.Request;
using TransitEasy.NotificationScheduler.Core.Models.Result;
using TransitEasy.PushNotificationScheduler.Models;

namespace TransitEasy.PushNotificationScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly IRequestHandler<CreateNotificationRequest, CreateNotificationResponse> _requestHandler;

        private readonly ILogger<SchedulerController> _logger;

        public SchedulerController(IRequestHandler<CreateNotificationRequest, CreateNotificationResponse> requestHandler, ILogger<SchedulerController> logger)
        {
            _requestHandler = requestHandler;
            _logger = logger; 
        }

        [HttpPost("/createschedulednotification")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateScheduledNotificationResponse>> CreateScheduledNotification([FromBody] CreateScheduledNotificationRequest request)
        {
            _logger.LogInformation("Received request to schedule notification");
            if (string.IsNullOrEmpty(request.FirebaseDeviceToken))
            {
                return Problem(detail: "No Firebase token specified", title: "An error occurred", statusCode: 400);
            }
            var mappedRequest = new CreateNotificationRequest {
                ExpectedDateTimeLocal = request.ExpectedLeaveTime.DateTime,
                ExpectedLeaveTimeUTC = request.ExpectedLeaveTime.UtcDateTime,
                ScheduleReminderInMin = request.ScheduleReminderInMin,
                FirebaseDeviceToken = request.FirebaseDeviceToken,
                RouteNo = request.RouteNo, 
                Destination = request.Destination,
                StopNo = request.StopNo,
                NumberofBuses = request.NumberOfNextBuses,
                DisplayExpectedLeaveTime = request.DisplayExpectedLeaveTime
            };

            var result = await _requestHandler.HandleRequest(mappedRequest);

            if (result.StatusCode == 201)
            {
                return CreatedAtAction("createschedulednotification", new CreateScheduledNotificationResponse { Message = result.Message });
            }
            else
            {
                return Problem(); 
            }
        }
    }
}
