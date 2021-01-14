using Microsoft.Extensions.DependencyInjection;

namespace XB.Hubert
{
    public static class HubertExtensions
    {
        public static IServiceCollection AddHubert(this IServiceCollection services)
        {
           return services.AddScoped<IHubertClient, HubertClient>();
        }
    }
}
