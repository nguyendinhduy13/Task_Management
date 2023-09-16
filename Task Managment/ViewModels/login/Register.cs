using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;
using Task_Managment.Models;

using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace Task_Managment.ViewModels
{
    public class Register
    {
        private const string ConnectionString = "mongodb+srv://Task_Manager_Team:softintro123456@cluster0.xc1uy.mongodb.net/TestDB?retryWrites=true&w=majority";
        private const string DatabaseName = "Task_Management_Application_DB";
        private const string MembersCollection = "Members";
        private const string MailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        public string connectionString { get; }
        public string databaseName { get; }
        public string membersCollection { get; }
        public string mailPattern { get; }

        public IMongoCollection<T> ConnectToMongo<T>(string collection)
        {
            // Configurate MongoDB Cloud Connection
            var settings = MongoClientSettings.FromConnectionString(ConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(DatabaseName);

            return database.GetCollection<T>(collection);
        }

        // Checkking mail pattern
        public void isValidatePatternCheck(string email, ref bool isValid)
        {
            isValid = Regex.IsMatch(email, MailPattern);
            
            if (!isValid)
            System.Windows.MessageBox.Show("Email insufficient!!");
        }

        // Checking existed mail
        public void isExistedCheck(string email, ref bool isValid)
        {
            var membersCollection = ConnectToMongo<Members>(MembersCollection);
            var matchedEmails = membersCollection.Find<Members>(mail => mail.Email == email);

            isValid = matchedEmails.ToList().Count() == 0;

            if (!isValid)
                System.Windows.MessageBox.Show("Email has already been used!!");
        }

        // Verify password
        public void isPasswordValid(string pass, string passVerify, ref bool isValid)
        {
            isValid = pass == passVerify;

            if (!isValid)
                System.Windows.MessageBox.Show("Passwords unmatch!!");
        }

        public string GenerateVerficationCode()
        {
            Random rand = new Random();
            string verificationCode = "";
            for (int i = 1; i <= 6; i++)
                verificationCode += rand.Next(0, 9);

            return verificationCode;
        }


        // Send validation code email
        public void SendVerificationCode(string verificationCode, string toAddress)
        {
            // Setting mail addresses
            MailAddress FromAddress = new MailAddress("20520682@gm.uit.edu.vn");
            MailAddress ToAddress = new MailAddress(toAddress);

            // Composing mail content
            MailMessage mail_msg = new MailMessage(FromAddress, ToAddress);
            mail_msg.Subject = "VERIFICATION CODE!!!";
            mail_msg.Body = "Your verification code is: " + verificationCode;

            // Configurating network credential
            NetworkCredential Network_Credential = new NetworkCredential(FromAddress.Address, "shhcfjfpctmuctmu");

            // Send mail
            SmtpClient smtp_client = new SmtpClient();
            smtp_client.Host = "smtp.gmail.com";
            smtp_client.Port = 587;
            smtp_client.EnableSsl = true;
            smtp_client.Credentials = Network_Credential;
            smtp_client.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp_client.Send(mail_msg);
        }

        // Add new member
        public async void AddNewMember(string email, string username, string password)
        {
            var membersCollection = ConnectToMongo<Members>(MembersCollection);
            
            var newMember = new Members(email, username, password);
            
            await membersCollection.InsertOneAsync(newMember).ContinueWith(_ =>
            {
                System.Windows.MessageBox.Show("Successfully added new membership!!!");
            });
        }
    }
}
