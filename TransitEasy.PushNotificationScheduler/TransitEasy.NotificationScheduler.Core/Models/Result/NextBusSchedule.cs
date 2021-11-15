using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Models.Result
{
    public class NextBusSchedule
    {
        public bool IsTripCancelled { get; set; }
        public bool IsStopCancelled { get; set; }
        public int CountdownInMin { get; set; }
        public NextBusScheduleStatus ScheduleStatus { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedLeaveTime { get; set; }
    }
}
