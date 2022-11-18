using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
//app.UseWelcomePage();

//app.Run(async (context) => await context.Response.WriteAsync("Hello, Artyom"));

//app.Run(async (context) =>
//{
//    var response = context.Response;
//    response.ContentType = "text/html; charset=utf-8";
//    await context.Response.WriteAsync(
//        "<h2> Hello, Artyom </h2><p>this is a response as html</p>");
//});

app.Run(async (context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
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

//    var name = context.Request.Query["name"].Count == 0/*.Equals(String.Empty) 
//*/        ? new StringValues("Пользователь") : context.Request.Query["name"];

//    switch (path)
//    {
//        //case "/":
//        //    var stringBuilder = new System.Text.StringBuilder("<table>");

//        //    foreach (var requestHeader in context.Request.Headers)
//        //    {
//        //        stringBuilder.Append($"<tr><td>{requestHeader.Key}</td><td>{requestHeader.Value}</td></tr>");
//        //    }

//        //    stringBuilder.Append("</table>");
//        //    await context.Response.WriteAsync(stringBuilder.ToString());
//        //    break;

//        case "/home":
//            await context.Response.SendFileAsync(
//                //$"<h2> Hello, {name} </h2><p>this is a response as html<i> for home page</i></p>");
//                "views/home.html");
//            break;

//        case "/about":
//            await context.Response.SendFileAsync(
//                //$"<h2> Hello, {name} </h2><p>this is a response as html<i> for home page</i></p>");
//                "views/about.html");
//            break;

//        case "/":
//            await context.Response.SendFileAsync(
//                //$"<h2> Hello, {name} </h2><p>this is a response as html<i> for home page</i></p>");
//                "views/index.html");
//            break;

//        default:
//            await context.Response.WriteAsync(
//                $"<h2> Answer for page{path}</h2>");
//            break;
});
app.Run();
