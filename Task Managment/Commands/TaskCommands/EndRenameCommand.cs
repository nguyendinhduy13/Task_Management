using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Task_Managment.Models;
using Task_Managment.ViewModels;

namespace Task_Managment.ViewModel.Commands
{
    public class EndRenameCommand : ICommand
    {
        private TaskDataAccess db = TaskDataAccess.Instance;
        //!Properties
        public TasksViewModel TasksViewModel { get; set; }

        public HomeViewModel HomeViewModel { get; set; }

        //!Events
        public event EventHandler CanExecuteChanged;

        //!Ctor
        public EndRenameCommand(TasksViewModel tasksViewModel)
        {
            this.TasksViewModel = tasksViewModel;
        }

        public EndRenameCommand(HomeViewModel tasksViewModel)
        {
            this.HomeViewModel = tasksViewModel;
        }

        //!Methods
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //parameter is the selectedtasklist (not required because we've also bound the textbox to the tasklist name
            if (parameter is Task)
            {
                Task currrentTask = parameter as Task;
                db.UpdateSelectedTask(currrentTask);

            }
            else
            {
                if(parameter is Tasklist)
                {
                    db.UpdateSelectedTasklist(this.TasksViewModel.SelectedTasklist);
                }
                else
                {
                    ListViewItem item = (ListViewItem)parameter;

                    switch (item.Content.GetType().ToString().Substring(22))
                    {
                        case "Subtask":
                            Subtask currrentSubtasklist = item.Content as Subtask;
                            db.UpdateSelectedSubTask(currrentSubtasklist);
                            break;
                        case "Tasklist":
                            Tasklist currrentTasklist = item.Content as Tasklist;
                            db.UpdateSelectedTasklist(currrentTasklist);
                            break;
                    }
                }
            
            }

            this.TasksViewModel.IsTasklistRenaming = false;
            return;
            //All I need to do is set this to false

        }
    }
}
