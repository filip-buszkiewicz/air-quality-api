using AirQualityApi.Db.Models;
namespace AirQualityApi.Services
{
    public interface IStationsDbService
    {
        Task<List<Station>> GetAllStations();
        Task<List<Sensor>> GetSensorDataById(int stationId);
        
    }
}
