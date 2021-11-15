using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransitEasy.NotificationScheduler.Core.Models.Request
{
    public class FCMRequest
    {
        public string RegistrationToken { get; set; }
        public string MessageBody { get; set; }
        public string MessageTitle { get; set; }
    }
}
