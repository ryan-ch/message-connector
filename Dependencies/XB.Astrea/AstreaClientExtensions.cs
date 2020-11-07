using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace XB.Astrea.Client
{
    public static class AstreaClientExtensions
    {
        private static readonly string Null = "null";
        private static readonly string Exception = "Exception";

        public static IServiceCollection AddAstreaClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("astrea", c =>
            {
                c.BaseAddress = new Uri(configuration["AppSettings:AstreaUrl"]);
                c.DefaultRequestHeaders.Add("Accept", "text/plain");
            });

            services.AddTransient<IAstreaClient, AstreaClient>();

            return services;
        }

        public static string ToJson(this Messages.Assessment.Request request)
        {
            if (request == null) return Null;

            try
            {
                return SerializeToJson(request);
            }
            catch (Exception exception)
            {
                //log exception but dont throw one
                return Exception;
            }
        }

        public static string ToJson(this Messages.ProcessTrail.Request request)
        {
            if (request == null) return Null;

            try
            {
                return SerializeToJson(request);
            }
            catch (Exception exception)
            {
                //log exception but dont throw one
                return Exception;
            }
        }

        private static string SerializeToJson(object value)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
        }
    }
}
