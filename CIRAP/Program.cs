using CIRAP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ProjectInfoContext>();
builder.Services.AddTransient<sys_SubProjectContext>();
builder.Services.AddTransient<ProjectTasksContext>();
builder.Services.AddTransient<Sys_Task1StepContext>();
builder.Services.AddTransient<Sys_Task2StepContext>();
builder.Services.AddTransient<UserContext>();
builder.Services.AddTransient<RiskAssessContext>();
builder.Services.AddTransient<ProjectTasksRiskAssessContext>();
builder.Services.AddTransient<RiskAssessCMpersonContext>();


// �N Session �s�b ASP.NET Core �O���餤
builder.Services.AddDistributedMemoryCache();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}");

app.Run();
