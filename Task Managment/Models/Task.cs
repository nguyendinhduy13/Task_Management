using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Task_Managment.Stores;
using Task_Managment.ViewModels;
using TrayIcon.Services;

namespace Task_Managment.Models
{
    public class Task : INotifyPropertyChanged
    {
        //!Fields
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        private string _taskID;
        public string TaskID {
            get { return _taskID; }
            set 
            {
                _taskID = value;
                PropertyUpdated("TaskID");
            }
        }

        public string TasklistID { get; set; }

        public string MemberId { get; set; }

        //!Properties
        public DateTime Date { get; set; }

        public DateTime Expiretime { get; set; }

        public bool IsNotifify { get; set; }

        public  TimerStore TimerStore;

        public string Name { get; set; }

        public bool Completed { get; set; }

        private bool _important;

        public bool Important
        {
            get { return _important; }
            set
            {
                _important = value;
                PropertyUpdated("Important");
            }
        }

        public string Notes { get; set; }

        public List<Subtask> Subtasks { get; set; }
       

        //!Events
        public event PropertyChangedEventHandler PropertyChanged;

        //!Ctor
        public Task(string tasklistId)
        {
           
            this.TasklistID = tasklistId;   
            this.Subtasks = new List<Subtask>();

            this.Date = DateTime.Now; // utc datetime format
           
            this.Expiretime = DateTime.Now;
            this.IsNotifify = true;
            this.TimerStore = new TimerStore((new NotifyIconNotificationService(MainWindowViewModel.NotifyIconInstance)),this);
                       
        }

        public Task(Task taskTemp)
        {
           
            this.TasklistID = taskTemp.TasklistID;
            this.Subtasks = taskTemp.Subtasks;
            this.Completed = taskTemp.Completed;
            this.Name = taskTemp.Name;
            this.Notes = taskTemp.Notes;
            this.Date = taskTemp.Date;
            this.Important = taskTemp.Important;
            this.Expiretime= taskTemp.Expiretime;
        }

        public Task()
        {
        }

        //!Methods
        public void PropertyUpdated(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
