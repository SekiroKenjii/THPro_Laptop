using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model.Configurations;
using Repository.GenericRepository;
using Repository.ImageRepository;
using Repository.Services.Cart;
using Repository.Services.ProductImage;
using Repository.Services.Role;
using Repository.Services.Security;
using Repository.Services.Token;
using Repository.Services.User;
using Repository.Services.Wishlist;
using Serilog;

namespace Utility.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(config.GetConnectionString("THProDB")));

            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();

            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IImageRepository, ImageRepository>();

            services.AddTransient<IProductImageService, ProductImageService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IWishlistService, WishlistService>();

            services.AddAutoMapper(typeof(MapperInitilizer));

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //Cloudinary
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            return services;
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        Log.Error($"Something went wrong in the {contextFeature.Error}");

                        await context.Response.WriteAsync(new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Please Try Again Later!"
                        }.ToString());
                    }
                });
            });
        }
    }
}
