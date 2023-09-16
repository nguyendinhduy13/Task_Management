using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Task_Managment.Commands.CalendarCommands;
using Task_Managment.DataAccess;
using Task_Managment.Models;
using Task_Managment.UserControls;
using Task_Managment.Views;
using Task = Task_Managment.Models.Task;

namespace Task_Managment.ViewModels
{
    public class EventWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string textboxday;
        private string textboxevent;
        public Members members { get; set; }
        public bool isDialogOpen { get; set; }

        public CloseDialogCommandCld CloseDialogCommand { get; set; }
        public OpenDiaLogCommandCld OpenDiaLogCommand { get; set; }
        TaskDataAccess db = new TaskDataAccess();
        TaskDataAccessLocal dblc = new TaskDataAccessLocal();
        List<Tasklist> tasks = new List<Tasklist>();
        List<Task> calendar1 = new List<Task>();
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

                    OnPropertyChanged("SelectedClockTime");

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

                    OnPropertyChanged("SelectedCalendarDate");

                    //this.SelectedTask.Expiretime = _selectedTime;
                    //db.UpdateSelectedTask(SelectedTask);
                    //PropertyUpdated("SelectedTime");
                }

            }
        }
        public string TextBoxDay
        {
            get { return textboxday; }
            set
            {
                textboxday = value;
                OnPropertyChanged("TextBoxDay");
            }
        }
        public string TextBoxEvent
        {
            get { return textboxevent; }
            set
            {
                textboxevent = value;
                OnPropertyChanged("TextBoxEvent");
            }
        }
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public EventWindowViewModel() 
        {
            initCommand();
            isDialogOpen = false;
            members = MainWindowViewModel.currentUser;
        }

        public ICommand SaveCM { get; set; }
        public ICommand EditCM { get; set; }
        public ICommand DeleteCM { get; set; }


        private void initCommand()
        {
            SaveCM=new RelayCommand<Task>(p => true, p =>SaveCalendar());
            EditCM = new RelayCommand<Task>(p => true, p => UpdateCalendarAsync());
            DeleteCM = new RelayCommand<Task>(p => true, p => DeleteCalendar());
            this.OpenDiaLogCommand = new OpenDiaLogCommandCld(this);
            this.CloseDialogCommand = new CloseDialogCommandCld(this);
        }

        public List<MyCalendar> GetAllCalendar()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("Task_Management");
            IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
            List<MyCalendar> calendarList = collectionCalendar.AsQueryable().ToList<MyCalendar>();
            return calendarList;
        }

        public void GetCalendar()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("Task_Management");
            IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
            List<MyCalendar> calendar = new List<MyCalendar>();
            calendar = GetAllCalendar();
            foreach (MyCalendar myCalendar in calendar)
            {
                if (myCalendar.Date.ToString("M/d/yyyy") == TextBoxDay)
                {
                    TextBoxEvent = myCalendar.Note;
                }
            }
        }
        private void SaveCalendar()
        {
            if (members.Email != "guest@gmail.com")
            {
                tasks = db.GetAllTask();
                foreach (Tasklist tasklist in tasks)
                {
                    if (tasklist.Name == "Calendar" + members.Email.ToString())
                    {
                        db.CreateNewTaskToTaskList(new Task() { TasklistID = tasklist.TasklistID, Expiretime = _selectedCalendarDate, Date = DateTime.Parse(TextBoxDay).AddDays(1), Notes = TextBoxEvent });
                    }
                }
            }
            else
            {
                //db.CreateNewTaskToTaskList(new Task() { Expiretime = _selectedCalendarDate, Date = DateTime.Parse(TextBoxDay).AddDays(1), Notes = TextBoxEvent });
                dblc.CreateNewTaskToTaskList(new Task() { Expiretime = _selectedCalendarDate, Date = DateTime.Parse(TextBoxDay).AddDays(1), Notes = TextBoxEvent });
            }


            //MongoClient client = new MongoClient("mongodb://localhost:27017");
            //IMongoDatabase database = client.GetDatabase("Task_Management");
            //IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
            //MyCalendar calendar = new MyCalendar(){ Date= DateTime.Parse(TextBoxDay).AddDays(1), Note = TextBoxEvent };
            //collectionCalendar.InsertOne(calendar);
            textboxevent = "";
            OnPropertyChanged("TextBoxEvent");
            
        }
        private void UpdateCalendarAsync()
        {

            if (members.Email != "guest@gmail.com")
            {
                calendar1 = db.GetAllTasksCld();
                tasks = db.GetAllTasklistOfMember(members);
                foreach (Tasklist tasklist in tasks)
                {
                    foreach (Task myCalendar in calendar1)
                    {
                        if (myCalendar.Date.ToString("M/d/yyyy") == (TextBoxDay) && tasklist.TasklistID == myCalendar.TasklistID)
                        {
                            myCalendar.Notes = TextBoxEvent;
                            db.UpdateSelectedTask(myCalendar);
                        }
                    }
                }
            }
            else
            {
                calendar1 = dblc.GetAllTasksCld();
                //calendar1 = db.GetAllTasksCld();
                foreach (Task myCalendar in calendar1)
                {
                    if (myCalendar.Date.ToString("M/d/yyyy") == (TextBoxDay) && myCalendar.TasklistID == null)
                    {
                        myCalendar.Notes = TextBoxEvent;
                        dblc.UpdateSelectedTask(myCalendar);
                    }
                }
            }


            //MongoClient client = new MongoClient("mongodb://localhost:27017");
            //IMongoDatabase database = client.GetDatabase("Task_Management");
            //IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
            //var UpdateDef = Builders<MyCalendar>.Update.Set("Date", DateTime.Parse(TextBoxDay).AddDays(1)).Set("Note", TextBoxEvent);
            //List<MyCalendar> calendar = new List<MyCalendar>();
            //calendar = GetAllCalendar();
            //foreach(MyCalendar myCalendar in calendar)
            //{
            //    if(myCalendar.Date.ToString("M/d/yyyy") == (TextBoxDay))
            //    {
            //        collectionCalendar.UpdateOne(b => b.Id == myCalendar.Id, UpdateDef);
            //    }
            //}
            textboxevent = "";
            OnPropertyChanged("TextBoxEvent");
        } 
        private void DeleteCalendar()
        {


            if (members.Email != "guest@gmail.com")
            {
                calendar1 = db.GetAllTasksCld();
                tasks = db.GetAllTasklistOfMember(members);
                foreach (Tasklist tasklist in tasks)
                {
                    foreach (Task myCalendar in calendar1)
                    {
                        if (myCalendar.Date.ToString("M/d/yyyy") == (TextBoxDay) && tasklist.TasklistID == myCalendar.TasklistID)
                        {
                            db.DeleteSelectedTask(myCalendar);
                        }
                    }
                }
            }
            else
            {
                calendar1=dblc.GetAllTasksCld();
                //calendar1 = db.GetAllTasksCld();
                foreach (Task myCalendar in calendar1)
                {
                    if (myCalendar.Date.ToString("M/d/yyyy") == (TextBoxDay) && myCalendar.TasklistID == null)
                    {
                        dblc.DeleteSelectedTask(myCalendar);
                    }
                }
            }

            //MongoClient client = new MongoClient("mongodb://localhost:27017");
            //IMongoDatabase database = client.GetDatabase("Task_Management");
            //List<MyCalendar> calendar = new List<MyCalendar>();
            //calendar = GetAllCalendar();
            //IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
            //foreach (MyCalendar myCalendar in calendar)
            //{
            //    if (myCalendar.Date.ToString("M/d/yyyy") == (TextBoxDay))
            //    {
            //        collectionCalendar.DeleteOne(b => b.Id == myCalendar.Id);
            //    }
            //}
            textboxevent = "";
            OnPropertyChanged("TextBoxEvent");
        }
    }
}
