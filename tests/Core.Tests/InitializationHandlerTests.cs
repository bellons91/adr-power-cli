using Castle.Core.Logging;
using Core.Models;
using Handlers;
using Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace adr_core_tests
{
    public class InitializationHandlerTests
    {
        private readonly Mock<IConfigurationService> _mockConfigService;
        private Initialization.InitializationHandler _sut;
        private CancellationToken _cancellationToken;

        public InitializationHandlerTests()
        {
            _mockConfigService = new Mock<IConfigurationService>();
        }

        [SetUp]
        public void Setup()
        {
            _cancellationToken = (new CancellationTokenSource()).Token;
            _mockConfigService.Setup(cs => cs.ConfigExists(_cancellationToken)).ReturnsAsync(false);

            var logger = Mock.Of<ILogger<Initialization.InitializationHandler>>();

            _sut = new Initialization.InitializationHandler(_mockConfigService.Object, logger);
        }


        [Test]
        public void Handler_Should_ThrowException_When_SettingsAreInvalid()
        {
            // Arrange
            var invalidRequest = ValidRequest;
            invalidRequest.Name = "";

            // Act + Assert 
            Assert.CatchAsync<InvalidInitializationException>(async () => await _sut.Handle(invalidRequest, _cancellationToken));
        }


        [Test]
        public void Handler_Should_ThrowException_When_SettingsAlreadyExist()
        {
            // Arrange
            var validRequest = ValidRequest;

            _mockConfigService.Setup(cs => cs.ConfigExists(_cancellationToken)).ReturnsAsync(true);

            // Act + Assert 
            Assert.CatchAsync<InvalidInitializationException>(async () => await _sut.Handle(validRequest, _cancellationToken));
            _mockConfigService.VerifyAll();
        }


        [Test]
        public async Task Handler_Should_CallSettingsInitialization_When_SettingsAreValidAndNotExist()
        {
            // Arrange
            var validRequest = ValidRequest;


            // Act
            await _sut.Handle(validRequest, _cancellationToken);

            // Assert 
            _mockConfigService.Verify(_ => _.InitializeAsync(It.IsAny<AdrSettings>(), _cancellationToken));
        }

        private static Initialization.InitRequest ValidRequest => new Initialization.InitRequest()
        {
            Name = "test",
            AvailableStatuses = new[] { "test" },
            Template = "test"

        };
    }
}