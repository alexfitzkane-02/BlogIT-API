using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;
using BlogIT.Data;
using Microsoft.Extensions.DependencyInjection;
using BlogIT.Repositories.Interface;
using BlogIT.Repositories.Implementation;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

var keyVaultName = builder.Configuration["KeyVault:KeyVaultName"];
var keyVaultUrl = new Uri($"https://{keyVaultName}.vault.azure.net/");

builder.Configuration.AddAzureKeyVault(
    keyVaultUrl,
    new DefaultAzureCredential()
);

var connectionSring = builder.Configuration.GetConnectionString("BlogITDevConnection")!;


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionSring);
});

builder.Services.AddScoped<ICategoryInterface, CategoryRepository>();
builder.Services.AddScoped<IAuthorInterface, AuthorRepository>();
builder.Services.AddScoped<IBlogInterface, BlogRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "BlogIT Api V1");
        options.RoutePrefix = string.Empty;
    });
}


app.UseHttpsRedirection();

//app.MapGet("secrets", async (IConfiguration configuration) =>
//{
//    var secretsClient = new SecretClient(
//        new Uri(configuration["KeyVault:KeyVaultUri"]!),
//        new DefaultAzureCredential());

//    Response<KeyVaultSecret> response = await secretsClient.GetSecretAsync("BlogITDevConnection");
//    return Results.Ok(response);
//});

app.UseAuthorization();

app.MapControllers();

app.Run();
