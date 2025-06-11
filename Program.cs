using QuantusBI.Infraestrutura;
using QuantusBI.Repositorio;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Injeção de dependência
builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();

builder.Services.AddScoped<IOrgaoRepositorio, OrgaoRepositorio>();
builder.Services.AddScoped<IEntidadeRepositorio, EntidadeRepositorio>();
builder.Services.AddScoped<IDocumentoContratualRepositorio, DocumentoContratualRepositorio>(); // Corrigido
builder.Services.AddScoped<IMetaRepositorio, MetaRepositorio>();
builder.Services.AddScoped<IMetaPeriodoValorRepositorio, MetaPeriodoValorRepositorio>();

// Novos repositórios
builder.Services.AddScoped<IMetaFaixaCumprimentoRepositorio, MetaFaixaCumprimentoRepositorio>();
builder.Services.AddScoped<IMetaConfiguracaoRepositorio, MetaConfiguracaoRepositorio>();

builder.Services.AddControllersWithViews();
builder.Logging.AddConsole();

var app = builder.Build();

// Cultura pt-BR
var defaultCulture = "pt-BR";
var supportedCultures = new[] { new CultureInfo(defaultCulture) };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new[] { new AcceptLanguageHeaderRequestCultureProvider() }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();