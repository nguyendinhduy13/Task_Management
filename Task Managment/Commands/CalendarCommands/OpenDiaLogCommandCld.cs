using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Task_Managment.ViewModels;

namespace Task_Managment.Commands.CalendarCommands
{
    public class OpenDiaLogCommandCld:ICommand
    {
        public EventWindowViewModel TasksViewModel { get; set; }

        //!Events
        public event EventHandler CanExecuteChanged;

        //!Ctor
        public OpenDiaLogCommandCld(EventWindowViewModel tasksViewModel)
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
            this.TasksViewModel.SelectedClockTime = this.TasksViewModel.SelectedClockTime.AddSeconds(0 - this.TasksViewModel.SelectedClockTime.Second);
            this.TasksViewModel.OnPropertyChanged("isDialogOpen");
        }
    }
}
