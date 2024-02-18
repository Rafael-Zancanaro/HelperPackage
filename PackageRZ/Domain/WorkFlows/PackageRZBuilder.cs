using Microsoft.Extensions.DependencyInjection;

namespace PackageRZ.Domain.WorkFlows
{
    public class PackageRZBuilder
    {
        private PackageRZBuilder()
        {
        }

        public static void Configure(IServiceCollection services)
        {
            services.AddHealthChecks();
        }
    }
}