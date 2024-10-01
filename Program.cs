var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (MinimalApi.Dominio.DTOs.LoginDTO loginDTO) => {
    if (loginDTO.Email == "adm@test.com" && loginDTO.Senha == "1234")
        return Results.Ok("Login com Sucesso");

    else
        return Results.Unauthorized();


});


app.Run();


public class LoginDTO
{

    public string Email { get; set; } = default;

    public string Senha { get; set; } = default;

 }