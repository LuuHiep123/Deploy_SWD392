using DataLayer.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SWDProject_BE.AppStarts;
using SWDProject_BE.SignalR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Install AutoMapper
builder.Services.ConfigureAutoMapper();
// Install DI and dbcontext
builder.Services.InstallService(builder.Configuration);
// Swagger config
//builder.Services.ConfigureSwaggerServices("SWDProject");
builder.Services.ConfigureAuthService(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(c =>
//{
//	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//	c.IncludeXmlComments(xmlPath);
//});

builder.Services.AddSwaggerGen(c =>
{
	//c.OperationFilter<SnakecasingParameOperationFilter>();
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "FUES API",
		Version = "v1"
	});
	c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

	var securitySchema = new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		Reference = new OpenApiReference
		{
			Type = ReferenceType.SecurityScheme,
			Id = "Bearer"
		}
	};
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        securitySchema, new string[] { "Bearer" }
        }
    });
});
// Add CORS
builder.Services.AddCors(options =>
{
options.AddPolicy("CorsPolicy",
	builder => builder
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials()
	.WithOrigins("https://localhost:7293", "http://localhost:3000", "https://exchangeweb-fpt.netlify.app")
    );
});

builder.Services.AddSignalR();

var app = builder.Build();

//ChatHyb
app.MapHub<SWDProjectHub>("/chatHub", options =>
{
    // Lấy thông tin kết nối từ cấu hình
    var configuration = app.Configuration;
    var ConnectionString = configuration["SignalR:ConnectionString"];
    var secondaryConnectionString = configuration["SignalR:secondaryConnectionString"];

    // Thiết lập các tùy chọn cho Hub nếu cần thiết
    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling; // Cấu hình các loại transport
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
