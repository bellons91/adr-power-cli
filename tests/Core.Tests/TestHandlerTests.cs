using Handlers;

namespace adrCoreTests;

public class TestsHandlerTests
{
    private Test.TestHandler _sut;

    [SetUp]
    public void Setup() => _sut = new Test.TestHandler();

    [Test]
    public async Task TestMustReturnText()
    {
        var testRequest = new Test.TestRequest
        {
            Name = "Davide"
        };

        var result = await _sut.Handle(testRequest, CancellationToken.None);

        Assert.That(result, Is.EqualTo("test Davide"));
    }
}
