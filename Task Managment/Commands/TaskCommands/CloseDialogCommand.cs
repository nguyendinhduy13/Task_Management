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
    public class CloseDialogCommand : ICommand
    {
        private TaskDataAccess db = TaskDataAccess.Instance;
        //!Properties
        public TasksViewModel TasksViewModel { get; set; }

        //!Events
        public event EventHandler CanExecuteChanged;

        //!Ctor
        public CloseDialogCommand(TasksViewModel tasksViewModel)
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
            this.TasksViewModel.isDialogOpen = false;
            this.TasksViewModel.PropertyUpdated("isDialogOpen");

            if (parameter as string == "0")
                return;
            DateTime temp = this.TasksViewModel.SelectedTime;
            temp = temp.AddDays(double.Parse(this.TasksViewModel.SelectedCalendarDate.Day.ToString()) - double.Parse(temp.Day.ToString()));
            temp = temp.AddMonths(int.Parse(this.TasksViewModel.SelectedCalendarDate.Month.ToString()) - int.Parse(temp.Month.ToString()));
            temp = temp.AddYears(int.Parse(this.TasksViewModel.SelectedCalendarDate.Year.ToString()) - int.Parse(temp.Year.ToString()));

            //if (this.TasksViewModel.SelectedTask != null)
            //    this.TasksViewModel.SelectedTime = temp;

            //temp = _selectedTime;
            temp = temp.AddHours(this.TasksViewModel.SelectedClockTime.Hour - temp.Hour);
            temp = temp.AddMinutes(this.TasksViewModel.SelectedClockTime.Minute - temp.Minute);
            temp = temp.AddSeconds(0 - temp.Second);
            //if (SelectedTask != null)
            //    this.SelectedTime = temp;
            if (this.TasksViewModel.SelectedTask != null)
                this.TasksViewModel.SelectedTime = temp;


            return;
           
        }
    }
}