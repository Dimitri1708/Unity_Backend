using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Unity_Backend.Repositories;
using Unity_Backend.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(connectionString);
if (!sqlConnectionStringFound) throw new ArgumentException("No connection string found.");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();

var requireUserPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 10;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddRoles<IdentityRole>()
    .AddDapperStores(options =>
    {
        options.ConnectionString = builder.Configuration.GetConnectionString("DapperIdentity");
    });

builder.Services.AddAuthorizationBuilder()
    .SetDefaultPolicy(requireUserPolicy)
    .SetFallbackPolicy(requireUserPolicy);

builder.Services.AddMvc().AddJsonOptions(options => { options.JsonSerializerOptions.MaxDepth = 64; });
builder.Services.AddScoped<IEnvironmentRepository, EnvironmentRepository>(_=> new EnvironmentRepository(connectionString!));
builder.Services.AddScoped<IObjectRepository, ObjectRepository>(_ => new ObjectRepository(connectionString!));
builder.Services.AddScoped<GlobalFunctions>();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();


app.MapGroup("/account").MapIdentityApi<IdentityUser>().AllowAnonymous();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/",
    () => $"The API is up and running. Connection string found: {(sqlConnectionStringFound ? "Yes" : "No")}").AllowAnonymous();

app.MapPost("/account/logout",
    async (SignInManager<IdentityUser> signInManager, [FromBody] object? empty) =>
    {
        if (empty == null) return Results.Unauthorized();
        await signInManager.SignOutAsync();
        return Results.Ok();
    });

app.Run();

