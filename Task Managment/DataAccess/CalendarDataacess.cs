using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task_Managment.Models;
using System.Threading.Tasks;
using Task_Managment.ViewModels;
using Task_Managment.UserControls;

namespace Task_Managment.DataAccess
{
    public class CalendarDataaccess
    {
        private const string ConnectionString = "mongodb+srv://Task_Manager_Team:softintro123456@cluster0.xc1uy.mongodb.net/TestDB?retryWrites=true&w=majority";
        private const string DatabaseName = "Task_Management_Application_DB";
        private const string CalendarCollection = "Calendar";

        public IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T>(collection);
        }
        //public async Task<List<MyCalendar>> GetAllCalendar()
        //{
        //    var calendarCollection = ConnectToMongo<MyCalendar>(CalendarCollection);
        //    var results = await calendarCollection.FindAsync(_ => true);
        //    return results.ToList();
        //}
        public List<MyCalendar> GetAllCalendar()
        {
            var _collection = ConnectToMongo<MyCalendar>(CalendarCollection);
            var _results = _collection.Find<MyCalendar>(_=>true);
            return _results.ToList();
        }

        public System.Threading.Tasks.Task SelectCalendar()
        {
            var calendarCollection = ConnectToMongo<MyCalendar>(CalendarCollection);
            var filter = Builders<MyCalendar>.Filter.Eq("Date", CalendarViewModel.static_month + "/" + UserControlDays.static_day + "/" + CalendarViewModel.static_year);
            return calendarCollection.FindAsync(filter);
        }

            public System.Threading.Tasks.Task CreateCalendar(MyCalendar calendar)
        {
            var calendarCollection = ConnectToMongo<MyCalendar>(CalendarCollection);
            return calendarCollection.InsertOneAsync(calendar);
        }

        public System.Threading.Tasks.Task UpdateCalendar(MyCalendar calendar)
        {
            var calendarCollection = ConnectToMongo<MyCalendar>(CalendarCollection);
            var filter = Builders<MyCalendar>.Filter.Eq("Id", calendar.Id);
            return calendarCollection.ReplaceOneAsync(filter, calendar, new ReplaceOptions { IsUpsert = true });
        }

        public System.Threading.Tasks.Task DeleteCalendar(MyCalendar calendar)
        {
            var calendarCollection = ConnectToMongo<MyCalendar>(CalendarCollection);
            return calendarCollection.DeleteOneAsync(c => c.Id == calendar.Id);
        }

        public async Task<List<MyCalendar>> GetCalendar(DateTime date)
        {
            var calendarCollection = ConnectToMongo<MyCalendar>(CalendarCollection);
            var results = await calendarCollection.FindAsync(x=>x.Date==date);
            return results.ToList();
        }
    }
}
