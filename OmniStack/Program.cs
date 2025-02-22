using WMB.Api.Services;
using WMB.Api.DbContext;
using FluentValidation;
using FluentValidation.AspNetCore;
using WMB.Api.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Add FluentValidation services
builder.Services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the ApplicationDbContext with the dependency injection container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the CrudService with the dependency injection container
builder.Services.AddScoped<ICrudService, CrudService>();

// Register the OpenAIService with the dependency injection container
builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();

// Add health check services
builder.Services.AddHealthChecks();

var app = builder.Build();

// Apply migrations to ensure the database schema is up to date
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();  // This will apply any pending migrations
}

app.MapControllers();  // Maps the routes defined in your controllers

// Map the health check endpoint
app.MapHealthChecks("/health");

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

// Listen on 0.0.0.0:80 to be accessible from outside the container
app.Run("http://0.0.0.0:80");
