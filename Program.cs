using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
//app.UseWelcomePage();

app.Run(async (context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";

    if (context.Request.Path.Equals("/postuser"))
    {
        var form = context.Request.Form;
        string name = form["name"];
        string age = form["age"];
        string[] languages = form["languages"];

        string aioLangs = String.Join(", ", languages);

        await context.Response.WriteAsync($"<div><p>Name: {name}</p>" +
                                          $"<p>Age: {age}</p>" +
                                          $"<div>Languages: {aioLangs}.</ul></div>");
        return;
    }

    var path = $"views/{context.Request.Path}.html";

    if (File.Exists(path))
    {
        await context.Response.SendFileAsync(path);
    }
    else
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync("<h2>Not Found</h2>");
    }
});
app.Run();
