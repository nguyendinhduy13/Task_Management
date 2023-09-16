using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Task_Managment.Models;
using Task_Managment.ViewModels;

namespace Task_Managment.ViewModel.Commands
{
    public class NewSubtaskCommand : ICommand
    {
        //!Fields
        private TaskDataAccess db = TaskDataAccess.Instance;
        //!Properties
        public TasksViewModel TasksViewModel { get; set; }

        //!Events
        public event EventHandler CanExecuteChanged;

        //!Ctor
        public NewSubtaskCommand(TasksViewModel tasksViewModel)
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
            //parameter is the TasksViewModel object

            //create a new string subtask
            Subtask newSubtask = new Subtask(this.TasksViewModel.SelectedTask.TaskID)
            {
                Name = ""
            };

            //add the string to the observable collection
            this.TasksViewModel.Subtasks.Add(newSubtask);

            //add the string to the subtasks list
            this.TasksViewModel.SelectedTask.Subtasks.Add(newSubtask);

            db.CreateNewSubTaskToTaskList(newSubtask);
        }
    }
}
