using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecipePlanner_back_end.Contexts;
using RecipePlanner_back_end.Middleware;
using RecipePlanner_back_end.Services;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy",
        builder =>
        {
            builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
            //.WithOrigins("http://localhost:3000", "localhost:3000");
        });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddDbContext<RecipeDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("recipeDb")));
builder.Services.AddDbContext<UserdbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("userDb")));

builder.Services.AddSingleton<IHasher, Md5Hasher>();
builder.Services.AddScoped<IAuthService, SessionAuthService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

//// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "ASP API Recipe";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API");
        c.RoutePrefix = String.Empty;
    });
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

app.UseSession();

app.UseSessionAuth();

app.UseAuthorization();

app.MapControllers();

app.Run();


