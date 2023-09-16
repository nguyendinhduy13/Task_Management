using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Task_Managment.DataAccess;
using Task_Managment.Models;
using Task_Managment.UserControls;
using Task_Managment.ViewModels;

namespace Task_Managment.Views
{
    /// <summary>
    /// Interaction logic for eventwindow.xaml
    /// </summary>
    public partial class eventwindow : Window
    {
        public eventwindow()
        {
            InitializeComponent();
            tbxdate.Text = CalendarViewModel.static_month + "/" + CalendarViewModel.date + "/" + CalendarViewModel.static_year;
        }
    }
}
