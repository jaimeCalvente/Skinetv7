using API.Errors;
using API.Extensions;
using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration); // calls the AddApplicationServices class

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();   // to use images

app.UseAuthorization();

app.MapControllers();

// Create and/or apply changes to DB when changes have been made
// This changes will apply when running the app
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();  // apply pending changes to the DB 
                                            // or Creates the DB if not existing

    await StoreContextSeed.SeedAsync(context);   // Save the possible changes into the DB                                     
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
