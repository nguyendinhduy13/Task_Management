using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Task_Managment.Models
{
    public class TaskIcon : INotifyPropertyChanged
    {
        public static readonly string ImagesPath = Path.GetFullPath("imagesForWpf\\TaskResource\\iconForTasks\\").Replace("\\bin\\Debug\\", "\\");

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyUpdated("Name");
            }
        }

        private string _fullPath;

        public event PropertyChangedEventHandler PropertyChanged;

        public string FullPath
        {
            get { return _fullPath; }
            set
            {
                _fullPath = value;
                PropertyUpdated("Name");
            }
        }

        public ImageSource i { get; set; }

        public TaskIcon(string name)
        {
            Name = Path.GetFileName(name );
            FullPath = ImagesPath+Name;
            i = new BitmapImage(new Uri((FullPath)));
        }



        //!Methods
        public void PropertyUpdated(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
