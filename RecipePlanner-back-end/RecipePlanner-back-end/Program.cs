using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecipePlanner_back_end.Contexts;
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
        });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddDbContext<RecipeDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("recipeDb")));
builder.Services.AddDbContext<UserdbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("userDb")));

builder.Services.AddSingleton<IHasher, Md5Hasher>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.UseAuthorization();

app.MapControllers();

app.Run();


