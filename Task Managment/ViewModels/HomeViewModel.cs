using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Task_Managment.DataAccess;
using Task_Managment.Models;
using Task_Managment.ViewModel.Commands;

namespace Task_Managment.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public Members _currentUser { get; set; }
        private TaskDataAccess db = TaskDataAccess.Instance;
        public static readonly string ImagesPath = Path.GetFullPath("imagesForWpf\\TaskResource\\iconForTasks\\").Replace("\\bin\\Debug\\", "\\");
        private UserSettingDataAccess db1 = UserSettingDataAccess.Instance;
        #region ImageList & UserSetting
        public ObservableCollection<TaskIcon> IconTaskList { get; set; }
        public ObservableCollection<TaskIcon> BackgroundList { get; set; }

        public ImageSource background { get; set; }

        #endregion

        public EndRenameCommand endRenameCommand;



        private string _selectedScratchPad;

        public event PropertyChangedEventHandler PropertyChanged;

        public string SelectedScratchPad
        {
            get => _selectedScratchPad;
            set
            {
                if (value != null)
                {
                    _selectedScratchPad = value;
                    PropertyUpdated("SelectedScratchPad");
                }
            }
        }

        public void PropertyUpdated(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HomeViewModel()
        {
            //init commands
            StartWindowViewModel startWindowViewModel = new StartWindowViewModel();
            Members currentUser = startWindowViewModel.getCurrentUser();
            init(currentUser);

        }

        public void init(Members currentUser)
        {
            _currentUser = currentUser;

            //get all the tasks of tripledefaultTasklists

            InitCommand();
            InitIconAndBackground();


        }
        private void InitIconAndBackground()
        {
            IconTaskList = new ObservableCollection<TaskIcon>();
            IconTaskList.Clear();

            BackgroundList = new ObservableCollection<TaskIcon>();
            BackgroundList.Clear();

            string[] backgroundOptions =
            {
                "\\imagesForWpf\\TaskResource\\iconForTasks\\img_background.png",
                "\\imagesForWpf\\TaskResource\\iconForTasks\\img2_background.png",
                "\\imagesForWpf\\TaskResource\\iconForTasks\\img3_background.png",
                "\\imagesForWpf\\TaskResource\\iconForTasks\\img4_background.png",
                "\\imagesForWpf\\TaskResource\\iconForTasks\\img5_background.png",
                "\\imagesForWpf\\TaskResource\\iconForTasks\\img6_background.png",
                "\\imagesForWpf\\TaskResource\\iconForTasks\\img7_background.png",
                "\\imagesForWpf\\TaskResource\\iconForTasks\\img8_background.png",
            };

            foreach (string temp in backgroundOptions)
            {
                BackgroundList.Add(new TaskIcon(temp));
            }
            _currentUser.Setting = db1.GetUserSetting(_currentUser.Email);


            if (_currentUser.Setting == null || _currentUser.Setting.homeViewBackground == "")

            {
                _currentUser.Setting = new UserSetting();
                _currentUser.Setting.taskBackground = "img_background.png";
                _currentUser.Setting.homeViewBackground = "img4_background.png";
                db1.CreateNewUserSetting(_currentUser.Setting);

            }
            background = new BitmapImage(new Uri((ImagesPath + _currentUser.Setting.homeViewBackground)));
        }

        private void InitCommand()
        {
            endRenameCommand = new EndRenameCommand(this);
        }
    }
}
