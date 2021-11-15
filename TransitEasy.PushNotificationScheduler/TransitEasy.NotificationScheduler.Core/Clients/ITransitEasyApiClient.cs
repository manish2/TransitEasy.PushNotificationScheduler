using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Models.Result;

namespace TransitEasy.NotificationScheduler.Core.Clients
{
    public interface ITransitEasyApiClient
    {
        Task<NextBusStopInfoResult> GetNextBusSchedules(string stopNumber, int numNextBuses); 
    }
}
