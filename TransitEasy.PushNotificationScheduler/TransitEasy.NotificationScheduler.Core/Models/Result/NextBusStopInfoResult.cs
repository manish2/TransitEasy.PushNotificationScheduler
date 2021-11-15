using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Models.Result
{
    public class NextBusStopInfoResult
    {
        public IEnumerable<NextBusStopInfo> NextBusStopInfo { get; set; }
        public StatusCode ResponseStatus { get; set; }
    }
}
