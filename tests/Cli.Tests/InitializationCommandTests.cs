using Commands;
using Handlers;
using MediatR;
using Moq;
using System.Diagnostics.CodeAnalysis;
using static Handlers.Initialization;

namespace CLITests
{


    public class InitializationCommandTests
    {
        private InitializationCommand.InitializationCommandHandler _handler;
        private readonly Mock<ISender> _mockSender;


        public InitializationCommandTests()
        {
            _mockSender = new Mock<ISender>();
        }
        [SetUp]
        public void Setup()
        {
            _mockSender.Reset();
            _handler = new InitializationCommand.InitializationCommandHandler(_mockSender.Object);
        }

        [Test]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase((string)null)]
        public async Task Reject_Command_When_ProjectNameIsNullOrEmpty(string invalidProjectName)
        {
            // Arrange
            var invalidOption = new InitializationCommand.InitOptions
            {
                ProjectName = invalidProjectName,
            };

            // Act
            var result = await _handler.Execute(invalidOption);

            // Assert
            _mockSender.VerifyNoOtherCalls();
            Assert.That(result, Is.EqualTo(0));

        }

        [Test]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase((string)null)]
        public async Task Reject_Command_When_TemplateIsNullOrEmptyAsync(string invalidTemplate)
        {
            // Arrange
            var invalidOption = new InitializationCommand.InitOptions
            {
                AdrTemplate = invalidTemplate,
                ProjectName = "project",
            };

            // Act
            var result = await _handler.Execute(invalidOption);

            // Assert
            _mockSender.VerifyNoOtherCalls();
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task SendInitializationCommand()
        {

            // Arrange
            var validOptions = new InitializationCommand.InitOptions
            {
                AdrTemplate = "MyTemplate",
                AvailableStatuses = new string[] { "FirstStatus" },
                ProjectName = "MyProject"
            };

            InitRequest actual = null;
            _mockSender.Setup(_ => _.Send(It.IsAny<InitRequest>(), CancellationToken.None))
                .Callback(new InvocationAction(i => actual = (InitRequest)i.Arguments[0]));

            InitRequest expected = new InitRequest
            {
                Name = "MyProject",
                Template = "MyTemplate",
                AvailableStatuses = new string[] { "FirstStatus" }
            };

            // Act
            var result = await _handler.Execute(validOptions);

            // Assert
            Assert.That(result, Is.EqualTo(1));
            Assert.That(actual, Is.EqualTo(expected).Using(new InitRequestComparer()));
        }

        class InitRequestComparer : IEqualityComparer<InitRequest>
        {
            public bool Equals(InitRequest? x, InitRequest? y)
            {
                return string.Equals(x.Name, y.Name)
                     &&
                     string.Equals(x.Template, y.Template)
                     && Enumerable.SequenceEqual(x.AvailableStatuses, y.AvailableStatuses)
                     ;
            }

            public int GetHashCode([DisallowNull] InitRequest obj)
            {
                return obj.Name.GetHashCode() ^ obj.Template.GetHashCode();
            }
        }
    }
}