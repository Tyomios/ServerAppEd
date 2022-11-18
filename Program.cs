using System.Text.Json;
using HelloApp.model;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
//app.UseWelcomePage();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;

    if (context.Request.HasJsonContentType())
    {
        if (request.Path == "/api/user")
        {
            var message = "Некорректные данные";

            var jsonoptions = new JsonSerializerOptions();
            jsonoptions.Converters.Add(new PersonConverter());

            var person = await request.ReadFromJsonAsync<Person>(jsonoptions);
            if (person != null)
            {
                message = $"Name: {person.Name}  Age: {person.Age}";
            }

            await response.WriteAsJsonAsync(new { text = message });
        }
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("views/index.html");
    }
});
app.Run();
