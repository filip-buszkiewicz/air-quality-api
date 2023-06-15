using AirQualityApi.Db;
using AirQualityApi.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace AirQualityApi.Services
{
    public class StationsDbService : IStationsDbService
    {
        private readonly AirQualityDbContext _dbContext;

        public StationsDbService(AirQualityDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<List<Station>> GetAllStations()
        {
            var stations = await _dbContext.Stations.Include(s => s.City).ToListAsync();
            return stations;
        }
        public async Task<List<Sensor>> GetSensorDataById(int stationId)
        {
            var sensors = await _dbContext.Sensors.Where(s => s.StationId == stationId).Include(s => s.Param).ToListAsync();
            return sensors; 
        }
    }
}
