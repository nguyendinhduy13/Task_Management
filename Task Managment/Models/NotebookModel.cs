using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace Task_Managment.Models
{
    public class NotebookModel : INotifyPropertyChanged
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string _ownerId { get; set; }
        public string _name { get; set; }
        public List<Note> _collection { get; set; }
        public DateTime _createdDate { get; set; }

        public DateTime _lastUpdateDate { get; set; }
        public NotebookModel()
        {
            _ownerId = "";
            _name = "";
            _collection = new List<Note>();
            _createdDate = DateTime.UtcNow;
            _lastUpdateDate = DateTime.UtcNow;
        }
        public NotebookModel(string ownerId, string name)
        {
            _ownerId = ownerId;
            _name = name;
            _collection = new List<Note>();
            _createdDate = DateTime.UtcNow;
            _lastUpdateDate = DateTime.UtcNow;

        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
