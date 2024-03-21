/*
using StudentPerformanceApp;

PopulateDatabaseFromCSV.Main();
*/

/*
using StudentMLTraining;

StudentMLTraining.Pogram.Main();
*/

using Microsoft.EntityFrameworkCore;
using StudentPerformanceApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StudentDbContext>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();

