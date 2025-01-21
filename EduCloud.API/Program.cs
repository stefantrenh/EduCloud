using EduCloud.Application.DependencyInjection;
using EduCloud.Infrastructure.DependencyInjection;
using EduCloud.Infrastructure.Persistence;
using EduCloud.API.Middlewares; 
using Microsoft.Data.SqlClient;
using System.Data;
using EduCloud.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddSwaggerConfiguration();

builder.Services.AddTransient<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();  

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
