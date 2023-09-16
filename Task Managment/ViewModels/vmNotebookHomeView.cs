using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Task_Managment.Models;

namespace Task_Managment.ViewModels
{
    public class vmNotebookHomeView : INotifyPropertyChanged
    {
        private Members mCurrentUser;

        private DataAccess db = DataAccess.Instance;

        public ObservableCollection<NotebookModel> mNotebooks { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _notebooksCount;
        public string NotebooksCount
        {
            get { return _notebooksCount; }
            set
            {
                _notebooksCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NotebooksCount"));
            }
        }

        private bool _isTitleSorted;
        private bool _isCreatedDateSorted;
        private bool _isLastUpdatedDateSorted;

        public bool IsTitleSorted
        {
            get { return _isTitleSorted; }
            set
            {
                _isTitleSorted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsTitleSorted"));
            }
        }
        public bool IsCreatedDateSorted
        {
            get { return _isCreatedDateSorted; }
            set
            {
                _isCreatedDateSorted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsCreatedDateSorted"));
            }
        }
        public bool IsLastUpdatedDateSorted
        {
            get { return _isLastUpdatedDateSorted; }
            set
            {
                _isLastUpdatedDateSorted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLastUpdatedDateSorted"));
            }
        }

        public ICommand NoteSwichCommand { get; set; }
        public ICommand CreateNewNoteBookCommand { get; set; }
        public ICommand EnableRenameSelectedNoteBookCommand { get; set; }
        public ICommand DeleteSelectedNoteBookCommand { get; set; }
        public ICommand SortByTitleCmd { get; set; }
        public ICommand SortByCreatedDateCmd { get; set; }
        public ICommand SortByLastUpdatedDateCmd { get; set; }
        public ICommand RenameNotebookCmd { get; set; }

        private NotebookModel _selectedNotebook;

        //filter
        #region FILTER
        private CollectionViewSource cvsNotebooks;
        public ICollectionView AllNotebooks
        {
            get { return cvsNotebooks.View; }
        }

        private string _searchBoxContent;
        public string SearchBoxContent
        {
            get { return _searchBoxContent; }
            set
            {
                _searchBoxContent = value;
                cvsNotebooks.View.Refresh();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchBoxContent"));
            }
        }

        private void CvsNotebooks_Filter(object sender, FilterEventArgs e)
        {
            NotebookModel notebook = (NotebookModel)e.Item;

            if (string.IsNullOrWhiteSpace(_searchBoxContent) || _searchBoxContent.Length == 0)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = notebook._name.IndexOf(_searchBoxContent, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }
        #endregion

        public NotebookModel SelectedNotebook
        {
            get => _selectedNotebook;
            set
            {
                if (value != null)
                {
                    _selectedNotebook = value;
                    //DateTime temp = _selectedTime;
                    //temp = temp.AddHours(_selectedClockTime.Hour - temp.Hour);
                    //temp = temp.AddMinutes(_selectedClockTime.Minute - temp.Minute);
                    //temp = temp.AddSeconds(0 - temp.Second);
                    //if (SelectedTask != null)
                    //    this.SelectedTime = temp;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNotebook"));

                }

            }
        }

        public vmNotebookHomeView()
        {
            MainWindow newWindow = new MainWindow();

            StartWindowViewModel startWindowViewModel = new StartWindowViewModel();
            Members currentUser = startWindowViewModel.getCurrentUser();
            
            Initialize(currentUser);

            List<NotebookModel> tempList = db.GetAllNotebooksOfMember(currentUser);

            if (tempList.Count > 0)
            {
                this.mNotebooks = new ObservableCollection<NotebookModel>();
                foreach (NotebookModel temp in tempList)
                {
                    this.mNotebooks.Add(temp);
                }
                for (int i = 0; i < this.mNotebooks.Count; i++)
                {
                    this.mNotebooks[i]._collection = db.GetAllNotesFromNotebook(this.mNotebooks[i]);
                }
            }
            else
            {
                NotebookModel notebookModel = new NotebookModel(currentUser.Email, "First notebook");
                db.CreateNewNotebook(notebookModel);
                this.mNotebooks = new ObservableCollection<NotebookModel> { notebookModel };
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("mNotebooks"));
            }

            // =================
            cvsNotebooks = new CollectionViewSource();
            cvsNotebooks.Source = mNotebooks;
            cvsNotebooks.Filter += CvsNotebooks_Filter;
            // =================

            InitCommand();
        }

        #region Commands

        private void InitCommand()
        {
            NoteSwichCommand = new RelayCommand<ListViewItem>(p => true, p => noteSwitch());
            CreateNewNoteBookCommand = new RelayCommand<Button>(p => true, p => CreateNewNoteBook());
            EnableRenameSelectedNoteBookCommand = new RelayCommand<ListViewItem>(p => true, p => EnableRenameSelectedNoteBook(p));
            DeleteSelectedNoteBookCommand = new RelayCommand<ListViewItem>(p => true, p => DeleteSelectedNoteBook(p));
            SortByTitleCmd = new RelayCommand<ListView>(p => true, p => SortByTitle(p));
            SortByCreatedDateCmd = new RelayCommand<ListView>(p => true, p => SortByCreatedDate(p));
            SortByLastUpdatedDateCmd = new RelayCommand<ListView>(p => true, p => SortByLastUpdatedDate(p));
            RenameNotebookCmd = new RelayCommand<NotebookModel>(p => true, p => RenameNotebook(p));
        }

        private void RenameNotebook(NotebookModel p)
        {
            if (p == null) return;
            else db.UpdateSelectedNotebook(p);
        }

        private void DeleteSelectedNoteBook(ListViewItem p)
        {
            try
            {
                NotebookModel selectedNotebook = p.Content as NotebookModel;
                if (selectedNotebook._name.Equals("First notebook"))
                {
                    MessageBox.Show("First notebook can't be delete!!");
                    return;
                }
                db.DeleteSelectedNotebook(selectedNotebook);
                MessageBox.Show("Successfully deleted!!");
                mNotebooks.Remove(selectedNotebook);
                NotebooksCount = mNotebooks.Count.ToString();
                SelectedNotebook = mNotebooks.Count > 0 ? mNotebooks[0] : null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void EnableRenameSelectedNoteBook(ListViewItem p)
        {
            p.IsEnabled = true;
        }

        private void CreateNewNoteBook()
        {
            NotebookModel notebookModel = new NotebookModel(mCurrentUser.Email, "Untitled");
            db.CreateNewNotebook(notebookModel);
            if (this.mNotebooks == null)
                this.mNotebooks = new ObservableCollection<NotebookModel> { notebookModel };
            else
                this.mNotebooks.Add(notebookModel);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("mNotebooks"));
        }

        private void noteSwitch()
        {
        }

        #endregion

        #region Functions
        private void Initialize(Members currentUser)
        {
            mCurrentUser = currentUser;
        }


        private void SortByCreatedDate(ListView p)
        {
            if (p.Items.SortDescriptions.Count != 0 &&
                p.Items.SortDescriptions.Last().PropertyName == "_createdDate" &&
                p.Items.SortDescriptions.Last().Direction == ListSortDirection.Ascending)
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("_createdDate", ListSortDirection.Descending));
            }
            else
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("_createdDate", ListSortDirection.Ascending));
            }

