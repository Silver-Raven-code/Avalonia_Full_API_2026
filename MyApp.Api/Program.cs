using System.Runtime.CompilerServices;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();


var periodsJson = File.ReadAllText("Periods.json");

List<PeriodItem> periods = [.. JsonSerializer.Deserialize<List<PeriodItem>>(periodsJson)];



// GET all
app.MapGet("/api/periods", () =>
        {
            return Results.Ok(periods);
        });

// GET by id
app.MapGet("/api/periods/{id:int}", (int id) =>
{
    var item = periods.FirstOrDefault(x => x.Id == id);
    return item is null ? Results.NotFound() : Results.Ok(item);
});

// POST
app.MapPost("/api/periods", (PeriodItem newItem) =>
{
    if (string.IsNullOrWhiteSpace(newItem.Name))
        return Results.BadRequest("Name is required.");

    var nextId = periods.Count == 0 ? 1 : periods.Max(x => x.Id) + 1;
    newItem.Id = nextId;
    periods.Add(newItem);

    return Results.Created($"/api/periods/{newItem.Id}", newItem);
});

// PUT
app.MapPut("/api/periods/{id:int}", (int id, PeriodItem updatedItem) =>
{
    var item = periods.FirstOrDefault(x => x.Id == id);

    if (item is null)
        return Results.NotFound();

    if (string.IsNullOrWhiteSpace(updatedItem.Name))
        return Results.BadRequest("Name is required.");

    item.Name = updatedItem.Name;

    return Results.Ok(item);
});

// DELETE
app.MapDelete("/api/periods/{id:int}", (int id) =>
{
    var item = periods.FirstOrDefault(x => x.Id == id);

    if (item is null)
        return Results.NotFound();

    periods.Remove(item);

    return Results.NoContent();
});

app.Run("http://0.0.0.0:5000");

public class PeriodItem
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
