using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Event.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopLeft; });
    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddScoped<DbContext, AppDbContext>();
    builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddRoleStore<AppDbContext>();
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
    builder.Services.AddDbContext<AppDbContext>(x =>
    {
        x.UseNpgsql(builder.Configuration.GetConnectionString("MyServer"));
    });


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.Services.CreateScope().ServiceProvider.GetService<DbContext>()!.Database.Migrate();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseNotyf();

    app.UseRouting();
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

