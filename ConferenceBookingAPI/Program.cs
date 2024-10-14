using ConferenceBookingAPI.Model;
using ConferenceBookingAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// for cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200") // Replace with your frontend URL
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddDbContext<ConferenceBookingContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultCon")));
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IConferenceService, ConferenceService>();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
