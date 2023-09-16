using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.ViewModel.Commands;
using Task_Managment.Commands;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Globalization;
using Task_Managment.UserControls;
using Task_Managment.DataAccess;
using System.Collections.ObjectModel;
using MongoDB.Driver;
using Task_Managment.Models;
using Task = Task_Managment.Models.Task;
using Task_Managment.Views;
using Task_Managment.Commands.CalendarCommands;

namespace Task_Managment.ViewModels
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        private string monthyear;
        int month, year;
        public Members members { get; set; }
        public static string date;
        public static int static_month, static_year;
        ObservableCollection<UserControlDays> userControlDays = new ObservableCollection<UserControlDays>();
        public static string static_day;
        public event PropertyChangedEventHandler PropertyChanged;
        private const string ConnectionString = "mongodb+srv://Task_Manager_Team:softintro123456@cluster0.xc1uy.mongodb.net/TestDB?retryWrites=true&w=majority";
        private const string DatabaseName = "Task_Management_Application_DB";
        private const string CalendarCollection = "Calendar";
        public DateTime a;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        List<Task> calendar = new List<Task>();
        List<Tasklist> tasklists = new List<Tasklist>();
        TaskDataAccess db = new TaskDataAccess();
        TaskDataAccessLocal dblc = new TaskDataAccessLocal();
      

        public Itemhandler itemhandler = new Itemhandler();

        public CalendarViewModel()
        {
            InitCommands();
            members = MainWindowViewModel.currentUser;
            GetUserControlDays();
        }
        private void test()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (members.Email != "guest@gmail.com")
            {
                calendar = db.GetAllTasksCld();
                tasklists = db.GetAllTasklistOfMember(members);
            }
            else
            {
                calendar=dblc.GetAllTasksCld();
            }
            for (int i = 0; i < itemhandler.items.Count; i++)
            {
                itemhandler.items[i] = displayevent(itemhandler.items[i]);
                itemhandler.items[i].UpdatePropertyChanged("LabelEvent");
            }

        }
        public ItemCld displayevent(ItemCld cld)
        {
            if (cld.labelevent != null)
            {
                cld.labelevent = "";
            }
            if (members.Email != "guest@gmail.com")
            {
                foreach (Tasklist task in tasklists)
                {
                    foreach (Task myCalendar in calendar)
                    {
                        if (static_month.ToString() + "/" + cld.labelday + "/" + static_year.ToString() == myCalendar.Date.ToString("M/d/yyyy") && task.TasklistID == myCalendar.TasklistID)
                        {
                            cld.labelevent = myCalendar.Notes;
                        }
                    }
                }
            }
            else
            {
                foreach (Task myCalendar in calendar)
                {
                    if (static_month.ToString() + "/" + cld.labelday + "/" + static_year.ToString() == myCalendar.Date.ToString("M/d/yyyy") && myCalendar.TasklistID == null)
                    {
                        cld.labelevent = myCalendar.Notes;
                    }
                }
            }
            return cld;
        }
        public ObservableCollection<ItemCld> items
        {
            get { return itemhandler.items; }
        }

        public class ItemCld : INotifyPropertyChanged
        {
            public string labelday { get; set; }
            public string labelevent { get; set; }
            public ItemCld(string lbday, string lbevent)
            {
                labelday = lbday;
                labelevent = lbevent;
            }
            public string LabelDay
            {
                get
                {
                    return labelday;
                }
                set
                {

                    labelday = value;
                    UpdatePropertyChanged("LabelDay");
                }
            }
            public string LabelEvent
            {
                get
                {
                    return labelevent;
                }
                set
                {

                    labelevent = value;
                    UpdatePropertyChanged("LabelEvent");
                }
            }
            public void UpdatePropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        public class Itemhandler
        {
            public ObservableCollection<ItemCld> items { get; set; }
            public Itemhandler()
            {
                items = new ObservableCollection<ItemCld>();
            }
            public void Add(ItemCld item)
            {
                items.Add(item);
            }
        }
        public DateTime A
        {
            get
            {
                return a;
            }
            set
            {

                a = value;
                OnPropertyChanged("A");
            }
        }
        public string MonthYear
        {
            get {
                return monthyear; 
            }
            set
            {

                monthyear = value;
                OnPropertyChanged("MonthYear");
            }
        }
        public ObservableCollection<UserControlDays> UserControlDays
        {
            get {
                return userControlDays; }
            set
            {
                userControlDays = value;
                OnPropertyChanged("UserControlDays");
            }
        }
        public IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T>(collection);
        }
        public void GetUserControlDays()
        {
            DateTime now = DateTime.Now;
            month = now.Month;
            year = now.Year;
            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            MonthYear = monthname + " " + year.ToString();
            static_month = month;
            static_year = year;
            DateTime startofthemonth = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;
            a = DateTime.Parse(static_month.ToString() + "/" + "1" + "/" + static_year.ToString());
            OnPropertyChanged("A");
            if (members.Email != "guest@gmail.com")
            {
                calendar = db.GetAllTasksCld();
                tasklists = db.GetAllTasklistOfMember(members);
            }
            else
            {
                calendar=dblc.GetAllTasksCld();
            }
            for (int i = 1; i < dayoftheweek; i++)
            {
                itemhandler.Add(new ItemCld("", ""));

            }
            for (int i = 1; i <= days; i++)
            {
                itemhandler.Add(new ItemCld(i.ToString(), ""));
                itemhandler.items[i-1] = displayevent(itemhandler.items[i-1]);
            }
        }
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public ICommand PreviousCM { get; set; }
        public ICommand NextCM { get; set; }
        public ICommand command { get; set; }

        private void InitCommands()
        {
            PreviousCM = new RelayCommand<RichTextBox>(p => true, p => PreviousMonth());
            NextCM = new RelayCommand<RichTextBox>(p => true, p => NextMonth());
            command = new RelayCommand<ListViewItem>(p => true, p => eventwindow1(p));
        }
        private void eventwindow1(ListViewItem p)
        {
            ItemCld b = p.Content as ItemCld;
            date = b.labelday;
            test();
            eventwindow a = new eventwindow();
            a.ShowDialog();
        }
        private void PreviousMonth()
        {
            itemhandler.items.Clear();
            month--;
            if (month <= 0)
            {
                year = year - 1;
                month = 12;
            }
            static_month = month;
            static_year = year;
            DateTime now = DateTime.Now;
            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            MonthYear = monthname + " " + year.ToString();
            DateTime startofthemonth = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;
            a = DateTime.Parse(static_month.ToString() + "/" + "1" + "/" + static_year.ToString());
            OnPropertyChanged("A");
            if (members.Email != "guest@gmail.com")
            {
                calendar = db.GetAllTasksCld();
                tasklists = db.GetAllTasklistOfMember(members);
            }
            else
            {
                calendar = dblc.GetAllTasksCld();
            }
            for (int i = 1; i < dayoftheweek; i++)
            {
                itemhandler.Add(new ItemCld("", ""));
            }
            for (int i = 1; i <= days; i++)
            {
                itemhandler.Add(new ItemCld(i.ToString(), ""));
                itemhandler.items[i - 1] = displayevent(itemhandler.items[i - 1]);
            }
        }
        private void NextMonth()
        {
            itemhandler.items.Clear();
            month++;
            if (month > 12)
            {
                year = year + 1;
                month = 1;
            }
            static_month = month;
            static_year = year;
            DateTime now = DateTime.Now;
            string monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            MonthYear = monthname + " " + year.ToString();
            DateTime startofthemonth = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;
            a = DateTime.Parse(static_month.ToString() + "/" + "1" + "/" + static_year.ToString());
            OnPropertyChanged("A");
            if (members.Email != "guest@gmail.com")
            {
                calendar = db.GetAllTasksCld();
                tasklists = db.GetAllTasklistOfMember(members);
            }
            else
            {
                calendar = dblc.GetAllTasksCld();
            }
            for (int i = 1; i < dayoftheweek; i++)
            {
                itemhandler.Add(new ItemCld("", ""));
            }
            for (int i = 1; i <= days; i++)
            {
                itemhandler.Add(new ItemCld(i.ToString(), ""));
                itemhandler.items[i - 1] = displayevent(itemhandler.items[i - 1]);
            }
        }
    }
}
