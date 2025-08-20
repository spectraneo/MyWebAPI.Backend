var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); c.RoutePrefix = string.Empty; });

app.MapGet("/", () => Results.Redirect("/swagger"));


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
