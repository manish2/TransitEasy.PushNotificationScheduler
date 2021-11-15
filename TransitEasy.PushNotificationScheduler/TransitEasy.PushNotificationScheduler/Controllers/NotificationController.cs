using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Handlers;
using TransitEasy.NotificationScheduler.Core.Models.Request;
using TransitEasy.NotificationScheduler.Core.Models.Result;

namespace TransitEasy.PushNotificationScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly ILogger<NotificationController> _logger;
        private readonly IRequestHandler<SendNotificationRequest, SendNotificationResponse> _requestHandler; 

        public NotificationController(ILogger<NotificationController> logger, IRequestHandler<SendNotificationRequest, SendNotificationResponse> requestHandler)
        {
            _logger = logger;
            _requestHandler = requestHandler; 
        }

        /// <summary>
        /// This is the callback that will be triggered from the google scheduled task
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("/sendnotification")]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> SendNotification([FromBody] SendNotificationRequest request)
        {
            _logger.LogInformation($"Received request to send notification for route {request.RouteNo} for next {request.NumberofNextBuses} buses");
            await _requestHandler.HandleRequest(request);
            return await Task.FromResult(Ok()); 
        }
    }
}
