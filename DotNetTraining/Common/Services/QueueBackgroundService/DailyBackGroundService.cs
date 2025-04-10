using BPMaster.Common.Services.Email;
using BPMaster.Utilities;
using Common.Loggers.Interfaces;

public class DailyTaskService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    //private readonly string _logFilePath = "Logs/daily_task_log.txt";
    private readonly TimeSpan _targetTime = new TimeSpan(10, 40, 0); // 9:00 AM
    private readonly ILogManager _logger;


    public DailyTaskService(IServiceProvider serviceProvider, ILogManager logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        Directory.CreateDirectory("Logs");
        // Define the target timezone (GMT+7)
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Calculate the next run time in GMT+7
            var now = DateTimeConvertUtil.GetCurrentTimeInUtc7ForMac();

            var nextRun = DateTime.Today.Add(_targetTime);
            if (now.TimeOfDay >= _targetTime) //|| IsWeekend(now)
            {
                nextRun = nextRun.AddDays(1); // Skip to the next day
            }

            // Adjust to skip weekends
            while (IsWeekend(nextRun))
            {
                nextRun = nextRun.AddDays(1);
            }

            // Calculate the delay in UTC
            var nextRunInUtc = DateTimeConvertUtil.ConvertTimeInUtc7ForMac(nextRun);
            var delay = nextRunInUtc - DateTimeConvertUtil.GetCurrentTimeInUtc7ForMac();

            _logger.Info($"Next run (UTC+7): {nextRun}", "DailyTask");
            _logger.Info($"Calculated delay: {delay.TotalSeconds} seconds", "DailyTask");

            // Wait until the next execution time
            if (delay > TimeSpan.Zero)
            {
                await Task.Delay(delay, stoppingToken);
            }

            // Perform the daily task
            _logger.Warn("Calculated delay is negative. Skipping delay and executing task now.", "DailyTask");
            await RunDailyTaskAsync(stoppingToken);
        }
    }

    private async Task RunDailyTaskAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            // Resolve your email service or other dependencies here
            var emailService = scope.ServiceProvider.GetRequiredService<EmailLogService>();

            try
            {
                // Call your email sending function
                _logger.Info("Task is running", "DailyTask");
                await emailService.SendReminderEmailAsync();
                _logger.Info("Task completed", "DailyTask");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error sending reminder email", "DailyTask");
                // Log the exception if logging is set up
            }
        }
    }
    private bool IsWeekend(DateTime date)
    {
        // Returns true if the date is Saturday (6) or Sunday (0)
        var dayOfWeek = date.DayOfWeek;
        return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
    }

    //private void Log(string message)
    //{
    //    var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
    //    File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
    //}
}
