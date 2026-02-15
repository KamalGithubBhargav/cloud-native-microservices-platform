using CloudNative.Identity.Api.Middlewares;
using CloudNative.Identity.Application;
using CloudNative.Identity.Infrastructure;

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
    .AddApplication()
    .AddInfrastructure(builder.Configuration);


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
