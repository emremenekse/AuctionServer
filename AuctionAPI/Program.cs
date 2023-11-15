using AuctionAPI.Concrete;
using AuctionAPI.Data;
using AuctionAPI.Middlewares;
using AuctionAPI.Repository;
using AuctionAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var Configuration = builder.Configuration;
builder.Services.AddDbContext<AuctionDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuctionService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
//builder.Services.AddEndpointsApiExplorer();
// Configure the HTTP request pipeline.



var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseRouting();
app.UseErrorHandlingMiddleware();
app.UseAuthorization();

app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Controller'larý etkinleþtirir.
});
app.Run();
