using System.Text.Json.Serialization;

namespace AirQualityApi.Db.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public int StationId { get; set; }  
        public SensorParams Param { get; set; }

        [JsonIgnore]
        public Station Station { get; set; }
    }
}
