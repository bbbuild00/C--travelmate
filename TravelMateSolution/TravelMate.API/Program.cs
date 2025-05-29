using Microsoft.EntityFrameworkCore;
using TravelMate.Data;
using TravelMate.Service.Implementations;
using TravelMate.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ✅ 服务注册都写在 Build 之前
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>(); // 注册服务
builder.Services.AddHttpClient(); // 注册 HttpClient 用于调用微信 API
builder.Services.AddDbContext<TravelMateDbContext>(options =>
	options.UseMySql(
		builder.Configuration.GetConnectionString("Default"),
		ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default"))
	));

var app = builder.Build();

// ✅ 中间件写在 Build 之后
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
