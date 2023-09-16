using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using Task_Managment.ViewModels;

namespace Task_Managment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel vm = new MainWindowViewModel();
            this.DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = new Action(this.Close);
            if (vm.HideAction == null) vm.HideAction = new Action(this.Hide);
            if (vm.ShowAction == null) vm.ShowAction = new Action(this.Show);
        }
       

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
