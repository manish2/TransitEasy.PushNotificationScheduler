using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using TransitEasy.NotificationScheduler.Core.Extensions;
using TransitEasy.NotificationScheduler.Core.Models.Result;
using TransitEasy.NotificationScheduler.Core.Options;

namespace TransitEasy.NotificationScheduler.Core.Clients
{
    public class TransitEasyApiClient : ITransitEasyApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<ApplicationOptions> _applicationOptions; 

        public TransitEasyApiClient(IHttpClientFactory httpClientFactory, IOptions<ApplicationOptions> applicationOptions)
        {
            _httpClientFactory = httpClientFactory;
            _applicationOptions = applicationOptions;
        }

        public async Task<NextBusStopInfoResult> GetNextBusSchedules(string stopNumber, int numNextBuses)
        {
            var apiClient = _httpClientFactory.CreateClient("TransitEasyApiClient");
            var path = _applicationOptions.Value.TransitEasyApiBaseUrl + $"/api/Stops/getnextbusschedules?stopNumber={stopNumber}&numNextBuses={numNextBuses}";
            return await apiClient.GetFromJsonAsync<NextBusStopInfoResult>(path);
        }
    }
}
