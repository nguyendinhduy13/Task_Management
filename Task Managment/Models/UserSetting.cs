using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Managment.Models
{
    public class UserSetting
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Email { get; set; }

        public static readonly string ImagesPath = Path.GetFullPath("imagesForWpf\\TaskResource\\iconForTasks\\").Replace("\\bin\\Debug\\", "\\");

        public static readonly string DefaultIcon = "img4_background.png";

        public string scratchPad = "";
        public string taskBackground { get; set; }

        public string homeViewBackground { get; set; }

        public UserSetting()
        {
            Email = "";
            this.taskBackground = DefaultIcon;
            this.homeViewBackground = "";
        }

        public UserSetting(string email)
        {
            Email = email;
            this.taskBackground = DefaultIcon;
            this.homeViewBackground = "";
        }

        public UserSetting(string email, string taskBackground, string homeViewBackground)
        {
            Email = email;
            this.taskBackground = taskBackground;
            this.homeViewBackground = homeViewBackground;
        }
    }
}
