using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Forms = System.Windows.Forms;
using TrayIcon.Services;
using Task_Managment.ViewModels;
using Task_Managment.Models;

namespace Task_Managment.Stores
{
    public class TimerStore : IDisposable
    {
        private readonly INotificationService _notificationService;
        private readonly Timer _timer;

        private DateTime _endTime;
        private bool _wasRunning;
        private int _lastDurationInSeconds;

        Task task;
        private TaskDataAccess db = TaskDataAccess.Instance;

        private double EndTimeCurrentTimeSecondsDifference => TimeSpan.FromTicks(_endTime.Ticks).TotalSeconds - TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;
        public double RemainingSeconds => EndTimeCurrentTimeSecondsDifference > 0 ? EndTimeCurrentTimeSecondsDifference : 0;

        public bool IsRunning => RemainingSeconds > 0;

        public event Action RemainingSecondsChanged;

        public TimerStore(INotificationService notificationService, Task task)
        {
            this.task = task;
            
            _notificationService = notificationService;
            _notificationService.NotificationAccepted += NotificationService_NotificationAccepted;

            _timer = new Timer(1000);
            _timer.Elapsed += Timer_Elapsed;
        }

        public void Start(int durationInSeconds)
        {
            _lastDurationInSeconds = durationInSeconds;

            _timer.Start();
            _endTime = DateTime.Now.AddSeconds(durationInSeconds);

            OnRemainingSecondsChanged();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OnRemainingSecondsChanged();

            if(_wasRunning && !IsRunning)
            {
                _notificationService.Notify(this.task.Name, this.task.Expiretime.ToLocalTime().ToString(), 
                    3000, NotificationType.RestartTimer, Forms.ToolTipIcon.Info);
                this.task.IsNotifify = true;
                db.UpdateSelectedTask(this.task);
                // mark the task that is done!
            }

            _wasRunning = IsRunning;
        }

        private void NotificationService_NotificationAccepted(NotificationType lastNotificationType)
        {
            if(lastNotificationType == NotificationType.RestartTimer)
            {
                Start(_lastDurationInSeconds);
            }
        }

        private void OnRemainingSecondsChanged()
        {
            RemainingSecondsChanged?.Invoke();
        }

        public void Dispose()
        {
            _notificationService.NotificationAccepted -= NotificationService_NotificationAccepted;
            _timer.Dispose();
        }
    }
}
