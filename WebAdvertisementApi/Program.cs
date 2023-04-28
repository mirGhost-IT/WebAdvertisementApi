using LibAdvertisementDB;
using LibBusinessLogic.Class;
using LibBusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IInfo, Info>();
builder.Services.AddTransient<IOrderByAndSearch, OrderByAndSearch>();
builder.Services.AddTransient<IAdvertisementInteraction, AdvertisementInteraction>();
builder.Services.AddTransient<IPagination, Pagination>();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AdvertisementContext>(options =>
    options.UseNpgsql(connection));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



app.Run();
