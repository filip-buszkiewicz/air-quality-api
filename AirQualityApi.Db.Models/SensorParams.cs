using System.ComponentModel.DataAnnotations;

namespace AirQualityApi.Db.Models
{
    public class SensorParams
    {
        [Key]
        public int IdParam { get; set; }
        public string ParamName { get; set; }
        public string ParamCode { get; set; }
        public string ParamFormula { get; set; }
    }
}
