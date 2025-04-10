namespace BPMaster.Common.Services.Email
{
    public interface IEmailLogService
    {
        Task SendReminderEmailAsync();
    }
}
