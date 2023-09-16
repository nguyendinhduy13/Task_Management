using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Windows.Documents;
using System.Windows.Markup;
using System.ComponentModel;

namespace Task_Managment.Models
{
    public class Note : INotifyPropertyChanged
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        private string _id;
        private string _memberId;
        private string _notebookId;
        private string _title;
        private string _description;
        private string _xamlFormat;
        private DateTime? _createdDate;
        private DateTime? _lastUpdatedDate;
        private bool _isImportant;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string MemberId
        {
            get { return _memberId; }
            set { _memberId = value; }
        }

        public string NotebookId
        {
            get { return _notebookId; }
            set { _notebookId = value; }
        }

        public string Title
        {
            get { return _title; }
            set 
            { 
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string Description
        {
            get { return _description; }
            set 
            { 
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public string XamlFormat
        {
            get { return _xamlFormat; }
            set
            {
                _xamlFormat = value;

                GetDescription(_xamlFormat);

                OnPropertyChanged("XamlFormat");
            }
        }

        public DateTime? CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public DateTime? LastUpdatedDate
        {
            get { return _lastUpdatedDate; }
            set { _lastUpdatedDate = value; }
        }

        public bool IsImportant
        {
            get { return _isImportant; }
            set { _isImportant = value; }
        }

        public Note(string memberId, string notebookId = "")
        {
            _memberId = memberId;
            _notebookId = notebookId;
            _title = "Untitled";
            _description = "";
            _xamlFormat = "<FlowDocument PagePadding=\"5,0,5,0\" AllowDrop=\"True\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph Foreground=\"#FF000000\"></Paragraph></FlowDocument>";
            _isImportant = false;
            _createdDate = DateTime.Now;
            _lastUpdatedDate = DateTime.Now;
        }

        private void GetDescription(string xamlFormatCopy)
        {
            string imageContainerString = "";
            string imageStart = "";
            string imageEnd = "";
            int index = xamlFormatCopy.IndexOf("<Paragraph><Image");

            if (index > 0)
            {
                imageStart = "<Paragraph><Image";
                imageEnd = "</Paragraph>";
            }
            else
            {
                index = xamlFormatCopy.IndexOf("<Image");
                imageStart = "<Image";
                imageEnd = "</Image>";
            }

            while (index > 0)
            {
                imageContainerString = imageStart + getBetween(xamlFormatCopy, imageStart, imageEnd) + imageEnd;
                xamlFormatCopy = xamlFormatCopy.Remove(index, imageContainerString.Length);
                index = xamlFormatCopy.IndexOf(imageStart);
            }

            FlowDocument doc = (FlowDocument)XamlReader.Parse(xamlFormatCopy);

            if (doc != null)
            {
                TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);
                Description = range.Text;
            }
        }

        private string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
