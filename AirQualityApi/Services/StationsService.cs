using AirQualityApi.Models;
using AirQualityApi.Exceptions;

namespace AirQualityApi.Services
{
    public class StationsService : IStationsService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<StationsService> _logger;

        public StationsService(IHttpClientFactory httpClientFactory, ILogger<StationsService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<Station>> GetAllStations()
        {
            _logger.LogInformation("Retrieving the data...");
            var client = _httpClientFactory.CreateClient("stationsclient");
            var apiResponse = await client.GetFromJsonAsync<List<Station>>("station/findAll");

            if (apiResponse.Count == 0)
            {
                _logger.LogError("An error occured while retrieving the data");
                throw new NotFoundException("Couldn't find any stations");
            }

            _logger.LogInformation("Successfully retrieved {NumberOfStations} stations", apiResponse.Count);
            return apiResponse;
        }

        public async Task<List<Sensor>> GetSensorDataById(int stationId)
        {
            _logger.LogInformation("Retrieving the data...");

            var client = _httpClientFactory.CreateClient("stationsclient");
            var apiResponse = await client.GetFromJsonAsync<List<Sensor>>($"station/sensors/{stationId}");

            if(stationId == null)
            {
                _logger.LogError("An argument was null");
                throw new ArgumentNullException("Bad request");
            }
            if (apiResponse.Count == 0)
            {
                _logger.LogError("An error occured while retrieving the data");
                throw new NotFoundException($"Station with given ID: {stationId} was not found");
            }
            
            _logger.LogInformation("Successfully retrieved the station with given ID: {StationID}", stationId);
            return apiResponse;
        }

        public async Task<List<StationWithParams>> GetStationAndSensorDataByStationName(string stationName)
        {
            _logger.LogInformation("Retrieving the data...");
            var client = _httpClientFactory.CreateClient("stationsclient");
            var stationResponse = await client.GetFromJsonAsync<List<Station>>("station/findAll");
            if (stationName == null)
            {
                _logger.LogError("An argument was null");
                throw new ArgumentNullException("Bad request");
            }
            if (stationResponse.Count == 0)
            {
                _logger.LogError("An error occured while retrieving the data");
                throw new NotFoundException($"Station with given name: {stationName} was not found");
            }
            
            List<Station> filteredStations = stationResponse.FindAll(s => s.StationName.Contains(stationName, StringComparison.InvariantCultureIgnoreCase)); 

            List<StationWithParams> stationsWithParams = new List<StationWithParams>();

            foreach (var station in filteredStations)
            {
                var sensorResponse = await client.GetFromJsonAsync<List<Sensor>>($"station/sensors/{station.Id}");

                List<Param> paramsList = new List<Param>();
                foreach (var sensor in sensorResponse)
                {
                    paramsList.Add(new Param
                    {
                        ParamName = sensor.Param.ParamName,
                        ParamFormula = sensor.Param.ParamFormula
                    });
                }

                stationsWithParams.Add(new StationWithParams
                {
                    StationName = station.StationName,
                    StationId = station.Id,
                    Param = paramsList
                });
            }
            _logger.LogInformation("Successfully retrieved the station data with given station name: {StationName}", stationName);
            return stationsWithParams;
        }
    }
}

