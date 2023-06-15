using AirQualityApi.Models;

namespace AirQualityApi.Services
{
    public interface IStationsService
    {
        Task<List<Station>> GetAllStations();
        Task<List<Sensor>> GetSensorDataById(int stationId);
        Task<List<StationWithParams>> GetStationAndSensorDataByStationName(string stationName);
    }
}
