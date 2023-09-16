using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Task_Managment.Views;
using Task_Managment.ViewModels;
using MongoDB.Driver;
using Task_Managment.Models;
using Task = Task_Managment.Models.Task;

namespace Task_Managment.UserControls
{
    /// <summary>
    public partial class UserControlDays : UserControl
    {
        public static List<string> list = new List<string>();
        
        private void test()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(1);
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender,EventArgs e)
        {
            displayevent();
        }

        public static string static_day,day;
        public UserControlDays()
        {
            InitializeComponent();
        }
        
        public void labelevent(string note){
            lbevent.Text = note;
        }
        public void days(int numday)
        {
            lbdays.Text= numday + "";
        }
        public void dayss(string numday)
        {
            lbdays.Text = numday + "";
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            static_day= lbdays.Text.ToString();
            test();
            eventwindow e_form = new eventwindow();
            e_form.ShowDialog();
        }
        //public List<MyCalendar> GetAllCalendar()
        //{
        //    MongoClient client = new MongoClient("mongodb://localhost:27017");
        //    IMongoDatabase database = client.GetDatabase("Task_Management");
        //    IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
        //    List<MyCalendar> calendarList = collectionCalendar.AsQueryable().ToList<MyCalendar>();
        //    return calendarList;
        //}

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            test();
        }

        public void displayevent()
        {
            //MongoClient client = new MongoClient("mongodb://localhost:27017");
            //IMongoDatabase database = client.GetDatabase("Task_Management");
            //IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
            //List<MyCalendar> calendar = new List<MyCalendar>();
            //calendar = GetAllCalendar();
            if (lbdays.Text != "")
            {
                TaskDataAccess db = new TaskDataAccess();
                List<Task> calendar = new List<Task>();
                calendar = db.GetAllTasksCld();
                if (lbevent.Text != null)
                {
                    lbevent.Text = "";
                }
                foreach (Task myCalendar in calendar)
                {

                    if (CalendarViewModel.static_month.ToString() + "/" + lbdays.Text + "/" + CalendarViewModel.static_year.ToString() == myCalendar.Date.ToString("M/d/yyyy"))
                    {
                        lbevent.Text = myCalendar.Notes;
                    }
                }
            }
        }
    }
}
