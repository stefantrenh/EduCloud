using Microsoft.OpenApi.Models;

namespace EduCloud.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EduCloud API",
                    Version = "v1",
                    Description = "Education Cloud"
                });

                // options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                // {
                //     In = ParameterLocation.Header,
                //     Description = "Please insert JWT with Bearer into field",
                //     Name = "Authorization",
                //     Type = SecuritySchemeType.ApiKey
                // });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "EduCloud API V1");
                options.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}
