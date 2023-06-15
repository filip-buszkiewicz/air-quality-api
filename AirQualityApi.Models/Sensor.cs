namespace AirQualityApi.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public int StationId { get; set; }  
        public SensorParams Param { get; set; }  
    }
}
