using System.Text.Json;
using System.Text.RegularExpressions;
using HelloApp.model;
using HelloApp.services;
using Microsoft.Extensions.Primitives;

var users = new List<Person>
{
    new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
};


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    var requestPersonService = new RequestPersonService();
    var expressionForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";

    if (path.Equals("/api/users") && request.Method.Equals("GET"))
    {
        await requestPersonService.GetAllPeople(response, users);
    }
    else if (Regex.IsMatch(path, expressionForGuid) 
             && request.Method.Equals("GET"))
    {
        string? id = path.Value?.Split("/")[3];
        await requestPersonService.GetPerson(id, response, users);
    }
    else if (path.Equals("/api/users") && request.Method.Equals("POST"))
    {
        await requestPersonService.CreatePerson(response, request, users);
    }
    else if (path.Equals("/api/users") && request.Method.Equals("PUT"))
    {
        await requestPersonService.UpdatePerson(response, request, users);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method.Equals("DELETE"))
    {
        string? id = path.Value?.Split("/")[3];
        await requestPersonService.DeletePerson(id, response, users);
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }
});
app.Run();