using Microsoft.Extensions.DependencyInjection;

namespace PackageRZ.Domain.WorkFlows
{
    public class PackageRZBuilder : IWorkFlow, IAddPackageRZ
    {
        private readonly IServiceCollection _services;

        private PackageRZBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public static IWorkFlow Configure(IServiceCollection services)
            => new PackageRZBuilder(services);

        public IAddPackageRZ AddPackageRZ()
        {
            _services.AddHealthChecks();
            _services.AddAuthentication();
            _services.AddAuthorization();
            return this;
        }

        public IAddPackageRZ AddAutenticacaoLoguin()
        {

            return this;
        }
    }
}