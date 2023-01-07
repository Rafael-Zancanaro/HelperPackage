using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PackageRZ.Domain.Utils
{
    public static class AppUse
    {
        public static IApplicationBuilder UsePackageRZ(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/health");
            if (!env.IsDevelopment())
                app.UseExceptionHandler("/error");
            return app;
        }
    }
}