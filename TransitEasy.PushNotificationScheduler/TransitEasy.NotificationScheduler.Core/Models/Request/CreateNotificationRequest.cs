using System;

namespace TransitEasy.NotificationScheduler.Core.Models.Request
{
    public class CreateNotificationRequest
    {
        public DateTime ExpectedDateTimeLocal { get; set; }
        public DateTime ExpectedLeaveTimeUTC { get; set; }
        public int ScheduleReminderInMin { get; set; }
        public string FirebaseDeviceToken { get; set; }
        public string RouteNo { get; set; }
        public string Destination { get; set; }
        public string StopNo { get; set; }
        public int NumberofBuses { get; set; }
        public string DisplayExpectedLeaveTime { get; set; }
    }
}
