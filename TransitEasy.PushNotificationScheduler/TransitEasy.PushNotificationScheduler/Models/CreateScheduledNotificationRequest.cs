using System;

namespace TransitEasy.PushNotificationScheduler.Models
{
    public class CreateScheduledNotificationRequest
    {
        public DateTimeOffset ExpectedLeaveTime { get; set; }
        public string DisplayExpectedLeaveTime { get; set; }
        public int ScheduleReminderInMin { get; set; }
        public string FirebaseDeviceToken { get; set; }
        public string RouteNo { get; set; }
        public string Destination { get; set; }
        public string StopNo { get; set; }
        public int NumberOfNextBuses { get; set; }
    }
}
