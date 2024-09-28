using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

// Add JWT Authentication with static token
var secretKey = "ThisIsASecretKeyThatIsVeryLong123456789!@#";

var key = Encoding.ASCII.GetBytes(secretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // Disable validation of issuer
        ValidateAudience = false, // Disable validation of audience
        ValidateLifetime = true, // Enable token expiration validation
        ValidateIssuerSigningKey = true, // Validate the signature
        IssuerSigningKey = new SymmetricSecurityKey(key) // The secret key used to sign the token
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerceBFF API", Version = "v1" });

    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
