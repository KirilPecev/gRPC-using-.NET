using Grpc.Net.Client;
using GrpcService;

HelloRequest message = new() { Name = "MyGRPC" };
GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7057");
Greeter.GreeterClient client = new Greeter.GreeterClient(channel);
HelloReply serverReply = await client.SayHelloAsync(message);
Console.WriteLine(serverReply.Message);

