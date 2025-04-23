// GÜVENLİK ZAFİYETLİ: Hardcoded credentials
app.MapPost("/login", (string username, string password) =>
{
    if (username == "admin" && password == "123456") // <-- zafiyetli
    {
        return Results.Ok("Logged in!");
    }

    return Results.Unauthorized();
});
