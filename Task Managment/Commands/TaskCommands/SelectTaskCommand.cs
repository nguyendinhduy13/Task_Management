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
    public class SelectTaskCommand : ICommand
    {

        //!Fields

        //!Properties
        public TasksViewModel TasksViewModel { get; set; }

        //!Events
        public event EventHandler CanExecuteChanged;

        //!Ctor
        public SelectTaskCommand(TasksViewModel tasksViewModel)
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
            if (parameter == null && TasksViewModel.SelectedTask != null)
            {
                if (TasksViewModel.SelectedTasklist != null)
                {
                    if (TasksViewModel.SelectedTasklist.Tasks != null)
                    {
                        if (TasksViewModel.SelectedTasklist.Tasks.Count > 0)
                        {
                            if (TasksViewModel.SelectedTask != null)
                            {

                                this.TasksViewModel.SubtasksPaneVisible = !this.TasksViewModel.SubtasksPaneVisible;
                                this.TasksViewModel.Subtasks.Clear();
                                if (TasksViewModel.SelectedTask.Subtasks != null)
                                {
                                    if (TasksViewModel.SelectedTask.Subtasks.Count > 0)
                                    {
                                        foreach (Subtask subTask in this.TasksViewModel.SelectedTask.Subtasks)
                                        {
                                            this.TasksViewModel.Subtasks.Add(subTask);
                                        }
                                    }
                                }
                            }
                            else this.TasksViewModel.SubtasksPaneVisible = false;
                        }
                    }
                }

            }
            if (parameter != TasksViewModel.SelectedTask && this.TasksViewModel.SubtasksPaneVisible == true)
            {
                if (TasksViewModel.SelectedTasklist != null)
                {
                    if (TasksViewModel.SelectedTasklist.Tasks != null)
                    {
                        if (TasksViewModel.SelectedTasklist.Tasks.Count > 0)
                        {
                            if (TasksViewModel.SelectedTask != null)
                            {

                                this.TasksViewModel.Subtasks.Clear();
                                if (TasksViewModel.SelectedTask.Subtasks != null)
                                {
                                    if (TasksViewModel.SelectedTask.Subtasks.Count > 0)
                                    {
                                        foreach (Subtask subTask in this.TasksViewModel.SelectedTask.Subtasks)
                                        {
                                            this.TasksViewModel.Subtasks.Add(subTask);
                                        }
                                    }
                                }
                            }
                            else this.TasksViewModel.SubtasksPaneVisible = false;
                        }
                    }
                }

            }
            else
            {
                // check and revert the panel status
                Task task = (Task)parameter;

                if (TasksViewModel.SelectedTasklist != null)
                {
                    if (TasksViewModel.SelectedTasklist.Tasks != null)
                    {
                        if (TasksViewModel.SelectedTasklist.Tasks.Count > 0)
                        {
                            if (TasksViewModel.SelectedTask != null)
                            {

                                this.TasksViewModel.SubtasksPaneVisible = !this.TasksViewModel.SubtasksPaneVisible;
                                this.TasksViewModel.Subtasks.Clear();
                                if (TasksViewModel.SelectedTask.Subtasks != null)
                                {
                                    if (TasksViewModel.SelectedTask.Subtasks.Count > 0)
                                    {
                                        foreach (Subtask subTask in this.TasksViewModel.SelectedTask.Subtasks)
                                        {
                                            this.TasksViewModel.Subtasks.Add(subTask);
                                        }
                                    }
                                }
                            }
                            else this.TasksViewModel.SubtasksPaneVisible = false;
                        }
                    }
                }


            }

            if (!this.TasksViewModel.SubtasksPaneVisible)
            {
                this.TasksViewModel.SelectedTask = null;
            }
            TasksViewModel.PropertyUpdated("SelectedTask");
            return;
        }

        ////create a new string subtask
        //Subtask newSubtask = new Subtask(this.TasksViewModel.SelectedTask.TaskID)
        //{
        //    Name = ""
        //};

        ////add the string to the observable collection
        //this.TasksViewModel.Subtasks.Add(newSubtask);

        ////add the string to the subtasks list
        //this.TasksViewModel.SelectedTask.Subtasks.Add(newSubtask);
    }
}