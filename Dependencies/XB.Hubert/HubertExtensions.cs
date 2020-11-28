using Microsoft.Extensions.DependencyInjection;

namespace XB.Hubert
{
    public static class HubertExtensions
    {
        public static IServiceCollection AddHubert(this IServiceCollection services)
        {
            services.AddTransient<IHubertClient, HubertClient>();
            return services;
        }
    }
}
