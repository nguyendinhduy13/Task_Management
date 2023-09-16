using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;
using Task_Managment.Models;
using Task_Managment.ViewModel.Commands;
using Task_Managment.Commands;
using Task_Managment.Commands.TaskCommands;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Task_Managment.Stores;
using TrayIcon.Services;
using System.Windows.Forms;
using MaterialDesignThemes.Wpf;
using Task_Managment.DataAccess;

namespace Task_Managment.ViewModels
{
    public class TasksViewModel : INotifyPropertyChanged, IDisposable
    {
        public Members _currentUser { get; set; }
        private TaskDataAccess db = TaskDataAccess.Instance;
        private UserSettingDataAccess db1 = UserSettingDataAccess.Instance;
        //!Fields
        public static readonly string ImagesPath = Path.GetFullPath("imagesForWpf\\TaskResource\\iconForTasks\\").Replace("\\bin\\Debug\\", "\\");
        //!Properties
        #region TasklistList 
        public ObservableCollection<Tasklist> TasklistsList { get; set; }

        public ObservableCollection<Task> TasksLists { get; set; }

        public ObservableCollection<Subtask> Subtasks { get; set; }

        public Tasklist DefaultMyDayList { get; set; }

        public Tasklist DefaultImportantList { get; set; }
        public Tasklist DefaultTasksList { get; set; }

        public Tasklist CalendarTaskList { get; set; }

        #endregion

        #region ImageList & UserSetting
        public ObservableCollection<TaskIcon> IconTaskList { get; set; }
        public ObservableCollection<TaskIcon> BackgroundList { get; set; }

        public ImageSource background { get; set; }

        #endregion

        #region datetime handling

        public bool isDialogOpen { get; set; }

        private DateTime _selectedClockTime;

        public DateTime SelectedClockTime
        {
            get => _selectedClockTime;
            set
            {
                if (value != null)
                {
                    _selectedClockTime = value;
                    //DateTime temp = _selectedTime;
                    //temp = temp.AddHours(_selectedClockTime.Hour - temp.Hour);
                    //temp = temp.AddMinutes(_selectedClockTime.Minute - temp.Minute);
                    //temp = temp.AddSeconds(0 - temp.Second);
                    //if (SelectedTask != null)
                    //    this.SelectedTime = temp;

                    PropertyUpdated("SelectedClockTime");

                }

            }
        }

        private DateTime _selectedCalendarDate;

        public DateTime SelectedCalendarDate
        {
            get => _selectedCalendarDate;
            set
            {
                if (value != null)
                {
                    _selectedCalendarDate = value;
                    //DateTime temp = _selectedTime;
                    //temp = temp.AddDays(double.Parse(_selectedCalendarDate.Day.ToString()) - double.Parse(temp.Day.ToString()));
                    //temp = temp.AddMonths(int.Parse(_selectedCalendarDate.Month.ToString()) - int.Parse(temp.Month.ToString()));
                    //temp = temp.AddYears(int.Parse(_selectedCalendarDate.Year.ToString()) - int.Parse(temp.Year.ToString()));

                    //if (SelectedTask != null)
                    //    this.SelectedTime = temp;

                    PropertyUpdated("SelectedCalendarDate");

                    //this.SelectedTask.Expiretime = _selectedTime;
                    //db.UpdateSelectedTask(SelectedTask);
                    //PropertyUpdated("SelectedTime");
                }

            }
        }

        private DateTime _selectedTime;

        public DateTime SelectedTime
        {
            get => _selectedTime;
            set
            {
                if (value != null)
                {
                    _selectedTime = value;
                    _selectedTime = _selectedTime.ToLocalTime();
                    _selectedTask.Expiretime = _selectedTime;
                    this.SelectedTask.Expiretime = _selectedTime;
                    this.SelectedTask.IsNotifify = false;
                    db.UpdateSelectedTask(SelectedTask);

                    this.SelectedTask.TimerStore.Start((int)(TimeSpan.FromTicks(SelectedTask.Expiretime.Ticks).TotalSeconds - TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds));
                    PropertyUpdated("SelectedTime");
                }

            }
        }

