using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Task_Managment.ViewModels;

namespace Task_Managment.Commands.CalendarCommands
{
    public class CloseDialogCommandCld:ICommand
    {
        public EventWindowViewModel TasksViewModel { get; set; }

        //!Events
        public event EventHandler CanExecuteChanged;

        //!Ctor
        public CloseDialogCommandCld(EventWindowViewModel tasksViewModel)
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
            this.TasksViewModel.OnPropertyChanged("isDialogOpen");       

            //if (this.TasksViewModel.SelectedTask != null)
            //    this.TasksViewModel.SelectedTime = temp;

            //temp = _selectedTime;

            return;

        }
    }
}
