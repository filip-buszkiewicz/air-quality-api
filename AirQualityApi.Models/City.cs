using System.ComponentModel.DataAnnotations.Schema;

namespace AirQualityApi.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
