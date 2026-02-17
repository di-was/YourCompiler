using YourCompiler.Domain;
using YourCompiler.Infrastructure;
using YourCompiler.Infrastructure.Interfaces;
using YourCompiler.Application;

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

// allow cors from frontend 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNext", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseCors("AllowNext");


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
