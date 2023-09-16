using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Managment.Models;
using MongoDB.Driver;

namespace Task_Managment.DataAccess
{
    public class MemberDataAccessLocal
    {
        #region Singleton
        private static MemberDataAccessLocal _Instance = null;
        public static MemberDataAccessLocal Instance
        {
            get
            {
                if (_Instance == null) _Instance = new MemberDataAccessLocal();
                return _Instance;
            }
        }
        private MemberDataAccessLocal() { }
        private MemberDataAccessLocal(MemberDataAccess dt) { }
        #endregion

        private const string DataAccessKey = "mongodb://localhost:27017";
        private const string MongoDatabase = "Task_Management_Application_DB";
        private const string NotebooksCollection = "Notebooks";
        private const string NotesCollection = "Notes";
        private const string MembersCollection = "Members";

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            MongoClient client = new MongoClient(DataAccessKey);
            IMongoDatabase database = client.GetDatabase(MongoDatabase);
            return database.GetCollection<T>(collection);
        }

        public async Task<List<T>> GetCollection<T>(string collection)
        {
            var _collection = ConnectToMongo<T>(collection);
            var _results = await _collection.FindAsync(_ => true);
            return _results.ToList();
        }

        public List<Members> GetMemberWithEmailAndPassword(string email, string password)
        {
            var _collection = ConnectToMongo<Members>(MembersCollection);

            var _filter = Builders<Members>.Filter.And(
                Builders<Members>.Filter.Eq("_id", email),
                Builders<Members>.Filter.Eq("Password", password));

            var _results = _collection.Find<Members>(_filter);
            return _results.ToList();
        }

        public List<Members> GetMemberWithEmail(string email)
        {
            var _collection = ConnectToMongo<Members>(MembersCollection);

            var _filter = Builders<Members>.Filter.Eq("_id", email);

            var _results = _collection.Find<Members>(_filter);
            return _results.ToList();
        }

        public System.Threading.Tasks.Task ResetMemberPassword(Members resetPassMember)
        {
            var _collection = ConnectToMongo<Members>(MembersCollection);
            var _filter = Builders<Members>.Filter.Eq("_id", resetPassMember.Email);
            return _collection.ReplaceOneAsync(_filter, resetPassMember, new ReplaceOptions { IsUpsert = true });
        }

    }
}
