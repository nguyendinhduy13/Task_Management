using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Task_Managment.Models;
using Task_Managment.ViewModels;
using Task_Managment.Views;
using Task = Task_Managment.Models.Task;

namespace Task_Managment.Commands.TaskCommands
{
    public class PickTaskIconCommand : ICommand
    {


        //!Fields
        private TaskDataAccess db = TaskDataAccess.Instance; // 1.

        //!Properties
        TasksViewModel TasksViewModel { get; set; }

        //!Events
        public event EventHandler CanExecuteChanged;


        //!Ctor
        public PickTaskIconCommand(TasksViewModel tasksViewModel)
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
            if (parameter is Tasklist)
            {
                this.TasksViewModel.PropertyUpdated("IconTaskList");
            }
            else
            {
                if (parameter is Image)
                {
                    Image image = (Image)parameter;
                    //this.TasksViewModel.SelectedTasklist.IconSource = new Uri(image.Source.ToString());
                    this.TasksViewModel.SelectedTasklist.IconName =  Path.GetFileName(image.Source.ToString());
                    Tasklist temp = this.TasksViewModel.SelectedTasklist;
                    ObservableCollection<Tasklist> TasklistsListTemp = new ObservableCollection<Tasklist>(this.TasksViewModel.TasklistsList);
               


                    db.UpdateSelectedTasklist(this.TasksViewModel.SelectedTasklist);
                    this.TasksViewModel.TasklistsList.Clear();

                    this.TasksViewModel.TasklistsList = TasklistsListTemp;
                    this.TasksViewModel.PropertyUpdated("TasklistsList");
                    this.TasksViewModel.SelectedTasklist = temp;
                    this.TasksViewModel.PropertyUpdated("SelectedTaskList");

                }

            }
            this.TasksViewModel.IconPaneVisible = !this.TasksViewModel.IconPaneVisible;
            this.TasksViewModel.PropertyUpdated("IconPaneVisible");

        }
    }

}