            IsCreatedDateSorted = !IsCreatedDateSorted;
            IsTitleSorted = IsLastUpdatedDateSorted = false;
        }

        private void SortByLastUpdatedDate(ListView p)
        {
            if (p.Items.SortDescriptions.Count != 0 &&
                p.Items.SortDescriptions.Last().PropertyName == "_lastUpdateDate" &&
                p.Items.SortDescriptions.Last().Direction == ListSortDirection.Ascending)
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("_lastUpdateDate", ListSortDirection.Descending));
            }
            else
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("_lastUpdateDate", ListSortDirection.Ascending));
            }

            IsLastUpdatedDateSorted = !IsLastUpdatedDateSorted;
            IsTitleSorted = IsCreatedDateSorted = false;
        }

        private void SortByTitle(ListView p)
        {
            if (p.Items.SortDescriptions.Count != 0 &&
                p.Items.SortDescriptions.Last().PropertyName == "_name" &&
                p.Items.SortDescriptions.Last().Direction == ListSortDirection.Ascending)
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("_name", ListSortDirection.Descending));
            }
            else
            {
                p.Items.SortDescriptions.Clear();
                p.Items.SortDescriptions.Add(new SortDescription("_name", ListSortDirection.Ascending));
            }

            IsTitleSorted = !IsTitleSorted;
            IsCreatedDateSorted = IsLastUpdatedDateSorted = false;
        }
        #endregion
    }
}
