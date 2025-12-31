using IMS.Plugin.EFCoreSQL;
using IMS.Plugins.InMemory;
using IMS.UseCases.Activeties;
using IMS.UseCases.Activeties.Interfaces;
using IMS.UseCases.Inventories;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Products;
using IMS.UseCases.Products.Interfaces;
using IMS.UseCases.Reports;
using IMS.WebApp.Components;
using Microsoft.EntityFrameworkCore;
using IMS.WebApp.Components.Account;
using IMS.WebApp.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<IMSContext>(options=>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    }
);
builder.Services.AddDbContext<IMSIdentityContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("IMSIdentityContext"));
});
// Add services to the container.
builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
if (builder.Environment.IsEnvironment("Testing"))
{
    //Repos
    builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();
    builder.Services.AddSingleton<IProductRepository, ProductRepository>();
    builder.Services.AddSingleton<IInventoryTransactionRepository, InvTransactionRepository>();
    builder.Services.AddSingleton<IProductTransactionRepo, ProductTransactionRepository>();
} else
{
    builder.Services.AddTransient<IInventoryRepository, InventoryEFCoreRepo>();
    builder.Services.AddTransient<IProductRepository, ProductEFCoreRepository>();
    builder.Services.AddTransient<IInventoryTransactionRepository, InventoryTransEFCoreRepo>();
    builder.Services.AddTransient<IProductTransactionRepo, ProductTransactionEFCoreRepo>();
}
//Inject all this architecture clutter
builder.Services.AddTransient<IViewInventoriesByNameUseCase, ViewInventoriesByNameUseCase>();
builder.Services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
builder.Services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
builder.Services.AddTransient<IViewInventoryByIdUseCase, ViewInventoryByIdUseCase>();
builder.Services.AddTransient<IDeleteInventoryUseCase, DeleteInventoryUseCase>();
builder.Services.AddTransient<IViewProductsByNameUseCase, ViewProductsByNameUseCase>();
builder.Services.AddTransient<IDeleteProductUseCase,DeleteProductUseCase>();
builder.Services.AddTransient<IAddProductUseCase, AddProductUseCase>();
builder.Services.AddTransient<IViewProductByIdUseCase, ViewProductByIdUseCase>();
builder.Services.AddTransient<IEditProductUseCase, EditProductUseCase>();

builder.Services.AddTransient<IPurchaseInventoryUC, PurchaseInventoryUC>();
builder.Services.AddTransient<IProduceProductUC, ProduceProductUC>();
builder.Services.AddTransient<ISellProductUC, SellProductUC>();
builder.Services.AddTransient<ISearchInventoryUC, SearchInventoryUC>();
builder.Services.AddTransient<ISearchProductUC, SearchProductUC>();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddIdentityCore<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<IMSIdentityContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<IdentityUser>, IdentityNoOpEmailSender>();
//build
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();;

app.Run();
