using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService1
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public async override Task SayHelloStream1(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var counter = 0;

            while (!context.CancellationToken.IsCancellationRequested)
            {
                var message = $"How are you {request.Name}? {++counter}";

                _logger.LogInformation($"Sending greeting {message}.");

                await responseStream.WriteAsync(new HelloReply { Message = message });

                // Gotta look busy
                await Task.Delay(1000);
            }
        }

    }
}
