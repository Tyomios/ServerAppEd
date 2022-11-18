using System.Text.Json;
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