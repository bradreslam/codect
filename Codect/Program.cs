using Microsoft.EntityFrameworkCore;
using DAL;
using Interfaces;
using BLL.Classes;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowReactApp", builder =>
	{
		builder.WithOrigins("http://localhost:5173")  // React app URL
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials();
	});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CodectEfCoreDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("CodectEfCoreDbContext"));
});

builder.Services.AddTransient<ComponentRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IComponent, ComponentManager>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<CodectEfCoreDbContext>();

	if (app.Environment.IsDevelopment())
	{
		dbContext.Database.EnsureCreated();
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
