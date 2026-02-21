using CloudNative.ConfigLibrary;
using CloudNative.Customer.Api.Middleware;
using CloudNative.Customer.Application;
using CloudNative.Customer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

string CorsAll = "AllowAll";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsAll, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddConfigLibaray()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(CorsAll);
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
