using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PackageRZ.Utils
{
    public static class SwaggerConfig
    {
        public static SwaggerGenOptions SetSwaggerGenAuthButton(this SwaggerGenOptions swaggerGen)
        {
            var securityScheme = new OpenApiSecurityScheme()
            {
                Description = "Insert Token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    Array.Empty<string>()
                }
            };

            swaggerGen.AddSecurityDefinition("bearerAuth", securityScheme);
            swaggerGen.AddSecurityRequirement(securityRequirement);

            return swaggerGen;
        }
    }
}