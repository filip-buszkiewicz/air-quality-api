using System.ComponentModel.DataAnnotations.Schema;

namespace AirQualityApi.Db.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
