using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using Task_Managment.ViewModels;

namespace Task_Managment.Models
{
    public class Tasklist : INotifyPropertyChanged
    {
        //!Fields
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        private string _tasklistID;

        public string TasklistID
        {
            get { return _tasklistID; }

            set
            {
                _tasklistID = value;
                PropertyUpdated("TasklistID");
            }
        }
        //{
        //    get { return _tasklistID; }
        //    set
        //    {
        //        _tasklistID = value;
        //        PropertyUpdated("TasklistID");
        //    }
        //}

        public string MemberId { get; set; }
        public static readonly string ImagesPath = Path.GetFullPath("imagesForWpf\\TaskResource\\iconForTasks\\").Replace("\\bin\\Debug\\", "\\");

        public static readonly string DefaultIcon = "defaultTask.png";

        //!Properties


        public List<Task> Tasks { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyUpdated("Name");
            }
        }

        public Uri IconSource
        {
            get { return new Uri(ImagesPath + IconName); }

        }
        public string IconName { get; set; }
        private string _totalCount;
        public string TotalCount
        {
            get
            {
                if (Tasks.Count > 0)
                {
                    return Tasks.Count.ToString();
                }
                else return "";
            }
            set
            {
                _totalCount = Tasks.Count.ToString();
                PropertyUpdated("TotalCount");
            }
        }

        //!Ctor
        public Tasklist()
        {
            //this.TasklistID = Guid.NewGuid().ToString();

            this.Name = "Untitled list";
            this.Tasks = new List<Task>();
            this.IconName = DefaultIcon;
            //this.IconSource = new Uri(Path.Combine(ImagesPath, DefaultIcon));
        }



        public Tasklist(List<Task> _tasks, Members _member)
        {
            //this.TasklistID = Guid.NewGuid().ToString();
            MemberId = _member.Email;

            this.Name = "Untitled list";
            this.Tasks = _tasks;
            this.IconName = DefaultIcon;
        }

        //!Events
        public event PropertyChangedEventHandler PropertyChanged;

        //!Methods
        public void PropertyUpdated(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
