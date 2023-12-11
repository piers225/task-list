
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using TaskList.DataAccess;
using TaskList.DataAccess.DataContext.Seeder;
using TaskList.DataAccess.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskList.DataAccess.Models;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(spec => {
  spec.DescribeAllParametersInCamelCase();
  spec.SupportNonNullableReferenceTypes();
});

ServiceCollectionRegistration.Setup(builder.Services);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SECRETKEY"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
       });

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var myService = scope.ServiceProvider.GetRequiredService<ISeeder>();
    myService.Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/tasks", [Authorize] (ITaskListService taskListService) => taskListService.GetTaskItems(1));
app.MapPost("/api/tasks", [Authorize] (ITaskListService taskListService, [FromBody] TaskItemDDL taskItem) => taskListService.AddTaskItem(1, taskItem));
app.MapPut("/api/tasks/{taskId:int}", [Authorize]  (ITaskListService taskListService, [FromQuery] int taskId, [FromBody] TaskItemDDL taskItem )  => taskListService.UpdateTaskItem(1, taskId, taskItem));

app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
{
    if (app.Environment.IsDevelopment()) 
    {
        builder.UseSpa(spa => {
            spa.Options.SourcePath = "../TaskList.Web/Client/";
            spa.UseReactDevelopmentServer(npmScript: "start");
        });
    }
});

app.Run();
