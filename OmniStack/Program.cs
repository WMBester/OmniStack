using WMB.Api.Services;
using Microsoft.EntityFrameworkCore;
using WMB.Api.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the ApplicationDbContext with the dependency injection container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the CrudService with the dependency injection container
builder.Services.AddScoped<ICrudService, CrudService>();

// Register the OpenAIService with the dependency injection container
builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();

var app = builder.Build();
app.MapControllers();  // Maps the routes defined in your controllers

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WMB.Api v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
