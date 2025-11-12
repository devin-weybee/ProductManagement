using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManagement.App.Interfaces;
using ProductManagement.App.Services;
using ProductManagement.Data;
using ProductManagement.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAssignedProductService, AssignedProductService>();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.Run();
