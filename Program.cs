using Blazored.LocalStorage;
using dentist_project.Data;
using dentist_project.Serivce;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddBlazoredLocalStorage();
//connect Api
builder.Services.AddHttpClient<IUserservice, Userservice>(
    client => client.BaseAddress = new Uri("https://localhost:7008"));
builder.Services.AddHttpClient<IChatTraditional, ChatTraditional>(
    client => client.BaseAddress = new Uri("https://localhost:7008"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
