using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Task_Managment.Models;
using Task_Managment.Views;
using TrayIcon.Services;
using Task_Managment.Stores;
using System.Windows;
using MessageBox = System.Windows.MessageBox;
using System.Reflection;

namespace Task_Managment.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public static Members currentUser { get; set; }

        public static readonly string ImagesPath = Path.GetFullPath("imagesForWpf").Replace("\\bin\\Debug\\", "\\");
        public ICommand openNoteViewCommand { get; set; }
        public ICommand openCalendarCommand { get; set; }
        public ICommand openTaskViewCommand { get; set; }
        public ICommand openHomeViewCommand { get; set; }
        public ICommand openNotebookViewCommand { get; set; }
        public ICommand onCloseCommand { get; set; }
        public ICommand onMinimizeCommand { get; set; }
        public ICommand logOutCommand { get; set; }

        public Action CloseAction { get; set; }
        public Action HideAction { get; set; }
        public Action ShowAction { get; set; }

        private static NotifyIcon _notifyIconInstance = null;
        public static NotifyIcon NotifyIconInstance
        {
            get
            {
                if (_notifyIconInstance == null) _notifyIconInstance = new NotifyIcon();
                return _notifyIconInstance;
            }
        }



        private Uri _frameSource;
        public Uri FrameSource
        {
            get { return _frameSource; }

            set
            {
                _frameSource = value;

                PropertyUpdated("FrameSource");
            }
        }

        public MainWindowViewModel()
        {
            StartWindowViewModel wdvm = new StartWindowViewModel();

            if (true/*wdvm.IsUserLoggedIn()*/)
            {
                // currentUser = new Members("phatlam1811@gmail.com", "phatlam1811", "123");
                currentUser = new Members(wdvm.getCurrentUser());
            }
            else
                currentUser = new Members("guest@gmail.com", "Guest", "123");


            _notifyIconInstance = MainWindowViewModel.NotifyIconInstance;
            Uri iconUri = new Uri("pack://application:,,,/app.ico", UriKind.RelativeOrAbsolute);
            //string temp = ImagesPath + "/app.ico";
            if(!_notifyIconInstance.Visible)
            {
                _notifyIconInstance.Icon = new Icon(Path.Combine(System.Environment.CurrentDirectory.Replace("\\bin\\Debug", "\\imagesForWpf\\"), "app.ico"));
            }
            



            _notifyIconInstance.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            _notifyIconInstance.ContextMenuStrip.Items.Add("Open");
            _notifyIconInstance.ContextMenuStrip.Items.Add(currentUser.UserName);
            _notifyIconInstance.Visible = true;
            _notifyIconInstance.BalloonTipClicked += NotifyIcon_BalloonTipClicked;
            _notifyIconInstance.Click += _notifyIconInstance_Click;
            init(currentUser);
        }

    

        private void _notifyIconInstance_Click(object sender, EventArgs e)
        {
            App.Current.Windows[0].Show();
            App.Current.Windows[0].WindowState = WindowState.Normal;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            App.Current.MainWindow.ShowDialog();
            App.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void init(Members currentUser)
        {
            FrameSource = new Uri("/Views/MainHomeView.xaml", UriKind.Relative);
            openNoteViewCommand = new RelayCommand<Frame>(p => true, p => OpenNoteView());
            openTaskViewCommand = new RelayCommand<Frame>(p => true, p => OpenTaskView());
            openHomeViewCommand = new RelayCommand<Frame>(p => true, p => OpenHomeView());
            openCalendarCommand = new RelayCommand<Frame>(p => true, p => OpenCalendarView());
            openNotebookViewCommand = new RelayCommand<Frame>(p => true, p => OpenNotebookView());
            logOutCommand = new RelayCommand<System.Windows.Controls.Button>(p => true, p => logOut());
            onCloseCommand = new RelayCommand<Window>(p => true, p => Dispose(p));
            onMinimizeCommand = new RelayCommand<Window>(p => true, p => OnMinimize(p));
        }

        private void logOut()
        {
            StartWindowViewModels startWindow = new StartWindowViewModels();
            CloseAction.Invoke();
            startWindow.Show();
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            MessageBox.Show("Application is running.", "Status", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #region button methods
        private void OpenCalendarView()
        {
            FrameSource = new Uri("/Views/Calendar.xaml", UriKind.Relative);
        }

        private void OpenNoteView()
        {
            FrameSource = new Uri("/Views/pNoteHomeView.xaml", UriKind.Relative);
        }

        private void OpenNotebookView()
        {
            FrameSource = new Uri("/Views/pNotebookHomeView.xaml", UriKind.Relative);
        }

        private void OpenTaskView()
        {
            FrameSource = new Uri("/Views/TaskHomeView.xaml", UriKind.Relative);
        }

        private void OpenHomeView()
        {
            FrameSource = new Uri("/Views/MainHomeView.xaml", UriKind.Relative);
        }
        #endregion
        public MainWindowViewModel(Members members)
        {
            init(members);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void PropertyUpdated(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void OnMinimize(Window p)
        {
            if (p.WindowState == WindowState.Minimized)
                p.Hide();
            else if (p.WindowState == WindowState.Normal)
            {

            }
        }

        public void Dispose(Window p)
        {
            _notifyIconInstance.Dispose();
        }
    }
}

/*       *
}*/
