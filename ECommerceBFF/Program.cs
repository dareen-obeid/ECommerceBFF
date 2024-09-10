using ECommerceBFF.ServiceClient;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure HttpClients
builder.Services.AddHttpClient("ProductServiceClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7124/");
});

builder.Services.AddHttpClient("CartServiceClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7218/");
});

builder.Services.AddHttpClient("OrderServiceClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7298/");
});


builder.Services.AddScoped<ProductServiceClient>();
builder.Services.AddScoped<OrderServiceClient>();
builder.Services.AddScoped<CartServiceClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

