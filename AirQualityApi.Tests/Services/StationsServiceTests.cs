using AirQualityApi.Exceptions;
using AirQualityApi.Models;
using AirQualityApi.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;

namespace AirQualityApi.Tests.Services
{
    [TestClass]
    public class StationsServiceTests
    {
        private const int _stationId = 1;
        private const string _stationName = "Station 1";

        private IHttpClientFactory _httpClientFactory;
        private ILogger<StationsService> _logger;
        private HttpClient _httpClient;
        private StationsService _sutService;
        private MockHttpMessageHandler _mockHttp;
        private List<Station> _fakeStations;
        private List<Sensor> _fakeSensors;
        private List<StationWithParams> _fakeStationWithParams;

        [TestInitialize]
        public void Setup()
        {
            _mockHttp = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHttp)
            {
                BaseAddress = new Uri("https://api.gios.gov.pl/pjp-api/rest/")
            };
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _httpClientFactory.CreateClient(Arg.Any<string>()).Returns(_httpClient);

            _logger = Substitute.For<ILogger<StationsService>>();

            _sutService = new StationsService(_httpClientFactory, _logger);

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

            _fakeSensors = new List<Sensor>
            {
                new Sensor {Id = 1, StationId = 1, Param = new SensorParams() }
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
        public async Task GetAllStations_WhenCalled_ReturnsAllStations()
        {
            //Arrange
            _mockHttp.When($"https://api.gios.gov.pl/pjp-api/rest/station/findAll").Respond(HttpStatusCode.OK, JsonContent.Create(_fakeStations));

            //Act
            var result = await _sutService.GetAllStations();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Station>>();
            result.Should().BeEquivalentTo(_fakeStations);
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(1);
            result.First().StationName.Should().Be("Station 1");
        }

        [TestMethod]
        public async Task GetSensorDataById_WhenValidIdIsPassed_ReturnSensorData()
        {
            //Arrange
            _mockHttp.When($"https://api.gios.gov.pl/pjp-api/rest/station/sensors/{_stationId}").Respond(HttpStatusCode.OK, JsonContent.Create(_fakeSensors));

            //Act
            var result = await _sutService.GetSensorDataById(_stationId);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(_fakeSensors);
            result.Should().HaveCount(1);
            result.First().Id.Should().Be(1);
            result.First().StationId.Should().Be(1);
            result.Should().BeOfType<List<Sensor>>();
        }

        [TestMethod]
       public async Task GetSensorDataById_WhenEmptyListIsReturned_ShouldThrowNotFoundException()
        {
            //Arrange
            var invalidStationId = 999;
            _mockHttp.When($"https://api.gios.gov.pl/pjp-api/rest/station/sensors/{invalidStationId}").Respond(JsonContent.Create(new List<Sensor>()));

            //Act
            Func<Task> result = async () => await _sutService.GetSensorDataById(invalidStationId);

            //Assert
            await result.Should().ThrowAsync<NotFoundException>().WithMessage($"Station with given ID: {invalidStationId} was not found");
        }

        [TestMethod]
        public async Task GetStationAndSensorDataByStationName_WhenValidStationNamePassed_ReturnsStationAndSensorData()
        {
            //Arrange
            var stationData = new List<Station>()
            {
                new Station()
                {
                    Id = 1,
                    StationName = "Station 1"
                }
            };

            var paramData = new List<Sensor>()
            {
                new Sensor()
                {
                    Id = 1,
                    StationId = 1,
                    Param = new SensorParams() {ParamName = "Param 1", ParamFormula = "Formula 1"}
                }
            };
        
            _mockHttp.When("https://api.gios.gov.pl/pjp-api/rest/station/findAll").Respond(HttpStatusCode.OK, JsonContent.Create(stationData));
            _mockHttp.When($"https://api.gios.gov.pl/pjp-api/rest/station/sensors/{_stationId}").Respond(HttpStatusCode.OK, JsonContent.Create(paramData));

            //Act
            var result = await _sutService.GetStationAndSensorDataByStationName(_stationName);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(_fakeStationWithParams);
            result.Should().HaveCount(1);
            result.First().StationName.Should().Be("Station 1");
            result.First().StationId.Should().Be(1);
            result.Should().BeOfType<List<StationWithParams>>();

        }

        [TestMethod]
        public async Task GetStationAndSensorDataByStationName_WhenEmptyListIsReturned_ShouldThrowNotFoundException()
        {
            //Arrange
            _mockHttp.When("https://api.gios.gov.pl/pjp-api/rest/station/findAll").Respond(JsonContent.Create(new List<Station>()));
            _mockHttp.When($"https://api.gios.gov.pl/pjp-api/rest/station/sensors/{_stationId}").Respond(JsonContent.Create(new List<Sensor>()));

            //Act
            Func<Task> result = async () => await _sutService.GetStationAndSensorDataByStationName(_stationName);

            //Assert
            await result.Should().ThrowAsync<NotFoundException>().WithMessage($"Station with given name: {_stationName} was not found");
        }
    }
}
