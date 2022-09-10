using Grpc.Net.Client;
using GrpcService;

HelloRequest message = new() { Name = "MyGRPC" };
GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7057");
Greeter.GreeterClient client = new Greeter.GreeterClient(channel);
HelloReply serverReply = await client.SayHelloAsync(message);
Console.WriteLine(serverReply.Message);

// ----------------------------------------------------------------------------------------

ProductsService.ProductsServiceClient productsServiceClient = new ProductsService.ProductsServiceClient(channel);

// --- Create ---
Console.WriteLine("CREATE");
ProductMessage productMessage = new ProductMessage()
{
    ProductName = "Peach",
    CategoryName = "Fruit",
    Manufacturer = "Frutko",
    Price = 100
};
ProductMessage replay = await productsServiceClient.CreateAsync(productMessage);
Print(replay);

// --- Get ---
Console.WriteLine("GET");
ProductIdMessage productIdMessage = new ProductIdMessage() { Id = 2 };
replay = await productsServiceClient.GetByIdAsync(productIdMessage);
Print(replay);

// --- GetAll ---
Console.WriteLine("GET ALL");
ProductsMessage products = await productsServiceClient.GetAllAsync(new());
foreach (var item in products.Items)
{
    Print(item);
}

// --- Edit ---
Console.WriteLine("EDIT");
ProductMessage productMessageEdit = new ProductMessage()
{
    Id = 3,
    ProductName = "Ananas",
    CategoryName = "Fruit",
    Manufacturer = "Frutko",
    Price = 100
};
replay = await productsServiceClient.EditAsync(productMessageEdit);
Print(replay);

// --- Delete ---
Console.WriteLine("DELETE");
ProductIdMessage productIdMessageDelete = new ProductIdMessage() { Id = 6 };
EmptyMessage replayDel = await productsServiceClient.DeleteAsync(productIdMessageDelete);

static void Print(ProductMessage replay)
{
    Console.WriteLine($"{replay.Id} | {replay.ProductName} | {replay.CategoryName} | {replay.Manufacturer} | {replay.Price}");
}
