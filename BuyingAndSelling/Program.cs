using Microsoft.Extensions.Configuration;
using Model.DBModel.MongoDBModel;
using Model;
using PremiseGlobalLibrary;
using PremiseGlobalLibrary.Middleware;
using Model.DBModel;
using DatabaseLayer.Dal;
using DatabaseLayer.DBContext;
using DatabaseLayer;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("ConnectionString"));
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.Configure<DBCollections>(builder.Configuration.GetSection("DBCollections"));

builder.Services.AddSingleton<IDBContext, MongoDBContext>();
builder.Services.AddSingleton<ProductDal>();
builder.Services.AddSingleton<ProductTagDal>();
builder.Services.AddMediatR(typeof(DatabseLayerStartup).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else { }

app.UseCors(b =>
            b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Total-Count")
            );
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
