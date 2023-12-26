using ThoughtsAligned.Repository;
using ThoughtsAligned.Service;
using ThoughtsAligned.Utility.Extention;
using ThoughtsAligned.Utility.Helpers;
using ThoughtsAligned.Utility.Utilities;
using log4net.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
builder.Services.Configure<AppSetting>(
    builder.Configuration.GetSection("AppSettings"));

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEntityFrameworkSqlServer();

builder.Services.AddCustomHttpContextAccessor();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

XmlConfigurator.Configure(new FileInfo("log4net.config"));

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .SetIsOriginAllowed((host) => true)
           .AllowCredentials();
}));

var config = builder.Configuration.GetConnectionString("DefaultConnection");

BaseService.RegisterTypes(builder.Services, config ?? "");

BaseService.RegisterJWTToken(builder.Services, builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("MyPolicy");

app.UseMiddleware(typeof(GlobalErrorHandlingMiddleware));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Use(async (ctx, next) =>
{
    await next(ctx);
});

app.Use(
    async (ctx, next) =>
    {
        var greeter = ctx.RequestServices.GetService<IRepository<Type>>();
        await next();
    });

app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{id?}");
});

app.Run();