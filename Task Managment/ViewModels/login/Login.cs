using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;
using Task_Managment.Models;
using Task_Managment.Views;

namespace Task_Managment.ViewModels
{
    internal class Login
    {
        private const string ConnectionString = "mongodb+srv://Task_Manager_Team:softintro123456@cluster0.xc1uy.mongodb.net/TestDB?retryWrites=true&w=majority";
        private const string DatabaseName = "TestDB";
        private const string MembersCollection = "Members";

        public string connectionString { get; }
        public string databaseName { get; }
        public string membersCollection { get; }

        public IMongoCollection<T> ConnectToMongo<T>(string collection)
        {
            // Configurate MongoDB Cloud Connection
            var settings = MongoClientSettings.FromConnectionString(ConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(DatabaseName);

            return database.GetCollection<T>(collection);
        }

        public async void Log_in(object sender, string email, string password)
        {
                var memberList = await Get_Accounts(email, password);
                if (memberList.Count > 0 && memberList[0].Password == password)
                {
                    MainWindow newWindow = new MainWindow();
                    //wndLogin _this = sender as wndLogin;
                    //_this.Hide(); 
                    newWindow.ShowDialog();
                    //_this.Show();
                }
                else
                {
                    System.Windows.MessageBox.Show("Wrong email or password!");
                }
        }

        public async Task<List<Members>> Get_Accounts(string email, string password)
        {
            var membersCollection = ConnectToMongo<Members>(MembersCollection);
            try
            {
                var matchedEmails = membersCollection.Find<Members>(mail => mail.Email == email);
                return matchedEmails.ToList();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
