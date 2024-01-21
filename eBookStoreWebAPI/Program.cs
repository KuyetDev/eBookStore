using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Repositories;
static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<Book>("Books");
    builder.EntitySet<Author>("Authors");
    builder.EntitySet<Publisher>("Publishers");
    builder.EntitySet<User>("Users");
    return builder.GetEdmModel();
}
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddOData(opt => opt.Select().Filter().Count().OrderBy().Expand().SetMaxTop(100).AddRouteComponents("odata", GetEdmModel()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var configuration = builder.Configuration;
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
