using Microsoft.EntityFrameworkCore;
using UserAuthApi.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Database Service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add CORS (Allows Angular to talk to this API)
builder.Services.AddCors(options => {
    options.AddPolicy("AngularPolicy", policy => {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// ... (rest of existing code)

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AngularPolicy"); // This MUST be before app.UseAuthorization()

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
