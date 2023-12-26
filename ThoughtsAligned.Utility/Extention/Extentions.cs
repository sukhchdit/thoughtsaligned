using ThoughtsAligned.Utility.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Reflection;

namespace ThoughtsAligned.Utility.Extention
{
    public static class Extentions
    {
        public static string RootUrl(this ControllerBase controller)
        {
            return controller.Url.Content("~/");
        }
        public static string GetFullUrlByCtrl(HttpRequest httpRequest)
        {
            return UriHelper.GetDisplayUrl(httpRequest);
        }
        public static string GetBaseUrlByCtrl(HttpRequest httpRequest)
        {
            return UriHelper.GetDisplayUrl(httpRequest);
        }
        public static IApplicationBuilder UseHttpContext(this IApplicationBuilder app)
        {
            CustomHttpContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }
        public static void AddCustomHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        public static void IgnoreProperty<T, TR>(this T parameter, Expression<Func<T, TR>> propertyLambda)
        {
            var parameterType = parameter?.GetType();
            var propertyName = propertyLambda.GetReturnedPropertyName();
            if (propertyName == null)
            {
                return;
            }
            if (parameterType != null)
            {
                var jsonPropertyAttribute = parameterType?.GetProperty(propertyName).GetCustomAttribute<JsonPropertyAttribute>();
                if (jsonPropertyAttribute != null)
                    jsonPropertyAttribute.DefaultValueHandling = DefaultValueHandling.Ignore;
            }
        }

        public static string GetReturnedPropertyName<T, TR>(this Expression<Func<T, TR>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            var memberPropertyInfo = member?.Member as PropertyInfo;
            if (memberPropertyInfo != null)
                return memberPropertyInfo.Name;
            else
                return string.Empty;
        }
    }
}
