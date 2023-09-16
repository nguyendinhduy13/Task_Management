using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Task_Managment.Models
{
    public class Subtask
    {
        //!Fields
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        private string _subtaskID;
        public string SubtaskID
        {
            get { return _subtaskID; }
            set
            {
                _subtaskID = value;
                PropertyUpdated("TaskID");
            }
        }

        public string MemberID { get; set; }

        public string TaskID { get; set; }


        //!Properties
        public string Name { get; set; }


        public bool Completed { get; set; }

        //!Events
        public event PropertyChangedEventHandler PropertyChanged;
        //!Ctor
        public Subtask(string parentTaskID)
        {
            //this.SubtaskID = Guid.NewGuid().ToString();
            this.TaskID = parentTaskID;
        }

        //!Methods
        public void PropertyUpdated(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
