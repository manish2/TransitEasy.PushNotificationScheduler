using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Models.Result
{
    public class NextBusStopInfo
    {
        public string RouteDescription { get; set; }
        public string Direction { get; set; }
        public IEnumerable<NextBusSchedule> Schedules { get; set; }
    }
}
