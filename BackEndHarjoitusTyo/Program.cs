using BackEndHarjoitusTyo.Middleware;
using BackEndHarjoitusTyo.Models;
using BackEndHarjoitusTyo.Repositories;
using BackEndHarjoitusTyo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<MessageServiceContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MessageDB")));
builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAutehtication", null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Message API",
        Description = "An ASP.NET Core Web API for sending and receiving messages",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddScoped<iMessageRepository, MessageRepository>();
builder.Services.AddScoped<iUserRepository, UserRepository>();
builder.Services.AddScoped<iMessageService, MessageService>();
builder.Services.AddScoped<iUserService, UserService>();
builder.Services.AddScoped<iUserAuthenticationService, UserAuthenticationService>();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    MessageServiceContext dbcontext = scope.ServiceProvider.GetRequiredService<MessageServiceContext>();
    dbcontext.Database.EnsureCreated();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
