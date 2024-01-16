using Commands;

namespace CLITests
{
    public class InitializationCommandTests
    {
        private InitializationCommand.InitializationCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new InitializationCommand.InitializationCommandHandler();
        }

        [Test]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase((string)null)]
        public void Reject_Command_When_ProjectNameIsNullOrEmpty(string invalidProjectName)
        {
            // Arrange
            var invalidOption = new InitializationCommand.InitOptions
            {
                ProjectName = invalidProjectName,
            };

            // Act
            var result = _handler.Execute(invalidOption);

            // Assert
            Assert.That(result, Is.EqualTo(0));

        }

        [Test]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase((string)null)]
        public void Reject_Command_When_TemplateIsNullOrEmpty(string invalidTemplate)
        {
            // Arrange
            var invalidOption = new InitializationCommand.InitOptions
            {
                AdrTemplate = invalidTemplate,
                ProjectName = "project",
            };

            // Act
            var result = _handler.Execute(invalidOption);

            // Assert
            Assert.That(result, Is.EqualTo(0));

        }
    }
}