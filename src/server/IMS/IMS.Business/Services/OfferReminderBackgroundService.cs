using System;
using IMS.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IMS.Business.Services;

public class OfferReminderBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    // private readonly IEmailService _emailService;
    private readonly ILogger<OfferReminderBackgroundService> _logger;

    public OfferReminderBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<OfferReminderBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWorks>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var upcomingOffers = unitOfWork.OfferRepository.GetAllQuery().Where(o => o.DueDate <= DateTime.Now.AddDays(3) && o.DueDate > DateTime.Now && o.Status == "Waiting for approval").Include(o => o.Candidate).Include(o => o.UserApproved).ToList();

                foreach (var offer in upcomingOffers)
                {
                    if (offer == null || offer.Candidate == null || offer.UserApproved == null)
                    {
                        _logger.LogWarning($"Offer or Candidate is null. Skipping this offer.");
                        continue; // Bỏ qua nếu offer hoặc Candidate là null
                    }
                    // Gửi email nhắc nhở
                    var link = $"http://localhost:4200/admin/offers/{offer.Id}/detail";
                    var subject = "no-reply-email-IMS-system <Take action on Job offer>";
                    var message = $@"
                        This email is from IMS system,  
                        You have an offer to take action for Candidate {offer.Candidate?.FullName} 
                        Position: {offer.Position} 
                        Before: {offer.DueDate}. The contract is attached with this no-reply-email.  
                        Please refer to this link to take action: {link}  
                        If anything is wrong, please reach out to recruiter: {offer.Candidate?.Email}. 
                        We are so sorry for this inconvenience.  
                        Thanks & Regards!  
                        IMS Team.";
                    
                    try
                    {
                        await emailService.SendEmailAsync(offer.UserApproved.Email, subject, message);
                        _logger.LogInformation($"Sent reminder email for offer to {offer.UserApproved.Email}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Failed to send reminder for offer: {ex.Message}");
                    }
                }

            }
            
            await Task.Delay(86400000, stoppingToken);

        }

    }
}
