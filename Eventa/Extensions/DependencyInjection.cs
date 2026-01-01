using Eventa.ApplicationCore.Interfaces.Services;
using Eventa.ApplicationCore.Interfaces.Services.General_Information;
using Eventa.ApplicationCore.Services.Authantication;
using Eventa.ApplicationCore.Services.Categories;
using Eventa.ApplicationCore.Services.Designs;
using Eventa.ApplicationCore.Services.FAQs;
using Eventa.ApplicationCore.Services.Files;
using Eventa.ApplicationCore.Services.General_Information;
using Eventa.ApplicationCore.Services.Mailing;
using Eventa.ApplicationCore.Services.Repository;
using Eventa.ApplicationCore.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Eventa.Web.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterCustomServises(this IServiceCollection services)
        {
            services.AddScoped<IAuthanticationService, AuthanticationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IFAQService, FAQService>();
            services.AddScoped<IDesignService, DesignService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IRequestDataRepository, RequestDataRepository>();
            services.AddScoped<IRequestGalleryPhotoRepository, RequestGalleryPhotoRepository>();
            services.AddScoped<IInvitationService, InvitationService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IGeneralInformationService, GeneralInformationService>();
            services.AddScoped<IQRCodeService, QRCodeService>();
            services.AddTransient<IMailingService, MailingService>();

            return services;
        }

        public static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailingSetting>(configuration.GetSection(nameof(MailingSetting)));
            services.Configure<ImagekitSettings>(configuration.GetSection(nameof(ImagekitSettings)));

            return services;
        }

    }
}