using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
//app.UseWelcomePage();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;

    if (request.Path == "/api/user")
    {
        var message = "Некорректные данные";  
        try
        {
            var person = await request.ReadFromJsonAsync<Person>();
            if (person is not null)
            {
                message = $"Name: {person.Name},   Age: {person.Age}";
            }
        }
        catch { }
        // отправляем пользователю данные
        await response.WriteAsJsonAsync(new { text = message });
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("views/index.html");
    }
});
app.Run();

public record Person(string Name, int Age);