        #endregion

        #region duration and timestore


        private int _duration;
        public int Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;

            }
        }



        #endregion

        #region view selected item
        private Tasklist _selectedTasklist;
        public Tasklist SelectedTasklist
        {
            get { return _selectedTasklist; }
            set
            {
                _selectedTasklist = value;

                this.TasksLists.Clear();
                if (SelectedTasklist != null)
                {
                    if (SelectedTasklist.Tasks != null)
                    {
                        if (SelectedTasklist.Tasks.Count > 0)
                        {
                            this.SelectedTask = null;
                            this.SubtasksPaneVisible = !this.SubtasksPaneVisible;

                            foreach (Task task in this.SelectedTasklist.Tasks)
                            {
                                this.TasksLists.Add(task);
                            }
                        }
                    }
                }
                PropertyUpdated("SelectedTasklist");
                this.SubtasksPaneVisible = false;
            }
        }

        private Task _selectedTask;
        public Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                if (_selectedTask == null)
                {
                    _selectedTask = value;

                }
                else if (_selectedTask != value)
                {
                    _selectedTask = value;

                }
                if (_selectedTask != null)
                    if (_selectedTask.Expiretime != null)
                        _selectedTime = _selectedTask.Expiretime.ToLocalTime();
                    else _selectedTime = DateTime.Now;

                PropertyUpdated("SelectedTime");
            }
        }

        private Tasklist _selectedSubtask;
        public Tasklist SelectedSubtask
        {
            get { return _selectedSubtask; }
            set
            {
                _selectedSubtask = value;

                PropertyUpdated("SelectedSubtask");
            }
        }

        private string _addaTaskText;
        public string AddaTaskText
        {
            get { return _addaTaskText; }
            set
            {
                _addaTaskText = value;
                PropertyUpdated("AddaTaskText");
            }
        }

        private bool _isTasklistRenaming;
        public bool IsTasklistRenaming
        {
            get { return _isTasklistRenaming; }
            set
            {
                _isTasklistRenaming = value;
                PropertyUpdated("IsTasklistRenaming");
            }
        }

        private bool _isTaskRenaming;
        public bool IsTaskRenaming
        {
            get { return _isTaskRenaming; }
            set
            {
                _isTaskRenaming = value;
                PropertyUpdated("IsTaskRenaming");
            }
        }

        private bool _subtasksPaneVisible;
        public bool SubtasksPaneVisible
        {
            get { return _subtasksPaneVisible; }
            set
            {
                _subtasksPaneVisible = value;
                PropertyUpdated("SubtasksPaneVisible");
            }
        }

        private bool _iconPaneVisible;
        public bool IconPaneVisible
        {
            get { return _iconPaneVisible; }
            set
            {
                _iconPaneVisible = value;
                PropertyUpdated("IconPaneVisible");
            }
        }

        private bool _morePaneVisible;
        public bool MorePaneVisible
        {
            get { return _morePaneVisible; }
            set
            {
                _morePaneVisible = value;
                PropertyUpdated("MorePaneVisible");
            }
        }
        #endregion

        #region commmands
        public NewTasklistCommand NewTasklistCommand { get; set; }
        public NewTaskCommand NewTaskCommand { get; set; }
        public NewSubtaskCommand NewSubtaskCommand { get; set; }

        public StartRenameCommand StartRenameCommand { get; set; }
        public EndRenameCommand EndRenameCommand { get; set; }

        public DeleteCommand DeleteCommand { get; set; }

        public MarkImportantCommand MarkImportantCommand { get; set; }

        public CloseSubtaskPanelCommand CloseSubtaskPanelCommand { get; set; }

        public SelectTaskCommand SelectTaskCommand { get; set; }

        public PickTaskIconCommand PickTaskIconCommand { get; set; }

        public PickTaskThemeCommand PickTaskThemeCommand { get; set; }

        public NotifyCommand NotifyCommand { get; set; }
        public StartCommand StartCommand { get; set; }

        public CloseDialogCommand CloseDialogCommand { get; set; }

        public OpenDiaLogCommand OpenDiaLogCommand { get; set; }

        //!Events
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        //!Ctor
        #region constructors
        public TasksViewModel()
        {
            //init commands
            init(MainWindowViewModel.currentUser);

        }


        public TasksViewModel(Members currentUser)
        {
            //init commands
            init(currentUser);

        }
        #endregion

        //!Methods

        #region init
        public void init(Members currentUser)
        {
            _currentUser = currentUser;

            isDialogOpen = false;
            _duration = 10;


            //get all the tasks of tripledefaultTasklists
            InitUserTasklist();
            this.TasksLists = new ObservableCollection<Task>();
            this.SelectedTasklist = this.DefaultImportantList;
            this.Subtasks = new ObservableCollection<Subtask>();

            InitCommand();
            InitIconAndBackground();

            InitClockAndCalendar();

            //begin to countdown for notification
            foreach (Tasklist temp in this.TasklistsList)
            {
                foreach (Task task in temp.Tasks)
                {
                    task.TimerStore = new TimerStore((new NotifyIconNotificationService(MainWindowViewModel.NotifyIconInstance)), task);

                    if (task.IsNotifify)
                    {
                        continue;
                    }
                    int durationTemp = (int)(TimeSpan.FromTicks(task.Expiretime.Ticks).TotalSeconds - TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds);
                    if (durationTemp > 0)
                    {
                        task.TimerStore.Start(durationTemp);
                    }
                    else
                    {
                        task.IsNotifify = true;
                        db.UpdateSelectedTask(task);
                        MainWindowViewModel.NotifyIconInstance.ShowBalloonTip(3000, task.Name, task.Expiretime.ToLocalTime().ToString(), ToolTipIcon.Info);

                    }

                }
            }

        }
        public void InitClockAndCalendar()
        {
            SelectedCalendarDate = DateTime.Now;
            SelectedClockTime = DateTime.Now;
            SelectedClockTime = SelectedClockTime.AddMinutes(1);
        }

        public void InitUserTasklist()
        {
            if (this.TasklistsList != null)
                this.TasklistsList.Clear();
            this.TasklistsList = new ObservableCollection<Tasklist>();

            List<Tasklist> tempList = db.GetAllTasklistOfMember(_currentUser);
            if (tempList.Count > 0)
            {
                this.TasklistsList = new ObservableCollection<Tasklist>();
                foreach (Tasklist temp in tempList) // lấy những tasklist như myday, importtant, untitledlist
                {

                    {
                        this.TasklistsList.Add(temp); // sau đó add từng tasklist vào
                    }



                }
                for (int i = 0; i < this.TasklistsList.Count; i++) // duyệt từng tasklist ở trong  this.TasklistsList (tức tổng số tasklist dc lưu ở local bây giờ)
                {

                    this.TasklistsList[i].Tasks = db.GetAllTasksFromTasklist(this.TasklistsList[i]); // lấy cái task ở trong từng tasklist đó * tưởng tự chỗ này !!!!

                    for (int j = 0; j < this.TasklistsList[i].Tasks.Count; j++)
                    {
                        this.TasklistsList[i].Tasks[j].Subtasks = db.GetAllSubTasksFromTask(this.TasklistsList[i].Tasks[j]); // get subtasks
                    }
                }


                for (int j = 0; j < this.TasklistsList[1].Tasks.Count; j++)
                {
                    if (this.TasklistsList[1].Tasks[j].Expiretime == DateTime.UtcNow)
                    {
                        Task tempTask = new Task(this.TasklistsList[1].Tasks[j]);
                        tempTask.Important = true;
                        this.TasklistsList[0].Tasks.Add(tempTask);// get subtasks

                    }
                }

                this.CalendarTaskList = this.TasklistsList[1]; // cung muon lam cho nay ghe :((
                this.TasklistsList.RemoveAt(1);
                this.DefaultMyDayList = this.TasklistsList[0];
                this.DefaultImportantList = this.TasklistsList[1];
                this.DefaultTasksList = this.TasklistsList[2];


            }
            else
            {
                DefaultMyDayList = new Tasklist() { Name = "My Day", IconName = "day.png", MemberId = _currentUser.Email };
                DefaultImportantList = new Tasklist() { Name = "Important", IconName = "important.png", MemberId = _currentUser.Email };
                DefaultTasksList = new Tasklist() { Name = "Tasks", IconName = "greenery.png", MemberId = _currentUser.Email };
                CalendarTaskList = new Tasklist() { Name = "Calendar" + _currentUser.Email, IconName = "greenery.png", MemberId = _currentUser.Email };

                db.CreateNewTasklist(DefaultMyDayList);
                db.CreateNewTasklist(DefaultImportantList);
                db.CreateNewTasklist(DefaultTasksList);
                db.CreateNewTasklist(CalendarTaskList);
                this.TasklistsList = new ObservableCollection<Tasklist>()
            {
                this.DefaultMyDayList,
                this.DefaultImportantList,
                this.DefaultTasksList
            };

            }

        }

        private void InitIconAndBackground()
        {
            IconTaskList = new ObservableCollection<TaskIcon>();
            IconTaskList.Clear();

            BackgroundList = new ObservableCollection<TaskIcon>();
            BackgroundList.Clear();

            string[] icon = {

                "baseball.png",

                "basketball.png",

                "call.png",

                "car.png",

                "christmas.png",

                "church.png",

                "clothes.png",

                "computer.png",

                "confetti.png",

                "day.png",

                "defaultTask.png",

                "earth.png",

                "food.png",

                "fuel.png",

                "game.png",

                "gift.png",

                "greenery.png",

                "hospital.png",

                "important.png",

                "like.png",

                "love.png",

                "mom.png",

                "muscle.png",

                "music.png",

                "party.png",

                "plan.png",

                "school.png",

                "shopping.png",

                "soccer.png",

                "sunday.png",

                "sunflower.png",

                "support.png"

        };

            foreach (string temp in icon)
            {
                IconTaskList.Add(new TaskIcon(temp));
            }

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
            if(db1.GetUserSetting(_currentUser.Email) == null)
            {
                _currentUser.Setting = new UserSetting();
                _currentUser.Setting.taskBackground = "img_background.png";
                _currentUser.Setting.homeViewBackground = "img4_background.png";
                db1.CreateNewUserSetting(_currentUser.Setting);

            }
            background = new BitmapImage(new Uri((ImagesPath + _currentUser.Setting.taskBackground)));

        }

        private void InitCommand()
        {
            this.NewTasklistCommand = new NewTasklistCommand(this);
            this.NewTaskCommand = new NewTaskCommand(this);
            this.NewSubtaskCommand = new NewSubtaskCommand(this);

            this.StartRenameCommand = new StartRenameCommand(this);
            this.EndRenameCommand = new EndRenameCommand(this);

            this.DeleteCommand = new DeleteCommand(this);

            this.MarkImportantCommand = new MarkImportantCommand(this);

            this.CloseSubtaskPanelCommand = new CloseSubtaskPanelCommand(this);

            this.SelectTaskCommand = new SelectTaskCommand(this);

            this.PickTaskIconCommand = new PickTaskIconCommand(this);

            this.PickTaskThemeCommand = new PickTaskThemeCommand(this);

            this.NotifyCommand = new NotifyCommand(this);

            this.StartCommand = new StartCommand(this);

            this.OpenDiaLogCommand = new OpenDiaLogCommand(this);

            this.CloseDialogCommand = new CloseDialogCommand(this);

        }

        #endregion

        #region utilities
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        public void PropertyUpdated(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            //_timerStore.RemainingSecondsChanged -= TimerStore_RemainingSecondsChanged;
        }

        #endregion
    }
}