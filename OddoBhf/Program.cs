using OddoBhf.Data;
using Microsoft.EntityFrameworkCore;
using OddoBhf.Interfaces;
using OddoBhf.Repositories;
using OddoBhf.Services;
using OddoBhf.Hub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// add DB context
builder.Services.AddDbContext<DataContext>(options => { 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnexion"));
});
// add repositories
builder.Services.AddScoped<ILicenceRepository, LicenceRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// add services
builder.Services.AddScoped<ILicenceService, LicenceService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUserService, UserService>();



builder.Services.AddHttpClient();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//CORS
app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();
//map incoming client requests to the proper Hub and give it the route "Notify"
app.MapHub<NotificationHub>("/Notify");
app.MapControllers();

app.Run();
