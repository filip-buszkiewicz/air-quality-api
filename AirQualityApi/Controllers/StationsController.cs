using Microsoft.AspNetCore.Mvc;
using AirQualityApi.Services;

namespace AirQualityApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationsController : ControllerBase
    {
        private readonly IStationsDbService _stationsService;

        public StationsController(IStationsDbService stationsService)
        {
            _stationsService = stationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStations()
        {
            var apiResponse = await _stationsService.GetAllStations();

            if(apiResponse == null) 
            {
                return NotFound();
            }

            return Ok(apiResponse);
        }

        [HttpGet("id/{stationId}")]
        public async Task<IActionResult> GetSensorDataById(int stationId)
        {
            var apiResponse = await _stationsService.GetSensorDataById(stationId);

            if (apiResponse == null)
            {
                return NotFound();
            }

            return Ok(apiResponse);
        }

        //[HttpGet("city/{stationName}")]
        //public async Task<IActionResult> GetStationAndSensorDataByStationName(string stationName)
        //{
        //    var stationResponse = await _stationsService.GetStationAndSensorDataByStationName(stationName);

        //    if (stationResponse == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(stationResponse);
        //}
    }
}
