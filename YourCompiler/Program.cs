using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using YourCompiler.Application;
using YourCompiler.Application.Interfaces;
using YourCompiler.Domain;
using YourCompiler.Infrastructure;
using YourCompiler.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
 builder.Configuration.AddJsonFile("languages.json", optional: true, reloadOnChange: true);


// Add services to the container.
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILanguageDetailsRegistry,LanguageDetailsRegistry>();
builder.Services.AddScoped<IDockerService, DockerService>();

// Add Factory services
builder.Services.AddScoped<CompilerFactory>();

// Add Compiler
builder.Services.AddKeyedScoped<ICompiler, PythonCompiler>("python");
builder.Services.AddKeyedScoped<ICompiler, JavaScriptCompiler>("javascript");
builder.Services.AddKeyedScoped<ICompiler, CSharpCompiler>("csharp");
builder.Services.AddKeyedScoped<ICompiler, CCompiler>("c");
builder.Services.AddKeyedScoped<ICompiler, CppCompiler>("cpp");

// allow cors from frontend 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// configure Rate Limiter

builder.Services.AddRateLimiter(options =>
{
    // compile limiter
    options.AddPolicy("Compile", context => {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                AutoReplenishment = true
            });

    });

    // global limiter
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 100,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();
app.UseCors("AllowAll");


// use rate limiter
app.UseRateLimiter();   


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>     {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
