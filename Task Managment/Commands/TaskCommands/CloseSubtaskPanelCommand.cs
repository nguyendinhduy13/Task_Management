using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Task_Managment.Models;
using Task_Managment.ViewModels;
using Task = Task_Managment.Models.Task;

namespace Task_Managment.Commands
{
    public class CloseSubtaskPanelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private TaskDataAccess db = TaskDataAccess.Instance;

        TasksViewModel TasksViewModel { get; set; }


        //!Ctor
        public CloseSubtaskPanelCommand(TasksViewModel tasksViewModel)
        {
            this.TasksViewModel = tasksViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (TasksViewModel.SubtasksPaneVisible == true)
            {
                TasksViewModel.SubtasksPaneVisible = false;
                this.TasksViewModel.SelectedSubtask = null;
                this.TasksViewModel.PropertyUpdated("SubtasksPaneVisible");
            }

        }
    }
}
