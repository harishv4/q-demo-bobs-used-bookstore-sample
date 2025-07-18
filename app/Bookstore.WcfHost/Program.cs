using Bookstore.Data;
using Bookstore.Data.FileServices;
using Bookstore.Data.ImageResizeService;
using Bookstore.Data.ImageValidationServices;
using Bookstore.Data.Repositories;
using Bookstore.Domain;
using Bookstore.Domain.Books;
using Bookstore.Domain.Orders;
using Bookstore.WcfServices;
using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookstoreDbDefaultConnection")));

// Register repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<Bookstore.Domain.IFileService, LocalFileService>(_ => 
    new LocalFileService(Directory.GetCurrentDirectory()));
builder.Services.AddScoped<Bookstore.Domain.IImageResizeService, ImageResizeService>();
builder.Services.AddScoped<Bookstore.Domain.IImageValidationService, LocalImageValidationService>();

// Register WCF service
builder.Services.AddScoped<BookSearchService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<BookSearchService>();
    serviceBuilder.AddServiceEndpoint<BookSearchService, IBookSearchService>(
        new BasicHttpBinding(), "/BookSearchService");
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

Console.WriteLine("WCF Book Search Service is running at http://localhost:8080/BookSearchService");
Console.WriteLine("Press Ctrl+C to stop the service.");

app.Run("http://localhost:8080");