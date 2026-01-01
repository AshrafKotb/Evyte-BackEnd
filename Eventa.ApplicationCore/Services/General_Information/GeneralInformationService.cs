using Eventa.ApplicationCore.Interfaces.Services.General_Information;
using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Eventa.ApplicationCore.Services.General_Information
{
    public class GeneralInformationService : IGeneralInformationService
    {
        private readonly ApplicationDbContext _context;

        public GeneralInformationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GeneralInformation> GetGeneralInformationAsync()
        {
            try
            {
                return await _context.GeneralInformation.FirstOrDefaultAsync(x => !x.IsDeleted);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching GeneralInformation: {ex.Message}");
                return null;
            }
        }

        public async Task<GeneralInformation> CreateDefaultGeneralInformationAsync()
        {
            try
            {
                var defaultInfo = new GeneralInformation
                {
                    FaceBook = "https://facebook.com",
                    Instagram = "https://instagram.com",
                    X = "https://x.com",
                    Tiktok = "https://tiktok.com",
                    Youtube = "https://youtube.com",
                    WhatsApp = "https://whatsapp.com",
                    Snapchat = "https://snapchat.com",
                    Email = "example@domain.com",
                    AddressAr = "",
                    AddressEn = "",
                    PhoneNumber = "",
                    PhoneNumber2 = "",
                    TermsAndConditionsAr = "?????? ???????? ???????? ???...",
                    TermsAndConditionsEn = "Terms and Conditions in English here...",
                    CreatedDate = DateTime.UtcNow
                };

                _context.GeneralInformation.Add(defaultInfo);
                await _context.SaveChangesAsync();
                return defaultInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating GeneralInformation: {ex.Message}");
                throw;
            }
        }

        public async Task<GeneralInformation> UpdateTermsAndConditionsAsync(GeneralInformation termsAndConditions)
        {
            try
            {
                var existingInfo = await _context.GeneralInformation.FirstOrDefaultAsync(x => !x.IsDeleted);
                if (existingInfo == null)
                {
                    throw new Exception("GeneralInformation not found.");
                }

                existingInfo.TermsAndConditionsAr = termsAndConditions.TermsAndConditionsAr;
                existingInfo.TermsAndConditionsEn = termsAndConditions.TermsAndConditionsEn;
                existingInfo.UpdatedDate = DateTime.UtcNow;

                _context.GeneralInformation.Update(existingInfo);
                await _context.SaveChangesAsync();
                return existingInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating TermsAndConditions: {ex.Message}");
                throw;
            }
        }

        public async Task<GeneralInformation> UpdateContactInformationAsync(GeneralInformation contactInformation)
        {
            try
            {
                var existingInfo = await _context.GeneralInformation.FirstOrDefaultAsync(x => !x.IsDeleted);
                if (existingInfo == null)
                {
                    throw new Exception("GeneralInformation not found.");
                }

                existingInfo.AddressAr = contactInformation.AddressAr;
                existingInfo.AddressEn = contactInformation.AddressEn;
                existingInfo.Email = contactInformation.Email;
                existingInfo.Email2 = contactInformation.Email2;
                existingInfo.PhoneNumber = contactInformation.PhoneNumber;
                existingInfo.PhoneNumber2 = contactInformation.PhoneNumber2;
                existingInfo.FaceBook = contactInformation.FaceBook;
                existingInfo.Instagram = contactInformation.Instagram;
                existingInfo.X = contactInformation.X;
                existingInfo.Tiktok = contactInformation.Tiktok;
                existingInfo.Youtube = contactInformation.Youtube;
                existingInfo.WhatsApp = contactInformation.WhatsApp;
                existingInfo.Snapchat = contactInformation.Snapchat;
                existingInfo.UpdatedDate = DateTime.UtcNow;

                _context.GeneralInformation.Update(existingInfo);
                await _context.SaveChangesAsync();
                return existingInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating ContactInformation: {ex.Message}");
                throw;
            }
        }
    }
}