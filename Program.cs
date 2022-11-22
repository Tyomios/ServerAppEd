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

	response.ContentType = "text/html; charset=utf-8";

	if (request.Path == "/upload" && request.Method == "POST")
	{
		IFormFileCollection files = request.Form.Files;
		
		var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";
		Directory.CreateDirectory(uploadPath);

		foreach (var file in files)
		{
            string fullPath = $"{uploadPath}/{file.FileName}";
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}
		}
		await response.WriteAsync("Файлы успешно загружены");
	}
	else
	{
		await response.SendFileAsync("views/index.html");
	}
});
app.Run();