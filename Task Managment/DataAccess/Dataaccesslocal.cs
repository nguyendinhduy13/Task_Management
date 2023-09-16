using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.Models;
using Task_Managment.ViewModels;

namespace Task_Managment.DataAccess
{
    public class Dataaccesslocal
    {
        
        MongoClient client = new MongoClient("mongodb://localhost:27017");
        Members members;
        public Dataaccesslocal()
        {
            StartWindowViewModel startWindow = new StartWindowViewModel();
            members = startWindow.getCurrentUser();
        }

        //public List<MyCalendar> GetAllCalendar()
        //{
        //    if (members.isGuest)
        //    {
        //        IMongoDatabase database = client.GetDatabase("Task_Management");
        //        IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
        //        List<MyCalendar> calendarList = collectionCalendar.AsQueryable().ToList<MyCalendar>();
        //        return calendarList;
        //    }
        //    else
        //    {

        //    }
        public List<MyCalendar> GetAllCalendar()
        {

            IMongoDatabase database = client.GetDatabase("Task_Management");
            IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
            List<MyCalendar> calendarList = collectionCalendar.AsQueryable().ToList<MyCalendar>();
            return calendarList;

        }
        public void AddCalendar(MyCalendar calendar)
        {
            IMongoDatabase database = client.GetDatabase("Task_Management");
            IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
            collectionCalendar.InsertOne(calendar);
        }
        public void UpdateCalendar(MyCalendar calendar)
        {
            IMongoDatabase database = client.GetDatabase("Task_Management");
            IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
            var UpdateDef = Builders<MyCalendar>.Update.Set("Date", calendar.Date.AddDays(1)).Set("Note", calendar.Note);
            collectionCalendar.UpdateOne(b => b.Id == calendar.Id, UpdateDef);
        }
        public void DeleteCalendar(MyCalendar calendar)
        {
            IMongoDatabase database = client.GetDatabase("Task_Management");
            IMongoCollection<MyCalendar> collectionCalendar = database.GetCollection<MyCalendar>("Calendar");
            collectionCalendar.DeleteOne(b => b.Id == calendar.Id);
        }
    }
}
