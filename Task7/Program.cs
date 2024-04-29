using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Task7.Configuration;
using Task7.Data;
using Task7.Hubs;
using Task7.Interfaces.Services;
using Task7.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityConfiguration();

builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
});

builder.Services.AddScoped<IApplicationAuthenticateService, ApplicationAuthenticateService>();
builder.Services.AddScoped<IChatService, ChatServise>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/chat");
app.MapHub<VideoChatHub>("/videoChat");

app.Run();
