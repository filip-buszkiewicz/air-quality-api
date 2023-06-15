namespace AirQualityApi.Models
{
    public class Station
    {
        public int Id { get; set; }
        public string? StationName { get; set; }
        public string? GegrLat { get; set; }
        public string? GegrLon { get; set; }
        public City City { get; set; }
        public string? AddressStreet { get; set; }
    }
}
