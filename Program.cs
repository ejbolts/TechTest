var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

//app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();
