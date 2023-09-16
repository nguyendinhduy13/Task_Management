using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Task_Managment.Views
{
    /// <summary>
    /// Interaction logic for wndVerifyCode.xaml
    /// </summary>
    public partial class wndVerifyCode : Window
    {
        private string generatedCode;

        public wndVerifyCode(string verifyCode)
        {
            InitializeComponent();

            Closing += wndVerifyCode_onClosing;
            this.generatedCode = verifyCode;
        }

        private void wndVerifyCode_onClosing(object sender, CancelEventArgs e)
        {
            string inputCode = tbVerifyCode.Text;
            DialogResult = generatedCode == inputCode;
        }
    }
}
