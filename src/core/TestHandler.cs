using MediatR;

namespace Core
{
    public class TestOperation
    {
        public class TestRequest : IRequest<string>
        {
            public string Name { get; set; }
        }

        public class TestHandler : IRequestHandler<TestRequest, string>
        {
            public Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
            {
                return Task.FromResult($"test {request.Name}");
            }
        }
    }
}
