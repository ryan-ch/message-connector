using Microsoft.Extensions.DependencyInjection;
using XB.MtParser.Interfaces;

namespace XB.MtParser
{
    public static class MtParserExtensions
    {
        public static IServiceCollection AddMtParser(this IServiceCollection services)
        {
            return services.AddScoped<IMTParser, MtParser>();
        }
    }
}
