using Microsoft.EntityFrameworkCore;
using SmartInventory.Api.Data;
using SmartInventory.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=inventory.db"));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS – tillat vår lille frontend (http://localhost:8080)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", p => p
        .WithOrigins("http://localhost:8080", "http://127.0.0.1:8080", "http://192.168.56.1:8080")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("dev");

// Swagger UI i dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Minimal API-ruter
app.MapGet("/", () => "SmartInventory API is running");

// List alle items
app.MapGet("/api/items", async (AppDbContext db) =>
    await db.Items.OrderBy(i => i.Id).ToListAsync());

// Hent ett item
app.MapGet("/api/items/{id:int}", async (int id, AppDbContext db) =>
    await db.Items.FindAsync(id) is Item item ? Results.Ok(item) : Results.NotFound());

// Opprett item
app.MapPost("/api/items", async (Item dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Sku) || string.IsNullOrWhiteSpace(dto.Name))
        return Results.BadRequest("Sku og Name må fylles ut");

    db.Items.Add(dto);
    await db.SaveChangesAsync();
    return Results.Created($"/api/items/{dto.Id}", dto);
});

// Oppdater item quantity/navn/sku
app.MapPut("/api/items/{id:int}", async (int id, Item update, AppDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();
    item.Sku = update.Sku;
    item.Name = update.Name;
    item.Quantity = update.Quantity;
    await db.SaveChangesAsync();
    return Results.Ok(item);
});

// Slett item
app.MapDelete("/api/items/{id:int}", async (int id, AppDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();
    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
