using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using Task_Managment.Models;
using Task_Managment.ViewModels;
using Image = System.Windows.Controls.Image;
using Task_Managment.DataAccess;
using System.IO;
using System.Windows.Media.Imaging;

namespace Task_Managment.Commands.TaskCommands
{

    public class PickTaskThemeCommand : ICommand
    {


        //!Fields
        private TaskDataAccess db = TaskDataAccess.Instance; // 1.
        private UserSettingDataAccess usDB = UserSettingDataAccess.Instance;
        public static readonly string ImagesPath = Path.GetFullPath("imagesForWpf\\TaskResource\\iconForTasks\\").Replace("\\bin\\Debug\\", "\\");

        //!Properties
        TasksViewModel TasksViewModel { get; set; }

        //!Events
        public event EventHandler CanExecuteChanged;


        //!Ctor
        public PickTaskThemeCommand(TasksViewModel tasksViewModel)
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

            if (parameter is Image)
            {
                Image image = (Image)parameter;
                this.TasksViewModel._currentUser.Setting.taskBackground = Path.GetFileName(image.Source.ToString());
                usDB.UpdateSelectedUserSetting(this.TasksViewModel._currentUser.Setting);

                this.TasksViewModel.background = new BitmapImage(new Uri((ImagesPath + this.TasksViewModel._currentUser.Setting.taskBackground)));

                this.TasksViewModel.PropertyUpdated("background");

            }




        }
    }

}
