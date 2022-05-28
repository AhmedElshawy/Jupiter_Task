using Jupiter_Task.Data;
using Jupiter_Task.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add DbContext
builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlServer("Server=.;Database=jupiter_task;Trusted_Connection=True;");
});

// add DbContext DI
builder.Services.AddScoped<AppDbContext>();

var app = builder.Build();

//seeding Emp data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();   

    await SeedData.SeedDataAsync(context);

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
