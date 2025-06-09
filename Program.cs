using QuantusBI.Infraestrutura;
using QuantusBI.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------
// Injeção de dependência (Dependency Injection)
// ----------------------------------------

// Injeção da fábrica de conexões como serviço Singleton (mesmo objeto para toda a aplicação)
builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();

// Injeção dos repositórios como serviços Scoped (novo objeto por requisição)
builder.Services.AddScoped<IOrgaoRepositorio, OrgaoRepositorio>();
builder.Services.AddScoped<IEntidadeRepositorio, EntidadeRepositorio>(); // <- Novo

// ----------------------------------------
// Adiciona suporte a Controllers com Views
// ----------------------------------------
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ----------------------------------------
// Pipeline de requisição HTTP
// ----------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ----------------------------------------
// Rota padrão do MVC
// ----------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();