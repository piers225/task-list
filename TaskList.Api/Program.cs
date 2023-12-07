
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using TaskList.DataAccess;
using TaskList.DataAccess.DataContext.Seeder;
using TaskList.DataAccess.Service;
using TaskList.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

app.MapGet("/api/get-task-lists", [Authorize] (ITaskListService taskListService) => taskListService.GetTaskItems(1));
app.MapPost("/api/create-pending-task", [Authorize] (ITaskListService taskListService, [FromBody] CreateTaskItem createTaskItem) => taskListService.AddPendingTaskItem(1, createTaskItem.Name));
app.MapPut("/api/update-task-status-pending", [Authorize]  (ITaskListService taskListService, [FromQuery] int taskId)  => taskListService.SetItemStatusToPending(1, taskId));
app.MapPut("/api/update-task-status-complete", [Authorize]  (ITaskListService taskListService, [FromQuery] int taskId)  => taskListService.SetItemStatusToComplete(1, taskId));

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
