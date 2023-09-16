using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Task_Managment.Models;

namespace Task_Managment.ViewModels
{
    public class DataAccess
    {
        #region framework
        #region Singleton
        private static DataAccess _Instance = null;
        public static DataAccess Instance
        {
            get
            {
                if (_Instance == null) _Instance = new DataAccess();
                return _Instance;
            }
        }
        private DataAccess() { }
        private DataAccess(DataAccess dt) { }
        #endregion

         private const string DataAccessKey = "mongodb+srv://Task_Manager_Team:softintro123456@cluster0.xc1uy.mongodb.net/test";
        private const string MongoDatabase = "Task_Management_Application_DB";
        private const string DataAccessKeyLocal = "mongodb://localhost:27017";
        private const string NotebooksCollection = "Notebooks";
        private const string NotesCollection = "Notes";
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

        #region Notes_Data_Access
        public List<Note> GetAllNotesOfMember(Members currentMember)
        {
            var _collection = ConnectToMongo<Note>(NotesCollection);
            var _results = _collection.Find<Note>(c => c.MemberId == currentMember.Email);
            return _results.ToList();
        }

        public List<Note> GetAllNotesFromNotebook(NotebookModel selectedNotebook)
        {
            var _collection = ConnectToMongo<Note>(NotesCollection);
            var _results = _collection.Find<Note>(c => c.NotebookId == selectedNotebook._id);
            return _results.ToList();
        }

        public System.Threading.Tasks.Task CreateNewNote(Note newNote)
        {
            var _collection = ConnectToMongo<Note>(NotesCollection);
            return _collection.InsertOneAsync(newNote);
        }

        public System.Threading.Tasks.Task UpdateSelectedNote(Note selectedNote)
        {
            var _collection = ConnectToMongo<Note>(NotesCollection);
            var _filter = Builders<Note>.Filter.Eq("_id", selectedNote.Id);
            return _collection.ReplaceOneAsync(_filter, selectedNote, new ReplaceOptions { IsUpsert = true });
        }

        public System.Threading.Tasks.Task DeleteSelectedNote(Note selectedNote)
        {
            var _collection = ConnectToMongo<Note>(NotesCollection);
            return _collection.DeleteOneAsync(c => c.Id == selectedNote.Id);
        }
        #endregion

        #region Notebooks_Data_Access
        public List<NotebookModel> GetAllNotebooksOfMember(Members currentMember)
        {
            var _collection = ConnectToMongo<NotebookModel>(NotebooksCollection);
            var _results = _collection.Find<NotebookModel>(c => c._ownerId == currentMember.Email);
            return _results.ToList();
        }


        public System.Threading.Tasks.Task CreateNewNotebook(NotebookModel newNote)
        {
            var _collection = ConnectToMongo<NotebookModel>(NotebooksCollection);
            return _collection.InsertOneAsync(newNote);
        }

        public System.Threading.Tasks.Task UpdateSelectedNotebook(NotebookModel selectedNote)
        {
            var _collection = ConnectToMongo<NotebookModel>(NotebooksCollection);
            var _filter = Builders<NotebookModel>.Filter.Eq("_id", selectedNote._id);
            return _collection.ReplaceOneAsync(_filter, selectedNote, new ReplaceOptions { IsUpsert = true });
        }

        public System.Threading.Tasks.Task DeleteSelectedNotebook(NotebookModel selectedNote)
        {
            var _collection = ConnectToMongo<NotebookModel>(NotebooksCollection);
            return _collection.DeleteOneAsync(c => c._id == selectedNote._id);
        }
        #endregion
    }
}
