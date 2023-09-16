using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.ViewModels;

namespace Task_Managment.Models
{
    public class TaskDataAccess 
    {
        Members members = MainWindowViewModel.currentUser;
        #region framework
        #region Singleton
        private static TaskDataAccess _Instance = null;
        public static TaskDataAccess Instance
        {
            get
            {
                if (_Instance == null) _Instance = new TaskDataAccess();
                return _Instance;
            }
        }
        public TaskDataAccess() { }
        public TaskDataAccess(TaskDataAccess dt) { }
        #endregion

        private const string DataAccessKey = "mongodb+srv://Task_Manager_Team:softintro123456@cluster0.xc1uy.mongodb.net/test";
        private const string DataAccessKeyLocal = "mongodb://localhost:27017";
        private const string MongoDatabase = "Task_Management_Application_DB";
        private const string TasklistsCollection = "Tasklists";
        private const string TasksCollection = "Tasks";
        private const string SubtasksCollection = "SubTasks";
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
        public List<Tasklist> GetAllTasklistOfMember(Members currentMember)
        {
            var _collection = ConnectToMongo<Tasklist>(TasklistsCollection);
            var _results = _collection.Find<Tasklist>(c => c.MemberId == currentMember.Email);
            return _results.ToList();
        }

        //get list of subtasks (means many tasks of that tasklists of that user)
        public List<Task> GetAllTasksFromTasklist(Tasklist selectedTasklist)
        {
            var _collection = ConnectToMongo<Task>(TasksCollection);
            var _results = _collection.Find<Task>(
                c => c.TasklistID == selectedTasklist.TasklistID
            );
            return _results.ToList();
        }
        public List<Tasklist> GetAllTask()
        {
            var _collection = ConnectToMongo<Tasklist>(TasklistsCollection);
            var _results = _collection.Find<Tasklist>(
                c => true
            );
            return _results.ToList();
        }
        public List<Task> GetAllTasksFromDate(DateTime dateTime)
        {
            var _collection = ConnectToMongo<Task>(TasksCollection);
            var _results = _collection.Find<Task>(
                c => c.Date== dateTime
            );
            return _results.ToList();
        }
        public List<Task> GetAllTasksCld()
        {
            var _collection = ConnectToMongo<Task>(TasksCollection);
            var _results = _collection.Find<Task>(
                c=>true);
            return _results.ToList();
        }

        //get list of subtasks (means many subtasks of that task)
        public List<Subtask> GetAllSubTasksFromTask(Task selectedTask)
        {
            var _collection = ConnectToMongo<Subtask>(SubtasksCollection);
            var _results = _collection.Find<Subtask>(
                c => c.TaskID == selectedTask.TaskID
            );
            return _results.ToList();
        }
        #endregion

        #region create - update - delete

        #region Tasklist
        //create new task to the created TaskList 
        public System.Threading.Tasks.Task CreateNewTasklist(Tasklist newTasklist)
        {
            var _collection = ConnectToMongo<Tasklist>(TasklistsCollection);
            return _collection.InsertOneAsync(newTasklist);
        }
        public System.Threading.Tasks.Task UpdateSelectedTasklist(Tasklist selectedTaskist)
        {
            var _collection = ConnectToMongo<Tasklist>(TasklistsCollection);
            var _filter = Builders<Tasklist>.Filter.Eq("TasklistID", selectedTaskist.TasklistID);
            return _collection.ReplaceOneAsync(_filter, selectedTaskist, new ReplaceOptions { IsUpsert = true });
        }

        public System.Threading.Tasks.Task DeleteSelectedTasklist(Tasklist selectedTaskist)
        {
            var _collection = ConnectToMongo<Tasklist>(TasklistsCollection);
            return _collection.DeleteOneAsync(c => c.TasklistID == selectedTaskist.TasklistID);
        }
        #endregion

        #region Task

        //create new task to the created TaskList 
        public System.Threading.Tasks.Task CreateNewTaskToTaskList(Task newTask)
        {
            var _collection = ConnectToMongo<Task>(TasksCollection);
            return _collection.InsertOneAsync(newTask);
        }
      
        public System.Threading.Tasks.Task UpdateSelectedTask(Task selectedTask)
        {
            var _collection = ConnectToMongo<Task>(TasksCollection);
            var _filter = Builders<Task>.Filter.Eq("TaskID", selectedTask.TaskID);
            return _collection.ReplaceOneAsync(_filter, selectedTask, new ReplaceOptions { IsUpsert = true });
        }
        public System.Threading.Tasks.Task DeleteSelectedTask(Task selectedTask)
        {
            var _collection = ConnectToMongo<Task>(TasksCollection);
            return _collection.DeleteOneAsync(c => c.TaskID == selectedTask.TaskID);
        }
        #endregion

        #region Subtask
        //create new subtask to the created TaskList 
        public System.Threading.Tasks.Task CreateNewSubTaskToTaskList(Subtask newSubTask)
        {
            var _collection = ConnectToMongo<Subtask>(SubtasksCollection);
            return _collection.InsertOneAsync(newSubTask);
        }

        public System.Threading.Tasks.Task UpdateSelectedSubTask(Subtask selectedSubTask)
        {
            var _collection = ConnectToMongo<Subtask>(SubtasksCollection);
            var _filter = Builders<Subtask>.Filter.Eq("SubtaskID", selectedSubTask.SubtaskID);
            return _collection.ReplaceOneAsync(_filter, selectedSubTask, new ReplaceOptions { IsUpsert = true });
        }

        public System.Threading.Tasks.Task DeleteSelectedSubTask(Subtask selectedSubTask)
        {
            var _collection = ConnectToMongo<Subtask>(SubtasksCollection);
            return _collection.DeleteOneAsync(c => c.SubtaskID == selectedSubTask.SubtaskID);
        }
        #endregion

        #endregion
    }
}
