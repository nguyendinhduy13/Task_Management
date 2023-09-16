using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Task_Managment.Models;
using Task_Managment.ViewModels;
using Task = Task_Managment.Models.Task;

namespace Task_Managment.ViewModel.Commands
{
    public class DeleteCommand : ICommand
    {
        //!Fields
        private TaskDataAccess db = TaskDataAccess.Instance; // 1.

        //!Properties
        TasksViewModel TasksViewModels { get; set; }

        //!Events
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //!Ctor
        public DeleteCommand(TasksViewModel tasksViewModel)
        {
            this.TasksViewModels = tasksViewModel;
        }

        //!Methods
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is ListViewItem)
            {
                ListViewItem item = (ListViewItem)parameter;
                this.TasksViewModels.SelectedTask = item.Content as Task;
            }
            if (parameter is Task || parameter is ListViewItem)
            {
                Task selectedTask = this.TasksViewModels.SelectedTask;
                foreach(Subtask subtask in selectedTask.Subtasks)
                {
                    db.DeleteSelectedSubTask(subtask);
                }
                //remove it from the observable collection if its not null
                this.TasksViewModels.TasksLists?.Remove(selectedTask);
                //and remove selected task from task list if its not null
                this.TasksViewModels.SelectedTasklist.Tasks?.Remove(selectedTask);
                db.DeleteSelectedTask(selectedTask);
                //update the Totalcount of this.TasksViewModels.SelectedTasklist
                this.TasksViewModels.SelectedTasklist.TotalCount = this.TasksViewModels.SelectedTasklist.Tasks.Count.ToString();

                db.UpdateSelectedTasklist(this.TasksViewModels.SelectedTasklist);
                this.TasksViewModels.SubtasksPaneVisible = (this.TasksViewModels.SubtasksPaneVisible == true) ? false : false;
                this.TasksViewModels.SelectedTask = null;
                this.TasksViewModels.PropertyUpdated("SubtasksPaneVisible");
            }

            else if(parameter is Tasklist)
            {
                Tasklist selectedTasklist = this.TasksViewModels.SelectedTasklist;
                db.DeleteSelectedTasklist(selectedTasklist);
                this.TasksViewModels.TasklistsList?.Remove(selectedTasklist);
            } 

           
        }
    }
}
