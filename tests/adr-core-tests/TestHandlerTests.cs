using Core;

namespace adr_core_tests
{
    public class TestsHandlerTests
    {
        private TestOperation.TestHandler _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new TestOperation.TestHandler();

        }

        [Test]
        public async Task TestMustReturnText()
        {
            var testRequest = new TestOperation.TestRequest();
            testRequest.Name = "Davide";

            var result = await _sut.Handle(testRequest, CancellationToken.None);

            Assert.That(result, Is.EqualTo("test Davide"));
        }
    }
}