using AirQualityApi.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace AirQualityApi.Db
{
    public class AirQualityDbContext : DbContext
    {
        public AirQualityDbContext(DbContextOptions<AirQualityDbContext> options) : base(options) 
        {
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<SensorParams> SensorParams { get; set; }
    }
}
