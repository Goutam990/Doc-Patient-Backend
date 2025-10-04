using Doc_Patient_Backend.Models;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS - Allow frontend (Angular/React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // frontend port
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add DbContexts
builder.Services.AddDbContext<EnquiryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EnquiryCon")));

builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppointmentCon")));

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
