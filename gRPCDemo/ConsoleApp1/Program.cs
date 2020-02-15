using Grpc.Net.Client;
using System;
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
             */
        static async Task Main(string[] args)
        {
            //创建通道
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //创建客户端
            var client = new GreeterClient(channel);
            var replay = await client.SayHelloAsync(new GrpcService1.HelloRequest { Name = "bing" });
            Console.WriteLine(replay.Message);
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
