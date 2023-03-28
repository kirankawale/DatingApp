using System.Text;
using API.Data;
using API.Interface;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extension
{
    public static class ApplicationServiceExtension
    {
         public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opt => { opt.UseSqlServer(configuration.GetConnectionString("MyCon")); });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                                    ValidateIssuer = false,
                                    ValidateAudience = false
                                };
                            });

                            return services;
        }
    }
}