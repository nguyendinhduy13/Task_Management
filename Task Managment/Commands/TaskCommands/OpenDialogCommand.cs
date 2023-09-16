using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Task_Managment.Models;
using Task_Managment.ViewModels;
using Task = Task_Managment.Models.Task;
namespace Task_Managment.Commands.TaskCommands
{
    public class OpenDiaLogCommand : ICommand
    {
        private TaskDataAccess db = TaskDataAccess.Instance;
        //!Properties
        public TasksViewModel TasksViewModel { get; set; }

        //!Events
        public event EventHandler CanExecuteChanged;

        //!Ctor
        public OpenDiaLogCommand(TasksViewModel tasksViewModel)
        {
            this.TasksViewModel = tasksViewModel;
        }

        //!Methods
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //parameter is the selectedtasklist (not required because we've also bound the textbox to the tasklist name
            this.TasksViewModel.isDialogOpen = true;
            this.TasksViewModel.SelectedClockTime = DateTime.Now;
            this.TasksViewModel.SelectedClockTime = this.TasksViewModel.SelectedClockTime.AddMinutes(1);
            this.TasksViewModel.SelectedClockTime = this.TasksViewModel.SelectedClockTime.AddSeconds(0-this.TasksViewModel.SelectedClockTime.Second);
            this.TasksViewModel.PropertyUpdated("isDialogOpen");
        }
    }
}
