using AirQualityApi.Controllers;
using AirQualityApi.Models;
using AirQualityApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NSubstitute.ExceptionExtensions;

namespace AirQualityApi.Tests.Controllers
{
    [TestClass]
    public class StationsControllerTests
    {
        private const int _stationId = 1;
        private const string _stationName = "Station";

        private StationsController _sutController;
        private IStationsService _sutService;
        private List<Station> _fakeStations;
        private List<Sensor> _fakeSensors;
        private List<StationWithParams> _fakeStationWithParams;

        [TestInitialize]
        public void Setup()
        {
            _sutService = Substitute.For<IStationsService>();
            _sutController = new StationsController(_sutService);
            _fakeStations = new List<Station>()
                {
                new Station()
                {
                    Id = 1,
                    StationName = "Station 1",
                    GegrLat = "123.456",
                    GegrLon = "123.456",
                    City = new City() { Name = "City 1" },
                    AddressStreet = "Street 1"
                },

                new Station()
                {
                    Id = 2,
                    StationName = "Station 2",
                    GegrLat = "657.89",
                    GegrLon = "657.89",
                    City = new City() { Name = "City 2" },
                    AddressStreet = "Street 2"
                }
             };
            _fakeSensors = new List<Sensor>()
                {
                new Sensor()
                {
                    Id = 1,
                    StationId = 1,
                    Param = new SensorParams() {ParamName = "Param 1", ParamFormula = "Formula 1"}
                },
                new Sensor()
                {
                    Id = 2,
                    StationId = 2,
                    Param = new SensorParams() {ParamName = "Param 2"}
                }
            };
            _fakeStationWithParams = new List<StationWithParams>()
            {
                new StationWithParams()
                {
                    StationName = "Station 1",
                    StationId = 1,
                    Param = new List<Param>
                    {
                        new Param {ParamName = "Param 1", ParamFormula = "Formula 1"}
                    } 
                }
            }; 
        }

        [TestMethod]
        public async Task GetAllStations_WhenCalled_ListOfStationsIsReturned()
        {
            //Arrange
            _sutService.GetAllStations().Returns(_fakeStations);

            //Act
            var result = await _sutController.GetAllStations();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<List<Station>>();

            var listResult = (List<Station>)okResult.Value;
            listResult.Should().HaveCount(2);
            listResult.First().Id.Should().Be(1);
            listResult.First().StationName.Should().Be("Station 1");

            _sutService.Received(1).GetAllStations();
        }

        [TestMethod]
        public async Task GetSensorDataById_ValidIdPassed_ReturnsSensorDataById()
        {
            //Arrange
            _sutService.GetSensorDataById(_stationId).Returns(_fakeSensors.Where(s => s.StationId == _stationId).ToList());

            //Act
            var result = await _sutController.GetSensorDataById(_stationId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<List<Sensor>>();

            var listResult = okResult.Value as List<Sensor>;
            listResult.Should().HaveCount(1);
            listResult[0].StationId.Should().Be(_stationId);

            _sutService.Received(1).GetSensorDataById(_stationId);
        }

        [TestMethod]
        public async Task GetAllStations_WhenServiceReturnsNull_ReturnsNotFound()
        {
            //Arrange
            _sutService.GetAllStations().ReturnsNull();

            //Act
            var result = await _sutController.GetAllStations();

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            _sutService.Received(1).GetAllStations();
        }

        [TestMethod]
        public async Task GetSensorDataById_WhenServiceReturnsNull_ReturnsNotFound()
        {
            //Arrange
            _sutService.GetSensorDataById(1).ReturnsNull();

            //Act
            var result = await _sutController.GetSensorDataById(1);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            _sutService.Received(1).GetSensorDataById(1);
        }

        [TestMethod]
        public async Task GetSensorDataById_WhenServiceThrowsException_ControllerThrowsException()
        {
            //Arrange
            _sutService.GetSensorDataById(_stationId).Throws<Exception>();

            //Act
            //var result = await _sutController.GetSensorDataById(stationId);
            Func<Task> result = async () => await _sutController.GetSensorDataById(_stationId);

            //Assert
            //result.Should().BeOfType<NotFoundResult>();
            await result.Should().ThrowAsync<Exception>();
            _sutService.Received(1).GetSensorDataById(1);
        }

        [TestMethod]
        public async Task GetStationAndSensorDataByStationName_ValidStationNamePassed_ReturnsListOfStationAndSensorData()
        {
            //Arrange
            _sutService.GetStationAndSensorDataByStationName(_stationName).Returns(_fakeStationWithParams);

            //Act
            var result = await _sutController.GetStationAndSensorDataByStationName(_stationName);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<List<StationWithParams>>();

            var listResult = (List<StationWithParams>)okResult.Value;
            listResult.Should().HaveCount(1);
            listResult.First().StationId.Should().Be(1);
            listResult.First().StationName.Should().Be("Station 1");

            _sutService.Received(1).GetStationAndSensorDataByStationName(_stationName);
        }

        [TestMethod]
        public async Task GetStationAndSensorDataByStationName_WhenServiceReturnsNull_ReturnsNotFound()
        {
            //Arrange
            _sutService.GetStationAndSensorDataByStationName(_stationName).ReturnsNull();

            //Act
            var result = await _sutController.GetStationAndSensorDataByStationName(_stationName);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            _sutService.Received(1).GetStationAndSensorDataByStationName(_stationName);
        }
    }
}
