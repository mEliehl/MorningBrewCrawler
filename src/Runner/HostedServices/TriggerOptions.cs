using System;

namespace Runner.HostedServices
{
    public class TriggerOptions
    {
        public uint PageLimit { get; set; }
        public uint UtcHourToRun { get; set; }
        public uint UtcMinuteToRun { get; set; }

        public DateTime GetScheduledTime()
        {
            var scheduledTime = DateTime.Today.AddHours(UtcHourToRun).AddMinutes(UtcMinuteToRun);
            if (DateTime.UtcNow > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }
            return scheduledTime;

        }
    }
}
