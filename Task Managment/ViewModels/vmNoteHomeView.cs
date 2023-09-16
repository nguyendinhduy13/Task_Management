using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Task_Managment.Models;

namespace Task_Managment.ViewModels
{
    public class vmNoteHomeView : INotifyPropertyChanged
    {
        #region Fields & Properties
        private DataAccess db = DataAccess.Instance;
        private Note _selectedNote;
        private string _notesCount;
        private bool _isTitleSorted;
        private bool _isCreatedDateSorted;
        private bool _isLastUpdatedDateSorted;
        private string _searchBoxContent;
        private CollectionViewSource cvsNotes;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Note> _notes { get; set; }
        public double[] _fontSizes { get; set; }
        public double _selectedFontSize { get; set; }
        public FontFamily _selectedFontFamily { get; set; }
        public Note SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                OnPropertyChanged("SelectedNote");
            }
        }
        public string NotesCount
        {
            get { return _notesCount; }
            set 
            { 
                _notesCount = value;
                OnPropertyChanged("NotesCount");
            }
        }
        public Members _currentUser { get; set; }
        public bool IsTitleSorted
        {
            get { return _isTitleSorted; }
            set
            {
                _isTitleSorted = value;
                OnPropertyChanged("IsTitleSorted");
            }
        }
        public bool IsCreatedDateSorted
        {
            get { return _isCreatedDateSorted; }
            set
            {
                _isCreatedDateSorted = value;
                OnPropertyChanged("IsCreatedDateSorted");
            }
        }
        public bool IsLastUpdatedDateSorted
        {
            get { return _isLastUpdatedDateSorted; }
            set
            {
                _isLastUpdatedDateSorted = value;
                OnPropertyChanged("IsLastUpdatedDateSorted");
            }
        }
        public string SearchBoxContent
        {
            get { return _searchBoxContent; }
            set
            {
                _searchBoxContent = value;
                cvsNotes.View.Refresh();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchBoxContent"));
            }
        }

        public ICollectionView AllNotes
        {
            get { return cvsNotes.View; }
        }
        #endregion

        public vmNoteHomeView()
        {
            Initialize(MainWindowViewModel.currentUser);
        }

        public vmNoteHomeView(Members currentUser)
        {
            
            Initialize(MainWindowViewModel.currentUser);
        }

        public vmNoteHomeView(Members currentUser, NotebookModel currentNotebook)
        {
           
            Initialize(MainWindowViewModel.currentUser);
        }

        #region Commands
        public ICommand CreateNewNoteCmd { get; set; }
        public ICommand DeleteSelectedNoteCmd { get; set; }
        public ICommand SaveSelectedNoteCmd { get; set; }
        public ICommand InsertPictureCmd { get; set; }
        public ICommand SortByTitleCmd { get; set; }
        public ICommand SortByCreatedDateCmd { get; set; }
        public ICommand SortByLastUpdatedDateCmd { get; set; }
        public ICommand EditSelectedTextSizeCmd { get; set; }
        public ICommand EditSelectedTextFontFamilyCmd { get; set; }
        #endregion

        #region Functions
        private void Initialize(Members currentUsers)
        {
            IsTitleSorted = false;  
            IsCreatedDateSorted = false;
            IsLastUpdatedDateSorted = false;

            _fontSizes = new double[16] {8.0, 9.0, 10.0, 11.0, 12.0, 14.0, 16.0, 18.0, 20.0, 22.0, 24.0, 26.0, 28.0, 36.0, 48.0, 72.0};
            _selectedFontSize = _fontSizes[4];

            _currentUser = currentUsers;

            _notes = new ObservableCollection<Note>(db.GetAllNotesOfMember(currentUsers));
            SelectedNote = _notes.Count > 0 ? _notes[0] : null;
            NotesCount = _notes.Count.ToString();

            cvsNotes = new CollectionViewSource();
            cvsNotes.Source = _notes;
            cvsNotes.Filter += CvsNotes_Filter;

            InitCommands();
        }

        private void InitCommands()
        {
            CreateNewNoteCmd = new RelayCommand<Note>(p => true, p => CreateNewNote());
            DeleteSelectedNoteCmd = new RelayCommand<Note>(p => p != null, p => DeleteSelectedNote(p));
            SaveSelectedNoteCmd = new RelayCommand<Note>(p => p != null, p => SaveSelectedNote(p));
            InsertPictureCmd = new RelayCommand<RichTextBox>(p => p != null, p => InsertPicture(p));
            SortByTitleCmd = new RelayCommand<ListView>(p => true, p => SortByTitle(p));
            SortByCreatedDateCmd = new RelayCommand<ListView>(p => true, p => SortByCreatedDate(p));
            SortByLastUpdatedDateCmd = new RelayCommand<ListView>(p => true, p => SortByLastUpdatedDate(p));
            EditSelectedTextSizeCmd = new RelayCommand<RichTextBox>(p => p != null, p => EditSelectedTextSize(p));
            EditSelectedTextFontFamilyCmd = new RelayCommand<RichTextBox>(p => p != null, p => EditSelectedTextFontFamily(p));
        }

        private void EditSelectedTextSize(RichTextBox p)
        {
            TextSelection textSelection = p.Selection;

            if (_selectedFontSize <= 0.0)
            {
                MessageBox.Show("Please enter valid font size!!"); return;
            }

            if (textSelection != null)
            {
                p.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, _selectedFontSize);
            }
            else
            {
                p.Document.FontSize = _selectedFontSize;
            }
            p.Focus();
        }

        private void EditSelectedTextFontFamily(RichTextBox p)
        {
            TextSelection textSelection = p.Selection;

            if (textSelection != null)
            {
                p.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, _selectedFontFamily);
                p.Focus();
            }
        }

        private void SortByCreatedDate(ListView p)
        {
            if (p.Items.SortDescriptions.Count != 0 &&
                p.Items.SortDescriptions.Last().PropertyName == "CreatedDate" &&
                p.Items.SortDescriptions.Last().Direction == ListSortDirection.Ascending)
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("CreatedDate", ListSortDirection.Descending));
            }
            else
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("CreatedDate", ListSortDirection.Ascending));
            }

            IsCreatedDateSorted = !IsCreatedDateSorted;
            IsTitleSorted = IsLastUpdatedDateSorted = false;
        }

        private void SortByLastUpdatedDate(ListView p)
        {
            if (p.Items.SortDescriptions.Count != 0 &&
                p.Items.SortDescriptions.Last().PropertyName == "LastUpdatedDate" &&
                p.Items.SortDescriptions.Last().Direction == ListSortDirection.Ascending)
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("LastUpdatedDate", ListSortDirection.Descending));
            }
            else
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("LastUpdatedDate", ListSortDirection.Ascending));
            }

            IsLastUpdatedDateSorted = !IsLastUpdatedDateSorted;
            IsTitleSorted = IsCreatedDateSorted = false;
        }

        private void SortByTitle(ListView p)
        {
            if (p.Items.SortDescriptions.Count != 0 && 
                p.Items.SortDescriptions.Last().PropertyName == "Title" && 
                p.Items.SortDescriptions.Last().Direction == ListSortDirection.Ascending)
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Descending));
            }
            else
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            }

            IsTitleSorted = !IsTitleSorted;
            IsCreatedDateSorted = IsLastUpdatedDateSorted = false;
        }

        private void CvsNotes_Filter(object sender, FilterEventArgs e)
        {
            Note note = (Note)e.Item;

            if (string.IsNullOrWhiteSpace(_searchBoxContent) || _searchBoxContent.Length == 0)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = note.Title.IndexOf(_searchBoxContent, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        private void InsertPicture(RichTextBox p)
        {
            OpenFileDialog o = new OpenFileDialog()
            {
                Title = "Find As",
                Filter = ".jpg|*.jpg;*.jpeg|.png|*.png|.bmp|*.bmp|.gif|*.gif|.tiff|*.tif;*.tiff|"
                    + "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff"
            };

            if (o.ShowDialog() == true)
            {
                Paragraph paragraph = new Paragraph();
                
                Image image = new Image();
                image.Width = 100;
                image.Height = 100;
                image.Stretch = System.Windows.Media.Stretch.UniformToFill;

                try
                {
                    byte[] imageBinary = File.ReadAllBytes(o.FileName);
                    string base64String = Convert.ToBase64String(imageBinary);

                    BitmapImage bmImage = new BitmapImage(new Uri(o.FileName, UriKind.Relative)) { CacheOption = BitmapCacheOption.OnLoad };

                    image.Tag = base64String;
                    image.Source = bmImage;

                    //AdornerLayer al = AdornerLayer.GetAdornerLayer(image);
                    //if (al != null)
                    //    al.Add(new ResizingAdorner(image));

                    paragraph.Inlines.Add(image);

                    p.Document.Blocks.Add(paragraph);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SaveSelectedNote(Note selectedNote)
        {
            try
            {
                selectedNote.LastUpdatedDate = DateTime.Now;
                db.UpdateSelectedNote(selectedNote);
                MessageBox.Show("Note is saved!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteSelectedNote(Note selectedNote)
        {
            try
            {
                db.DeleteSelectedNote(selectedNote);
                MessageBox.Show("Successfully deleted!!");
                _notes.Remove(SelectedNote);
                NotesCount = _notes.Count.ToString();
                SelectedNote = _notes.Count > 0 ? _notes[0] : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CreateNewNote()
        {
            Note newNote = new Note(_currentUser.Email);

            try
            {
                db.CreateNewNote(newNote);
                MessageBox.Show("Successfully created new note!");
                _notes.Add(newNote);
                NotesCount = _notes.Count.ToString();
                SelectedNote = _notes.Last();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Events
        #endregion
    }
}