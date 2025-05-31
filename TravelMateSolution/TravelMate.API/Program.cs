// 在你现有的 Program.cs 中添加 CORS 配置

using Microsoft.EntityFrameworkCore;
using TravelMate.Data;
using TravelMate.Service.Implementations;
using TravelMate.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ✅ 注册基本服务
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(); // 如果有 HTTP 外部请求，例如 AI 接口

// ✅ 添加 CORS 服务
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ✅ 注册数据库上下文
builder.Services.AddDbContext<TravelMateDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Default"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default"))
    ));

// ✅ 注册依赖注入服务
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IIntRecService, IntRecService>();
builder.Services.AddScoped<IZhipuAIService, ZhipuAIService>();
builder.Services.AddScoped<IItineraryService, ItineraryService>();

var app = builder.Build();

// ✅ 配置中间件
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ 启用 CORS（要在 UseAuthorization 之前）
app.UseCors();

app.UseAuthorization();
app.MapControllers();
app.Run();