using System;
using System.Collections.Generic;
using System.Text;
using Task_Managment.Stores;
using Task_Managment.ViewModels;
using System.Windows.Input;
using System.IO;
using System.Windows.Forms;
using Task_Managment.Models;
using TrayIcon.Services;

namespace Task_Managment.Commands
{
    public class StartCommand : ICommand
    {
        TasksViewModel TasksViewModels { get; set; }

        private readonly NotifyIcon _notifyIcon;

        public static readonly string ImagesPath = Path.GetFullPath("imagesForWpf").Replace("\\bin\\Debug\\", "\\");
        public event EventHandler CanExecuteChanged = null;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public StartCommand(TasksViewModel tasksViewModel)
        {
            _notifyIcon = MainWindowViewModel.NotifyIconInstance;
            this.TasksViewModels = tasksViewModel;
        }

        public  void Execute(object parameter)
        {
            //foreach (Tasklist temp in this.TasksViewModels.TasklistsList)
            //{
            //    foreach (Task task in temp.Tasks)
            //    {
            //        (new TimerStore((new NotifyIconNotificationService(_notifyIcon))))
            //            .Start((int)(TimeSpan.FromTicks(task.Expiretime.Ticks).TotalSeconds - TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds)
            //            );
            //    }
            //}

        }
    }
}
