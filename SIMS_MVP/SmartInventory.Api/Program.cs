using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartInventory.Api.Data;
using SmartInventory.Api.Models;
using SmartInventory.Api.Dtos;   // for LoginDto

var builder = WebApplication.CreateBuilder(args);

// ================== Services ==================

// SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=inventory.db"));

// Lightweight password hasher (no full Identity)
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

// CORS – add the origins you use to open the frontend
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("dev", p => p
        .WithOrigins(
            "http://localhost:8080",
            "http://127.0.0.1:8080",
            "http://192.168.56.1:8080" // remove or change as needed
        )
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// JWT
var jwt = builder.Configuration.GetSection("Jwt");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Swagger + Bearer
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartInventory API", Version = "v1" });

    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", scheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { scheme, Array.Empty<string>() }
    });
});

// ================== App ==================

var app = builder.Build();

app.UseCors("dev");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// ============== Seed default user (once) ==============
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

    if (!await db.Users.AnyAsync())
    {
        var u = new AppUser
        {
            Email = "demo@sims.local",
            Name = "Demo Company"
        };
        u.PasswordHash = hasher.HashPassword(u, "Passw0rd!"); // preset password
        db.Users.Add(u);
        await db.SaveChangesAsync();
    }
}
// ======================================================

// Small health endpoint
app.MapGet("/", () => "SmartInventory API is running");

// ============== AUTH endpoints =================
app.MapPost("/api/auth/login", async (
    AppDbContext db,
    IPasswordHasher<AppUser> hasher,
    [FromBody] LoginDto dto) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
    if (user is null) return Results.Unauthorized();

    var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
    if (result == PasswordVerificationResult.Failed)
        return Results.Unauthorized();

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("name", user.Name)
    };

    var minutes = int.TryParse(jwt["ExpiresMinutes"], out var m) ? m : 480; // default 8h
    var token = new JwtSecurityToken(
        issuer: jwt["Issuer"],
        audience: jwt["Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(minutes),
        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new { token = tokenString, user = new { user.Id, user.Email, user.Name } });
});

app.MapGet("/api/auth/me", [Authorize] (ClaimsPrincipal user) =>
{
    var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
    var email = user.FindFirstValue(ClaimTypes.Email);
    var name = user.FindFirstValue("name");
    return Results.Ok(new { id, email, name });
});
// ===============================================


// ============== Item endpoints =================
app.MapGet("/api/items", [Authorize] async (AppDbContext db) =>
    await db.Items.OrderBy(i => i.Id).ToListAsync());

app.MapGet("/api/items/{id:int}", [Authorize] async (int id, AppDbContext db) =>
    await db.Items.FindAsync(id) is Item item ? Results.Ok(item) : Results.NotFound());

app.MapPost("/api/items", [Authorize] async (Item dto, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Sku) || string.IsNullOrWhiteSpace(dto.Name))
        return Results.BadRequest("Sku og Name må fylles ut");

    db.Items.Add(dto);
    await db.SaveChangesAsync();
    return Results.Created($"/api/items/{dto.Id}", dto);
});

app.MapPut("/api/items/{id:int}", [Authorize] async (int id, Item update, AppDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    item.Sku = update.Sku;
    item.Name = update.Name;
    item.Quantity = update.Quantity;
    // If you added Category to Item, set it here: item.Category = update.Category;

    await db.SaveChangesAsync();
    return Results.Ok(item);
});

app.MapDelete("/api/items/{id:int}", [Authorize] async (int id, AppDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
// ===============================================

app.Run();
