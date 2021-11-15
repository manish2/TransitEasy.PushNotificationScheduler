using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Options
{
    public class GoogleCloudOptions
    {
        public string ProjectId { get; set; }
        public string QueueLocationId { get; set; }
        public string QueueId { get; set; }
    }
}
