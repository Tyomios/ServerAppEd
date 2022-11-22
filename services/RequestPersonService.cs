using HelloApp.model;

namespace HelloApp.services;

/// <summary>
/// Содержит методы для обработки запросов.
/// </summary>
public class RequestPersonService
{
    // отправлеяет JSON всех пользователей.
    public async Task GetAllPeople(HttpResponse response, List<Person> users)
    {
        await response.WriteAsJsonAsync(users);
    }

    // отправляет JSON одного пользователя по указанному id.
    public async Task GetPerson(string? id, HttpResponse response, List<Person> users)
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

    // удаляет из списка пользователей пользователя с указанным Id.
    public async Task DeletePerson(string? id, HttpResponse response, List<Person> users)
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
    public async Task CreatePerson(HttpResponse response, HttpRequest request, List<Person> users)
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

    // обновляет данные пользователя.
    public async Task UpdatePerson(HttpResponse response, HttpRequest request, List<Person> users)
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
}