using Grpc.Net.Client;
using GrpcService;

HelloRequest message = new() { Name = "MyGRPC" };
GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7057");
Greeter.GreeterClient client = new Greeter.GreeterClient(channel);
HelloReply serverReply = await client.SayHelloAsync(message);
Console.WriteLine(serverReply.Message);

// ----------------------------------------------------------------------------------------

Product.ProductClient productClient = new Product.ProductClient(channel);
GetProductDetail product = new GetProductDetail { ProductId = 3 };
ProductModel productInfo = await productClient.GetProductsInformationAsync(product);
Console.WriteLine($"{productInfo.ProductName} | {productInfo.ProductDescription} | {productInfo.ProductPrice} | {productInfo.ProductStock}");

