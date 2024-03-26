using BlazorLogin.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorLogin.Client.Extensiones;

using BlazorLogin.Client.Services;
using CurrieTechnologies.Razor.SweetAlert2;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IDepartamentoService,DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService,EmpleadoService>();
builder.Services.AddSweetAlert2();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthenticationStateProvider,AutenticacionExtension>();
builder.Services.AddAuthorizationCore();


await builder.Build().RunAsync();
