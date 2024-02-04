using MediatR;

namespace Handlers;

public class Test
{
    public class TestRequest : IRequest<string>
    {
        public string Name { get; set; }
    }

    public class TestHandler : IRequestHandler<TestRequest, string>
    {
        public Task<string> Handle(TestRequest request, CancellationToken cancellationToken) => Task.FromResult($"test {request.Name}");
    }
}
