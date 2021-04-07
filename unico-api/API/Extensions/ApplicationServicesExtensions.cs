using System.Linq;
using Application.Errors;
using Application.Helpers;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
           // services.AddScoped<IRandomStringGenerator, RamdomStringGeneratorHelper>();
            
            // services.AddScoped<IEmailSender, EmailService>();
            // services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotosUrl, GetImagesPathHelper>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            return services;

        }
    }
}