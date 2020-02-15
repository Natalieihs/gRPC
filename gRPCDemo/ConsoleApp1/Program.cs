using Grpc.Core;
using Grpc.Net.Client;
using GrpcService1;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using static GrpcService1.Greeter;

namespace ConsoleApp1
{
    class Program
    {
        /*
         * 客户端需要安装几个包
         Install-Package Grpc.Net.Client
         Install-Package Google.Protobuf
         Install-Package Grpc.Tools
         把服务端的接口复制一份
         修改项目文件为Client
         服务端修改后，客户端同步修改  添加为连接

            客户端和服务端共用一个协议文件，可以把协议文件添加到公共文件夹中
             */
        static async Task Main(string[] args)
        {
            //创建常连接  可以多个通道共享一个长连接
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            ////创建通道
            //var client = new GreeterClient(channel);
            //var replay = await client.SayHelloAsync(new GrpcService1.HelloRequest { Name = "bing111" });
            //Console.WriteLine(replay.Message);
            await ServerStreamingCallExample();
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }


        /// <summary>
        /// 使用依赖注入的方式
        /// </summary>
        /// <returns></returns>
        static async Task CreateClient2()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddGrpcClient<GreeterClient>(options =>
            {
                options.Address = new Uri("https://localhost:5001");
            });
            await Task.Delay(1000);


            var serviceProvider = services.BuildServiceProvider();
            GreeterClient service = serviceProvider.GetRequiredService<GreeterClient>();
            //  service.SayHelloStreaml
           // await ServerStreamingCallExample(service);
        }

        private static async Task ServerStreamingCallExample()
        {


            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GreeterClient(channel);
            var cts = new CancellationTokenSource();

            cts.CancelAfter(TimeSpan.FromSeconds(80));

            using var call = client.SayHelloStream1(new HelloRequest { Name = "零度" }, cancellationToken: cts.Token);

            try
            {
                await foreach (var message in call.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine("Greeting: " + message.Message);
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Stream cancelled.");
            }
        }
    }
}
