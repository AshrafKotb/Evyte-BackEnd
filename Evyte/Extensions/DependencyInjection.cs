using Evyte.ApplicationCore.Interfaces.Services;
using Evyte.ApplicationCore.Interfaces.Services.General_Information;
using Evyte.ApplicationCore.Services.Authantication;
using Evyte.ApplicationCore.Services.Categories;
using Evyte.ApplicationCore.Services.Designs;
using Evyte.ApplicationCore.Services.FAQs;
using Evyte.ApplicationCore.Services.Files;
using Evyte.ApplicationCore.Services.General_Information;
using Evyte.ApplicationCore.Services.Mailing;
using Evyte.ApplicationCore.Services.Repository;
using Evyte.ApplicationCore.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Evyte.Web.Extensions
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