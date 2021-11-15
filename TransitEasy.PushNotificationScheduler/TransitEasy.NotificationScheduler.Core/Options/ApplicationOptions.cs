using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Options
{
    public class ApplicationOptions
    {
        public GoogleCloudOptions GoogleCloudSettings { get; set; }
        public string SendNotificationCallbackUrl { get; set; }
        public string TransitEasyApiBaseUrl { get; set; }
        public int TransitEasyApiTimeoutInSec { get; set; }
    }
}
