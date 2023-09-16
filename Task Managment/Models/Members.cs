using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Task_Managment.DataAccess;

namespace Task_Managment.Models
{
    public class Members
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool isGuest { get; set; }

         UserSettingDataAccess db = UserSettingDataAccess.Instance;
        public UserSetting Setting { get; set; }

        public Members(string email, string username, string password)
        {
            Email = email;
            UserName = username;
            Password = password;
            Setting = db.GetUserSetting(email);
            //Setting = new UserSetting(email);
            // db.CreateNewUserSetting(Setting);
        }

        public Members(Members refObj)
        {
            Email = refObj.Email;
            UserName = refObj.UserName;
            Password = refObj.Password;
            Setting = db.GetUserSetting(Email);
        }
    }
}
