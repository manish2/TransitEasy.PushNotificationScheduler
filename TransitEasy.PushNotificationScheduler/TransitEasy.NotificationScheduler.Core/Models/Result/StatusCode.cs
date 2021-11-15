using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Models.Result
{
    public enum StatusCode
    {
        Success = 0,
        NoStopsNearLocation = 1,
        NoVehiclesAvailable = 2,
    }
}
