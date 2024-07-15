using Asp.Versioning;
using ecommerse_api.Common.Infastracture;
using ecommerse_api.Common.Logger;
using ecommerse_api.Common.Middleware;
using ecommerse_api.Features.CartItems.Repository;
using ecommerse_api.Features.Orders.Repository;
using ecommerse_api.Features.Users.Repository;
using ecommerse_api.OpenApi;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Set up Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Configure Swagger for Authorization
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("x-user-id", new OpenApiSecurityScheme
    {
        Description = "User ID header",
        In = ParameterLocation.Header,
        Name = "x-user-id",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "x-user-id"
                }
            },
            new string[] {}
        }
    });

    options.OperationFilter<UserIdHeaderParameterOperationFilter>();
});

var assembly = typeof(Program).Assembly;
string MysqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

// Register LoggingInterceptor
builder.Services.AddScoped<LoggingInterceptor>();

// DB Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(MysqlConnectionString,
        new MySqlServerVersion(new Version(8, 0, 36)))
        .LogTo(message => Log.Debug(message), LogLevel.Information);
});
ApplicationDbContext.GlobalDbConnection.MysqlConnection = MysqlConnectionString;

// Additional service registrations
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

// API Versioning
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 2);
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API V1",
        Description = "API V1 Description"
    });

    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "API V2",
        Description = "API V2 Description"
    });

    c.ResolveConflictingActions(a => a.First());
    c.OperationFilter<RemoveVesionFromParameter>();
    c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", "API V1");
        c.SwaggerEndpoint($"/swagger/v2/swagger.json", "API V2");
    });
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

// Authorize
app.UseMiddleware<UserIdHeaderAuthenticationMiddleware>("Test");
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
