using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
//app.UseWelcomePage();

app.Run(async (context) =>
{
    var tom = new Person("Tom", 18);
    context.Response.WriteAsJsonAsync(tom);

    // Are equals.

    //var response = context.Response; 
    //response.Headers.ContentType = "application/json; charset=utf-8";
    //await response.WriteAsync("{'name':'Tom', 'age':37}");
});
app.Run();

public record Person(string Name, int Age);
