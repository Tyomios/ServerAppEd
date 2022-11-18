using System.Text.Json;
using System.Text.RegularExpressions;
using HelloApp.model;
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

    var expressionForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";

    if (path.Equals("/api/users") && request.Method.Equals("GET"))
    {
        await GetAllPeople(response);
    }
    else if (Regex.IsMatch(path, expressionForGuid) 
             && request.Method.Equals("GET"))
    {
        string? id = path.Value?.Split("/")[3];
        await GetPerson(id, response);
    }
    else if (path.Equals("/api/users") && request.Method.Equals("POST"))
    {
        await CreatePerson(response, request);
    }
    else if (path.Equals("/api/users") && request.Method.Equals("PUT"))
    {
        await UpdatePerson(response, request);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method.Equals("DELETE"))
    {
        string? id = path.Value?.Split("/")[3];
        await DeletePerson(id, response);
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }
});
app.Run();

// получение всех пользователей
async Task GetAllPeople(HttpResponse response)
{
    await response.WriteAsJsonAsync(users);
}


// получение одного пользователя по id
async Task GetPerson(string? id, HttpResponse response)
{
    Person? user = users.FirstOrDefault((u) => u.Id.Equals(id));

    if (user is not null)
    {
        await response.WriteAsJsonAsync(user);
        return;
    }

    response.StatusCode = 404;
    await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
}

//удаление пользователя по Id.
async Task DeletePerson(string? id, HttpResponse response)
{
    Person? user = users.FirstOrDefault((u) => u.Id.Equals(id));
    
    if (user is not null)
    {
        users.Remove(user);
        await response.WriteAsJsonAsync(user);
        return;
    }

    response.StatusCode = 404;
    await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
}

// Создает пользователя по полученным из запроса данным.
async Task CreatePerson(HttpResponse response, HttpRequest request)
{
    try
    {
        var user = await request.ReadFromJsonAsync<Person>();
        if (user is not null)
        {
            user.Id = Guid.NewGuid().ToString();
            users.Add(user);

            await response.WriteAsJsonAsync(user);
            return;
        }

        throw new Exception("Некорректные данные");
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
    }
}

// обновление данных пользователя.
async Task UpdatePerson(HttpResponse response, HttpRequest request)
{
    try
    {
        Person? userData = await request.ReadFromJsonAsync<Person>();
        if (userData is not null)
        {
            var user = users.FirstOrDefault(u => u.Id == userData.Id);
            
            if (user is not null)
            {
                user.Age = userData.Age;
                user.Name = userData.Name;
                await response.WriteAsJsonAsync(user);
                return;
            }

            response.StatusCode = 404;
            await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
            return;
        }

        throw new Exception("Некорректные данные");
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
    }
}