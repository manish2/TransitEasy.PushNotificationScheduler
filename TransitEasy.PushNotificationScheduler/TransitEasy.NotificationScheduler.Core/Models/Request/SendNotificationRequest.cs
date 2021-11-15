using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Models.Request
{
    public class SendNotificationRequest
    {
        public DateTime ExpectedLeaveTime { get; set; }
        public int ScheduleReminderInMin { get; set; }
        public string FirebaseDeviceToken { get; set; }
        public string RouteNo { get; set; }
        public string Destination { get; set; }
        public string StopNo { get; set; }
        public int NumberofNextBuses { get; set; }
        public string DisplayExpectedLeaveTime { get; set; }
    }
}
