using AutoMapper;
using ThoughtsAligned.IService;
using ThoughtsAligned.Context.Entities;
using ThoughtsAligned.Repository;
using ThoughtsAligned.Utility.Helpers;
using ThoughtsAligned.Utility.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ThoughtsAligned.Models.ViewModels;

namespace ThoughtsAligned.Service
{
    public class BaseService
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<ErrorLog>, Repository<ErrorLog>>();
            services.AddScoped<IRepository<FileInformation>, Repository<FileInformation>>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IErrorLogService, ErrorLogService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileInfoService, FileInfoService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void RegisterTypes(IServiceCollection services, string connection)
        {
            services.AddDbContextPool<ThoughtsAlignedContext>(options => options.UseSqlServer(connection)).BuildServiceProvider();
            AddServices(services);
        }

        public static void RegisterJWTToken(IServiceCollection services, IConfiguration Configuration)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSetting>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSetting>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                       var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                       var userId = String.IsNullOrEmpty(context.Principal?.Identity?.Name) ? "" : context.Principal.Identity.Name;
                       var user = userService.GetById(userId);
                       if (user == null)
                       {
                           context.Fail("Unauthorized");
                       }
                       return Task.CompletedTask;
                   }
               };
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });
        }
    }
}
