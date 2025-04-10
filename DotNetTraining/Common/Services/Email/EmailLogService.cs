using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BPMaster.Common.Services.Email
{
    public class EmailLogService : IEmailLogService
    {
        private readonly ILogger<EmailLogService> _logger;

        public EmailLogService(ILogger<EmailLogService> logger)
        {
            _logger = logger;
        }

        public async Task SendReminderEmailAsync()
        {
            // TODO: Thêm logic gửi email ở đây (SMTP, MailKit, SendGrid,...)
            _logger.LogInformation("Sending reminder email...");

            // Giả lập gửi email mất 1 giây
            await Task.Delay(1000);

            _logger.LogInformation("Reminder email sent successfully.");
        }
    }
}
