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
    public class UserSettingDataAccess
    {
        Members members = MainWindowViewModel.currentUser;
        #region framework
        #region Singleton
        private static UserSettingDataAccess _Instance = null;
        public static UserSettingDataAccess Instance
        {
            get
            {
                if (_Instance == null) _Instance = new UserSettingDataAccess();
                return _Instance;
            }
        }
        public UserSettingDataAccess() { }
        public UserSettingDataAccess(UserSettingDataAccess dt) { }
        #endregion
        

        private const string DataAccessKey = "mongodb+srv://Task_Manager_Team:softintro123456@cluster0.xc1uy.mongodb.net/test";
        private const string DataAccessKeyLocal = "mongodb://localhost:27017";
        private const string MongoDatabase = "Task_Management_Application_DB";
        private const string UserSettingCollection = "UserSetting";
        
        private const string MembersCollection = "Members";

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            if (StartWindowViewModel.mIsUser)
            {
                var client = new MongoClient(DataAccessKey);
                var db = client.GetDatabase(MongoDatabase);
                return db.GetCollection<T>(collection);
            }
            else
            {
                MongoClient client = new MongoClient(DataAccessKeyLocal);
                IMongoDatabase database = client.GetDatabase(MongoDatabase);
                return database.GetCollection<T>(collection);
            }
        }

        public async Task<List<T>> GetCollection<T>(string collection)
        {
            var _collection = ConnectToMongo<T>(collection);
            var _results = await _collection.FindAsync(_ => true);
            return _results.ToList();
        }
        #endregion

        #region get data from MongoDB
        //get list of tasklists (means many tasklists of user)
        public UserSetting GetUserSetting(string email)
        {
            var _collection = ConnectToMongo<UserSetting>(UserSettingCollection);
            var _results = _collection.Find<UserSetting>(c => c.Email == email).Limit(1).FirstOrDefault();
            return _results;
        }

        #endregion

        #region create - update - delete

        
        //create new task to the created TaskList 
        public System.Threading.Tasks.Task CreateNewUserSetting(UserSetting newSetting)
        {
            var _collection = ConnectToMongo<UserSetting>(UserSettingCollection);
            return _collection.InsertOneAsync(newSetting);
        }

        public System.Threading.Tasks.Task UpdateSelectedUserSetting(UserSetting selectedSetting)
        {
            var _collection = ConnectToMongo<UserSetting>(UserSettingCollection);
            var _filter = Builders<UserSetting>.Filter.Eq("Email", selectedSetting.Email);
            return _collection.ReplaceOneAsync(_filter, selectedSetting, new ReplaceOptions { IsUpsert = true });
        }

        public System.Threading.Tasks.Task DeleteSelectedUserSetting(UserSetting selectedSetting)
        {
            var _collection = ConnectToMongo<UserSetting>(UserSettingCollection);
            return _collection.DeleteOneAsync(c => c.Email == selectedSetting.Email);
        }
        #endregion
    }
}
