using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Task_Managment.Models;
using Task_Managment.ViewModels;

namespace Task_Managment.ViewModel.Commands
{
    public class MarkImportantCommand : ICommand
    {
        private TaskDataAccess db = TaskDataAccess.Instance;


        //!Properties
        public TasksViewModel TasksViewModel { get; set; }

        //!Ctor
        public MarkImportantCommand(TasksViewModel tasksViewModel)
        {
            this.TasksViewModel = tasksViewModel;
        }

        //!Events
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        //!Methods
        public bool CanExecute(object parameter)
        {


            //you can only mark important if the task you are trying to mark important == selected task


            return true;



        }

        public void Execute(object parameter)
        {
            if (parameter is ListViewItem)
            {
                ListViewItem item = (ListViewItem)parameter;
                this.TasksViewModel.SelectedTask = item.Content as Task;
            }


            if (this.TasksViewModel.SelectedTask.Important == false)
            {
                this.TasksViewModel.SelectedTask.Important = true;

                //if the important list doesn't contain this task, add it
                //if (importantList.Tasks.Contains(this.TasksViewModel.SelectedTask) == false)
                //{
                //    importantList.Tasks.Add(newImportantTask);
                //    db.CreateNewTaskToTaskList(newImportantTask);
                //    //update the task counter on the important list
                //    importantList.TotalCount = importantList.Tasks.Count.ToString();
                //}
            }
            else
            {
                this.TasksViewModel.SelectedTask.Important = false;

                ////if the important list does contain this task, Remove it
                //if (importantList.Tasks.Contains(this.TasksViewModel.SelectedTask) == true)
                //{
                //    importantList.Tasks.Remove(newImportantTask);
                //    db.DeleteSelectedTask(newImportantTask);
                //    //if you are in the important list then also remove from the general tasklist collection
                //    if (this.TasksViewModel.SelectedTasklist == importantList)
                //    {
                //        this.TasksViewModel.TasksList.Remove(this.TasksViewModel.SelectedTask);
                //    }

                //    //update the task counter on the important list
                //    importantList.TotalCount = importantList.Tasks.Count.ToString();
                //}
            }
            db.UpdateSelectedTask(this.TasksViewModel.SelectedTask);

            //db.UpdateSelectedTasklist(importantList);
            //db.UpdateSelectedTasklist(importantList); because we dont do about 3 default list, so just ignore them
        }
    }
}
